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
        
        [Route("/addproduct")]
        public IActionResult AddProduct(string name, double originalPrice, double agentPrice, 
            double resellerPrice, string category)
        {
            try
            {
                var product = new Product();
                product.Name = name;
                product.OriginalPrice = originalPrice;
                product.CreatedDate = new DateTime();
                product.AgentPrice = agentPrice;
                product.ResellerPrice = resellerPrice;
                _productRepository.Add(product);
                return Ok(new ApiResponse
                {
                    Status = "success",
                    Message = "Successfully add product"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = "fail",
                    Message = "Fail to add product"
                });
            }
        }
        
    }
}