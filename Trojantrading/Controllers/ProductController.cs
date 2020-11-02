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
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;

        public ProductController(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IUserRepository userRepository
            )
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }


        [HttpGet("GetAllProducts")]
        [NoCache]
        [ProducesResponseType(typeof(List<Product>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                List<Product> products = new List<Product>();
                products = await _productRepository.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse { Status = "false", Message = ex.Message });

            }
        }
        
        [HttpPost("AddProduct")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> AddProduct([FromBody]Product product)
        {
            return Ok(await _productRepository.Add(product));
        }

        [HttpPost("UpdateProduct")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> UpdateProduct([FromBody]Product product)
        {
            return Ok(await _productRepository.UpdateProduct(product));
        }

        [HttpPost("UpdatePackagingList")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> UpdatePackagingList(int productId, [FromBody]List<PackagingList> lists)
        {
            return Ok(await _productRepository.UpdatePackagingList(lists, productId));
        }

        [HttpPost("DeleteProduct")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> DeleteProduct([FromBody]Product product)
        {
            return Ok(await _productRepository.DeleteProduct(product));
        }
    }
}