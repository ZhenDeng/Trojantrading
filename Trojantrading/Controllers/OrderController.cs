using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Trojantrading.Repositories;
using Trojantrading.Models;
using Trojantrading.Util;
using System.Threading.Tasks;

namespace Trojantrading.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController:Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(
            IOrderRepository orderRepository
          )
        {
            _orderRepository = orderRepository;
        }

        [HttpPost("AddOrder")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> AddOrder(double gst, double priceExclGst, double discount, [FromBody]ShoppingCart cart)
        {
            return Ok(await _orderRepository.AddOrder(cart, gst, priceExclGst, discount));
        }

        [HttpPost("UpdateOrder")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> UpdateOrder([FromBody]Order order)
        {
            return Ok(await _orderRepository.UpdateOrder(order));
        }

        [HttpGet("GetOrdersWithShoppingItems")]
        [NoCache]
        [ProducesResponseType(typeof(Order), 200)]
        public async Task<IActionResult> GetOrdersWithShoppingItems(int orderId)
        {
            var result = await _orderRepository.GetOrdersWithShoppingItems(orderId);
            return Ok(result);
        }

        [HttpGet("GetOrdersByUserID")]
        [NoCache]
        [ProducesResponseType(typeof(List<Order>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> GetOrdersByUserID(string userId, string dateFrom, string dateTo)
        {
            try
            {
                List<Order> results = new List<Order>();
                
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    int id = int.Parse(userId);
                    results = await _orderRepository.GetOrdersByUserID(id, dateFrom, dateTo);
                }
                else
                {
                    results = await _orderRepository.GetOrdersByDate(dateFrom, dateTo);
                }


                return Ok(results);
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse { Status = "false", Message = ex.Message });
            }
        }

        [HttpGet("DeleteOrder")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var result = await _orderRepository.DeleteOrder(orderId);
            return Ok(result);
        }
    }
}