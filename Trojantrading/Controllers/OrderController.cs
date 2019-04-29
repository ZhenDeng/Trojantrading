using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Trojantrading.Repositories;
using Trojantrading.Models;
using Trojantrading.Util;

namespace Trojantrading.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController:Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("GetOrdersByUserID")]
        [ProducesResponseType(typeof(Order[]), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult GetOrdersByUserID(string userId)
        {
            try
            {
                int id = int.Parse(userId);
                var results = _orderRepository.GetOrdersByUserID(id);

                return Ok(results.ToArray());
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse { Status = "false", Message = ex.Message });
            }
        }

        [HttpPost("AddOrder")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult AddOrder([FromBody]ShoppingCart cart)
        {
            return Ok(_orderRepository.AddOrder(cart));
        }

        [HttpGet("GetOrderWithUser")]
        [NoCache]
        [ProducesResponseType(typeof(List<Order>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult GetOrderWithUser(int userId)
        {
            return Ok(_orderRepository.GetOrderWithUser(userId));
        }
    }
}