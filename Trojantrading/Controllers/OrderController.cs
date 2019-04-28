using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Trojantrading.Repositories;
using Trojantrading.Models;

namespace Trojantrading.Controllers
{
    [Route("[Controller]")]
    [Authorize(Roles="Admin, User")]
    public class OrderController:Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(OrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
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

        public IActionResult CreateOrder()
        {
            var order = new Order();
            var shoppingCart = new ShoppingCart();
            return null;
        }

        public IActionResult DeleteOrder()
        {
            return null;
        }

        public IActionResult UpdateOrder()
        {
            return null;
        }

        public IActionResult EditOrder()
        {
            return null;
        }

        public IActionResult GetOrders()
        {
            return null;
        }
    }
}