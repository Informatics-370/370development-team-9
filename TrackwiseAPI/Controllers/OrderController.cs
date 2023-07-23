﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        public OrderController(TwDbContext dbContext, IProductRepository productRepository, IOrderRepository orderRepository, UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetAllOrders")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllAdmins()
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
        public async Task<IActionResult> Checkout(OrderVM orderVM)
        {
            // Convert OrderVM to OrderDTO
            var orderDto = new OrderDTO
            {
                OrderLines = orderVM.OrderLines.Select(ol => new OrderLineDTO
                {
                    Product = new ProductDTO { Product_ID = ol.ProductId },
                    Quantity = ol.Quantity
                }).ToList()
            };
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

            decimal total = 0;

            foreach (var orderLine in orderDto.OrderLines)
            {
                var product = await _productRepository.GetProductAsync(orderLine.Product.Product_ID);
                decimal subtotal = (decimal)(orderLine.Quantity * product.Product_Price);
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
                    SubTotal = product.Product_Price * orderLineDto.Quantity
                };

                order.OrderLines.Add(orderLine);

                // Update the product quantity
                product.Quantity -= orderLineDto.Quantity;
            }

            // Save the order and update the product quantities
            _dbContext.Orders.Add(order);
            await _productRepository.SaveChangesAsync();
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("GetAllCustomerOrdersAsync")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Customer")]

        public async Task<IActionResult> GetAllCustomerOrdersAsync()
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

    }
}
