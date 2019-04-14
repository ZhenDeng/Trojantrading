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
    public class ShoppingCartController:Controller
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
        public IActionResult GetShoppingCart(int id)
        {
            return Ok(_shoppingCartRepository.GetCart(id));
        }

        public IActionResult Add(int productId, int number)
        {
            var shoppingCartId = 12;
            var shoppingCart = _shoppingCartRepository.GetCart(shoppingCartId);
            var shoppingItem = new ShoppingItem();
            var product = _productRepository.Get(productId);
            shoppingItem.Product = product;
            shoppingItem.Amount = number;
            _shoppingCartRepository.UpdateShoppingItems(shoppingCart);
            return null;
        }

        public IActionResult Update(int shoppingItemId, int number)
        {
           
            return null;
        }

        public IActionResult Remove(int productId)
        {
            var product = _productRepository.ToString();
            return null;
        }

        public IActionResult Empty()
        {
            var shoppingCardId = 12;
            _shoppingCartRepository.Empty(shoppingCardId);
            return null;
        }
    }
}