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

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
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

        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> Checkout(OrderDto orderDto)
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

            decimal total = 0;

            foreach (var orderLine in orderDto.OrderLines)
            {
                var product = await _productRepository.GetProductAsync(orderLine.ProductId);
                decimal subtotal = (decimal)(orderLine.Quantity * product.Product_Price);
                total += subtotal;
            }

            var order = new Order
            {
                Order_ID = GenerateOrderId(),
                Date = DateTime.Now,
                Total = (double)total,
                Status = "Ordered",
                Customer_ID = customerId,
                OrderLines = new List<Order_Line>()
            };

            foreach (var orderLineDto in orderDto.OrderLines)
            {
                var product = await _productRepository.GetProductAsync(orderLineDto.ProductId);
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
                    Order_line_ID = GenerateOrderLineId(),
                    Productid = orderLineDto.ProductId,
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


        private string GenerateOrderId()
        {
            return Guid.NewGuid().ToString();
            // Generate a unique order ID logic goes here
        }

        private string GenerateOrderLineId()
        {
            return Guid.NewGuid().ToString();
            // Generate a unique order line ID logic goes here
        }

        public class OrderDto
        {
            public List<OrderLineDto> OrderLines { get; set; }
        }

        public class OrderLineDto
        {
            public string ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
