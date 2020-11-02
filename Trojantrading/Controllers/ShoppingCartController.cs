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
        public async Task<IActionResult> GetShoppingCart(int userId)
        {
            return Ok(await _shoppingCartRepository.GetCartWithShoppingItems(userId));
        }

        [HttpGet("GetCartInIdWithShoppingItems")]
        [NoCache]
        [ProducesResponseType(typeof(ShoppingCart), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> GetCartInIdWithShoppingItems(int shoppingCartId)
        {
            return Ok(await _shoppingCartRepository.GetCartInIdWithShoppingItems(shoppingCartId));
        }

        [HttpGet("AddShoppingCart")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> AddShoppingCart(int userId)
        {
            return Ok(await _shoppingCartRepository.AddShoppingCart(userId));
        }

        [HttpPost("UpdateShoppingCart")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> UpdateShoppingCart(int userId, [FromBody]ShoppingItem shoppingItem)
        {
            return Ok(await _shoppingCartRepository.UpdateShoppingCart(userId, shoppingItem));
        }

        [HttpGet("UpdateShoppingCartPaymentMethod")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> UpdateShoppingCartPaymentMethod(int userId, string selectedPayment)
        {
            return Ok(await _shoppingCartRepository.UpdateShoppingCartPaymentMethod(userId, selectedPayment));
        }

        [HttpDelete("deleteShoppingItem")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> deleteShoppingItem(int shoppingItemId)
        {
            return Ok(await _shoppingCartRepository.deleteShoppingItem(shoppingItemId));
        }
    }
}