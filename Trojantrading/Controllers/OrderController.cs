using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Trojantrading.Repositories;
using Trojantrading.Models;
using Trojantrading.Util;
using OfficeOpenXml;
using System.IO;
using Trojantrading.DAL;

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
        public IActionResult AddOrder([FromBody]ShoppingCart cart)
        {
            return Ok(_orderRepository.AddOrder(cart));
        }

        [HttpPost("UpdateOrder")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult UpdateOrder([FromBody]Order order)
        {
            return Ok(_orderRepository.UpdateOrder(order));
        }

        [HttpPost("GetOrdersWithShoppingItems")]
        [NoCache]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult GetOrdersWithShoppingItems(int orderId)
        {
            return Ok(_orderRepository.GetOrdersWithShoppingItems(orderId));
        }

        [HttpGet("GetOrdersByUserID")]
        [NoCache]
        [ProducesResponseType(typeof(List<Order>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult GetOrdersByUserID(string userId, string dateFrom, string dateTo)
        {
            try
            {
                List<Order> results = new List<Order>();
                
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    int id = int.Parse(userId);
                    results = _orderRepository.GetOrdersByUserID(id, dateFrom, dateTo);
                }
                else
                {
                    results = _orderRepository.GetOrdersByDate(dateFrom, dateTo);
                }


                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse { Status = "false", Message = ex.Message });
            }
        }

        [HttpGet("DeleteOrder")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public IActionResult DeleteOrder(int orderId)
        {
            var result = _orderRepository.DeleteOrder(orderId);
            return Ok(result);
        }
    }
}