using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.Repositories;
using static TrackwiseAPI.Controllers.OrderController;
using TrackwiseAPI.Models.DataTransferObjects;
using TrackwiseAPI.Models.ViewModels;
using TrackwiseAPI.Controllers;
using TrackwiseAPI.Models.Email;
using System.Runtime.Intrinsics.X86;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly TwDbContext _dbContext;
        private readonly IPaymentRepository _paymentRepository;
        private readonly MailController _mailController;
        private readonly IAuditRepository _auditRepository;
        private readonly IVATRepository _VATRepository;

        public OrderController(
            TwDbContext dbContext,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IPaymentRepository paymentRepository,
            UserManager<AppUser> userManager,
            MailController mailController,
            IAuditRepository auditRepository,
            IVATRepository vATRepository)
        {
            _dbContext = dbContext;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _paymentRepository = paymentRepository;
            _mailController = mailController;
            _auditRepository = auditRepository;
            _VATRepository = vATRepository;
        }

        [HttpGet]
        [Route("GetAllOrders")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderRepository.GetAllOrdersAsync();
                var orderDTOs = orders.Select(order => new OrderDTO
                {
                    Order_ID = order.Order_ID,
                    Date = order.Date,
                    Total = order.Total,
                    Status = order.Status,
                    Customer_ID = order.Customer_ID,
                    Customer = new CustomerDTO
                    {
                        Customer_ID = order.Customer_ID,
                        Name = order.Customer.Name,
                        LastName = order.Customer.LastName,
                    },
                    OrderLines = order.OrderLines.Select(ol => new OrderLineDTO
                    {
                        Order_line_ID = ol.Order_line_ID,
                        Quantity = ol.Quantity,
                        SubTotal = ol.SubTotal,
                        Product = new ProductDTO
                        {
                            Product_ID = ol.Product.Product_ID,
                            Product_Name = ol.Product.Product_Name,
                            Product_Description = ol.Product.Product_Description,
                            Product_Price = ol.Product.Product_Price,
                            Quantity = (int)ol.Quantity,
                            Product_Type = new ProductTypeDTO 
                            {
                                Product_Type_ID  = ol.Product.ProductType.Product_Type_ID,
                                Name = ol.Product.ProductType.Name,
                                Description = ol.Product.ProductType.Description,
                            },
                            Product_Category = new ProductCategoryDTO
                            {
                                Product_Category_ID = ol.Product.ProductCategory.Product_Category_ID,
                                Name = ol.Product.ProductCategory.Name,
                                Description = ol.Product.ProductCategory.Description,
                            }
                            // Add other product properties as needed
                        }
                    }).ToList()
                }).ToList();

                return Ok(orderDTOs);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("CreateOrder")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
        public async Task<IActionResult> Checkout(CheckoutRequest checkoutRequest)
        {

            // Convert OrderVM to OrderDTO
            var orderDto = new OrderDTO
            {
                OrderLines = checkoutRequest.orderVM.OrderLines.Select(ol => new OrderLineDTO
                {
                    Product = new ProductDTO { Product_ID = ol.ProductId },
                    Quantity = ol.Quantity
                }).ToList()
            };
            // Retrieve the authenticated user's email address
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Create Order", CreatedDate = DateTime.Now, User = userEmail };
           
            var VAT = await _VATRepository.GetVATAsync();

            // Query the customer repository to get the customer ID
            var customer = await _userManager.FindByEmailAsync(userEmail);

            if (customer == null)
            {
                return BadRequest("Customer not found");
            }

            var customerId = customer.Id;

            if (string.IsNullOrEmpty(customerId))
            {
                return BadRequest("Customer ID not found");
            }

            decimal total = 0;

            foreach (var orderLine in orderDto.OrderLines)
            {
                var product = await _productRepository.GetProductAsync(orderLine.Product.Product_ID);
                decimal subtotal = (decimal)(orderLine.Quantity * (product.Product_Price + (product.Product_Price * (double)VAT.VAT_Amount)));
                total += subtotal;
            }

            var order = new Order
            {
                Order_ID = Guid.NewGuid().ToString(),
                Date = DateTime.Now,
                Total = (double)total,
                Status = "Ordered",
                Customer_ID = customerId,
                OrderLines = new List<Order_Line>()
            };


            var orderLinesForInvoice = new List<OrderLineInvoiceDTO>();

            foreach (var orderLineDto in orderDto.OrderLines)
            {
                var product = await _productRepository.GetProductAsync(orderLineDto.Product.Product_ID);
                if (product == null)
                {
                    return NotFound(); // Handle product not found scenario
                }

                if (product.Quantity < orderLineDto.Quantity)
                {
                    return BadRequest("Insufficient stock for the product"); // Handle insufficient stock scenario
                }

                var orderLine = new Order_Line
                {
                    Order_line_ID = Guid.NewGuid().ToString(),
                    Productid = orderLineDto.Product.Product_ID,
                    Quantity = orderLineDto.Quantity,
                    SubTotal = orderLineDto.Quantity * (product.Product_Price + (product.Product_Price * (double)VAT.VAT_Amount))
                };

                order.OrderLines.Add(orderLine);
                // Add order line details to the list for the invoice
                var subtotal = (decimal)(orderLineDto.Quantity * product.Product_Price);
                orderLinesForInvoice.Add(new OrderLineInvoiceDTO
                {
                    ProductName = product.Product_Name,
                    Quantity = orderLineDto.Quantity,
                    SubTotal = (double)subtotal,
                    // Add other relevant properties here
                });

                // Update the product quantity
                product.Quantity -= orderLineDto.Quantity;
            }

            if (checkoutRequest == null || checkoutRequest.newCard == null)
            {
                return BadRequest("Invalid request data. The 'checkoutRequest' or 'checkoutRequest.NewCard' is null.");
            }

            // Set the Order_ID in the NewCard model to the newly created order's ID
            checkoutRequest.newCard.Order_ID = order.Order_ID;
            checkoutRequest.newCard.Amount = (decimal)order.Total;

            // Call the AddNewCard method to process the payment and pass the newCard model
          
            var paymentResponse = await _paymentRepository.AddNewCard(checkoutRequest.newCard);
            
            var InvoiceNumber = Guid.NewGuid().ToString();
            var Invoice1 = new newInvoice { InvoiceNumber = InvoiceNumber, Email = userEmail, 
                Name = customer.UserName, Total =  order.Total,
                OrderLines = orderLinesForInvoice,
                OrderNumber = order.Order_ID,
                OrderDate = order.Date
            };

            var mail = await _mailController.SendInvoiceEmail(Invoice1);
            
            // Save the order and update the product quantities
            _dbContext.Orders.Add(order);


            /*            _dbContext.Orders.Add(order);*/
            await _productRepository.SaveChangesAsync();
            await _dbContext.SaveChangesAsync();
            _auditRepository.Add(audit);
            await _auditRepository.SaveChangesAsync();
            
            return Ok();
        }

        [HttpGet]
        [Route("GetOrder/{orderId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Customer")]
        public async Task<IActionResult> GetOrderAsync(string orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderAsync(orderId);

                if (order == null)
                    return NotFound("Order does not exist.");

                var orderDto = new OrderDTO
                {
                    Order_ID = order.Order_ID,
                    Date = order.Date,
                    Total = order.Total,
                    Status = order.Status,
                    Customer_ID = order.Customer_ID,
                    Customer = new CustomerDTO
                    {
                        Customer_ID = order.Customer_ID,
                        Name = order.Customer.Name,
                        LastName = order.Customer.LastName,
                    },
                    OrderLines = order.OrderLines.Select(ol => new OrderLineDTO
                    {
                        Order_line_ID = ol.Order_line_ID,
                        Quantity = ol.Quantity,
                        SubTotal = ol.SubTotal,
                        Product = new ProductDTO
                        {
                            Product_ID = ol.Product.Product_ID,
                            Product_Name = ol.Product.Product_Name,
                            Product_Description = ol.Product.Product_Description,
                            Product_Price = ol.Product.Product_Price,
                            Quantity = (int)ol.Quantity,
                            Product_Type = new ProductTypeDTO
                            {
                                Product_Type_ID = ol.Product.ProductType.Product_Type_ID,
                                Name = ol.Product.ProductType.Name,
                                Description = ol.Product.ProductType.Description,
                            },
                            Product_Category = new ProductCategoryDTO
                            {
                                Product_Category_ID = ol.Product.ProductCategory.Product_Category_ID,
                                Name = ol.Product.ProductCategory.Name,
                                Description = ol.Product.ProductCategory.Description,
                            }
                            // Add other product properties as needed
                        }
                        // Map other properties as needed
                    }).ToList()
                };

                return Ok(orderDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }


        [HttpGet]
        [Route("GetAllCustomerOrders")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Customer")]

        public async Task<IActionResult> GetAllCustomerOrders()
        {
            try
            {
                // Retrieve the authenticated user's email address
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                // Query the customer repository to get the customer ID
                var customer = await _userManager.FindByEmailAsync(userEmail);

                if (customer == null)
                {
                    return BadRequest("Customer not found");
                }

                var customerId = customer.Id;

                if (string.IsNullOrEmpty(customerId))
                {
                    return BadRequest("Customer ID not found");
                }

                var result = await _orderRepository.GetAllCustomerOrdersAsync(customerId);

                var orderDTOs = result.Select(order => new OrderDTO
                {
                    Order_ID = order.Order_ID,
                    Date = order.Date,
                    Total = order.Total,
                    Status = order.Status,
                    Customer_ID = order.Customer_ID,
                    Customer = new CustomerDTO
                    {
                        Customer_ID = order.Customer_ID,
                        Name = order.Customer.Name,
                        LastName = order.Customer.LastName,
                    },
                    OrderLines = order.OrderLines.Select(ol => new OrderLineDTO
                    {
                        Order_line_ID = ol.Order_line_ID,
                        Quantity = ol.Quantity,
                        SubTotal = ol.SubTotal,
                        Product = new ProductDTO
                        {
                            Product_ID = ol.Product.Product_ID,
                            Product_Name = ol.Product.Product_Name,
                            Product_Description = ol.Product.Product_Description,
                            Product_Price = ol.Product.Product_Price,
                            Quantity = (int)ol.Quantity,
                            Product_Type = new ProductTypeDTO
                            {
                                Product_Type_ID = ol.Product.ProductType.Product_Type_ID,
                                Name = ol.Product.ProductType.Name,
                                Description = ol.Product.ProductType.Description,
                            },
                            Product_Category = new ProductCategoryDTO
                            {
                                Product_Category_ID = ol.Product.ProductCategory.Product_Category_ID,
                                Name = ol.Product.ProductCategory.Name,
                                Description = ol.Product.ProductCategory.Description,
                            }
                            // Add other product properties as needed
                        }
                    }).ToList()
                }).ToList();

                if (result == null) return NotFound("Order does not exist");

                return Ok(orderDTOs);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

        [HttpPut]
        [Route("CancelOrder/{orderId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
        public async Task<IActionResult> CancelOrder(string orderId)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Cancel Order", CreatedDate = DateTime.Now, User = userEmail };
            try
            {
                var existingOrder = await _orderRepository.GetOrderAsync(orderId);
                if (existingOrder == null) 
                    return NotFound($"The order does not exist");

                if (existingOrder.Status == "Cancelled")
                    return NotFound($"The order is already cancelled");

                if (existingOrder.Status == "Collected")
                    return NotFound($"The order is already collected, cannot cancel");

                existingOrder.Status = "Cancelled";

                foreach (var orderLineDto in existingOrder.OrderLines)
                {
                    var product = await _productRepository.GetProductAsync(orderLineDto.Product.Product_ID);
                    if (product == null)
                    {
                        return NotFound(); // Handle product not found scenario
                    }


                    // Update the product quantity
                    product.Quantity += orderLineDto.Quantity;

                    _productRepository.Update(product);
                }

                if (await _orderRepository.SaveChangesAsync())
                {
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
                    return Ok(existingOrder);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpPut]
        [Route("CollectOrder/{orderId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> CollectOrder(string orderId)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var auditId = Guid.NewGuid().ToString();
            var audit = new Audit { Audit_ID = auditId, Action = "Collect Order", CreatedDate = DateTime.Now, User = userEmail };
            try
            {
                var existingOrder = await _orderRepository.GetOrderAsync(orderId);
                if (existingOrder == null)
                    return NotFound($"The order does not exist");

                if (existingOrder.Status == "Cancelled")
                    return NotFound($"The order is already cancelled, cannot collect");

                if (existingOrder.Status == "Collected")
                    return NotFound($"The order is already collected, cannot collect");

                existingOrder.Status = "Collected";

                var confirmationMail = new ConfirmEmail
                {
                    Email = user.Email,
                    Name = user.UserName,

                    ConfirmationLink = "http://localhost:4200/Authentication/confirm-email/" + encodedToken + "/" + user.UserName
                };

                await _mailController.ConfirmEmail(confirmationMail);


                if (await _orderRepository.SaveChangesAsync())
                {
                    _auditRepository.Add(audit);
                    await _auditRepository.SaveChangesAsync();
                    return Ok(existingOrder);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

    }
}
