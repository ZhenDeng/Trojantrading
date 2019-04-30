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
        public IActionResult GetAllProducts()
        {
            try
            {
                List<Product> products = new List<Product>();
                products = _productRepository.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse { Status = "false", Message = ex.Message });

            }
        }
        
        [HttpPost("AddProduct")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult AddProduct([FromBody]Product product)
        {
            return Ok(_productRepository.Add(product));
        }

        [HttpPost("UpdateProduct")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult UpdateProduct([FromBody]Product product)
        {
            return Ok(_productRepository.UpdateProduct(product));
        }
    }
}