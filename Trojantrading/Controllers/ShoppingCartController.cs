using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Trojantrading.Models;
using Trojantrading.Repositories;
using Trojantrading.Util;
using System.Collections.Generic;

namespace Trojantrading.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;

        public ShoppingCartController(ShoppingCartRepository shoppingCartRepository, ProductRepository productRepository)
        {
            this._shoppingCartRepository = shoppingCartRepository;
            this._productRepository = productRepository;
        }

        [HttpGet("GetShoppingCart")]
        [NoCache]
        [ProducesResponseType(typeof(ShoppingCart), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult GetShoppingCart(int userId)
        {
            return Ok(_shoppingCartRepository.GetCart(userId));
        }

        [HttpGet("AddShoppingCart")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult AddShoppingCart(int userId)
        {
            return Ok(_shoppingCartRepository.AddShoppingCart(userId));
        }

        [HttpPost("UpdateShoppingCartUpdateShoppingCart")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult UpdateShoppingCart(int userId, [FromBody]ShoppingItem shoppingItem)
        {
            return Ok(_shoppingCartRepository.UpdateShoppingCart(userId, shoppingItem));
        }
    }
}