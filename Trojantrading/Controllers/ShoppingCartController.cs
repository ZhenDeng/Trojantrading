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

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet("GetShoppingCart")]
        [NoCache]
        [ProducesResponseType(typeof(ShoppingCart), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult GetShoppingCart(int userId)
        {
            return Ok(_shoppingCartRepository.GetCartWithShoppingItems(userId));
        }

        [HttpGet("GetCartInIdWithShoppingItems")]
        [NoCache]
        [ProducesResponseType(typeof(ShoppingCart), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult GetCartInIdWithShoppingItems(int shoppingCartId)
        {
            return Ok(_shoppingCartRepository.GetCartInIdWithShoppingItems(shoppingCartId));
        }

        [HttpGet("AddShoppingCart")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult AddShoppingCart(int userId)
        {
            return Ok(_shoppingCartRepository.AddShoppingCart(userId));
        }

        [HttpPost("UpdateShoppingCart")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult UpdateShoppingCart(int userId, [FromBody]ShoppingItem shoppingItem)
        {
            return Ok(_shoppingCartRepository.UpdateShoppingCart(userId, shoppingItem));
        }

        [HttpDelete("deleteShoppingItem")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult deleteShoppingItem(int shoppingItemId)
        {
            return Ok(_shoppingCartRepository.deleteShoppingItem(shoppingItemId));
        }
    }
}