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