using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class OrderController : ControllerBase
    {
        private readonly TwDbContext dbContext;

        public OrderController(TwDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("CreateOrder")]
        public IActionResult Checkout(OrderDto orderDto)
        {
            // Map the data from the DTO to your entity models
            decimal total = 0;

            foreach (var orderLine in orderDto.OrderLines)
            {
                decimal subtotal = (decimal)(orderLine.Quantity * orderLine.Price);
                total += subtotal;
            }

            orderDto.Total = (double)total;

            var order = new Order
            {
                Order_ID = GenerateOrderId(), // Generate a unique order ID
                Date = DateTime.Now,
                Total = orderDto.Total,
                Status = "Ordered",
                Customer_ID = orderDto.CustomerId,
                OrderLines = new List<Order_Line>()
            };

            foreach (var orderLineDto in orderDto.OrderLines)
            {
                var orderLine = new Order_Line
                {
                    Order_line_ID = GenerateOrderLineId(), // Generate a unique order line ID
                    Productid = orderLineDto.ProductId,
                    Quantity = orderLineDto.Quantity,
                    SubTotal = orderLineDto.Price * orderLineDto.Quantity
                };

                order.OrderLines.Add(orderLine);
            }

            // Save the order to the database
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();

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
            public string CustomerId { get; set; }
            public double Total { get; set; }
            public List<OrderLineDto> OrderLines { get; set; }
        }

        public class OrderLineDto
        {
            public string ProductId { get; set; }
            public int Quantity { get; set; }
            public double Price { get; set; }
        }
    }
}
