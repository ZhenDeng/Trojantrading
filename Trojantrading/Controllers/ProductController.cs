﻿using System;
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
        [ProducesResponseType(typeof(Product[]), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult GetAllProducts()
        {
            try
            {
                List<Product> products = new List<Product>();
                //products = _productRepository.GetAllProducts();
                //dummy data for testing, to be deleted..
                if (products.Count <= 0)
                {
                    products.Add(new Product
                    {
                        Id = 10001,
                        Name = "Winfield",
                        CreatedDate = new DateTime(),
                        OriginalPrice = 35.05,
                        VipOnePrice = 32.11,
                        VipTwoPrice = 31.5,
                        Category = "Winfield Blue",
                        ShoppingItemId = 20001,
                        Status = ""
                    });

                    products.Add(new Product
                    {
                        Id = 10002,
                        Name = "Pall Mall",
                        CreatedDate = new DateTime(),
                        OriginalPrice = 36.05,
                        VipOnePrice = 33.11,
                        VipTwoPrice = 32.5,
                        Category = "25pk",
                        ShoppingItemId = 20002,
                        Status = "New"

                    });

                    products.Add(new Product
                    {
                        Id = 10003,
                        Name = "Rotherman",
                        CreatedDate = new DateTime(),
                        OriginalPrice = 27.05,
                        VipOnePrice = 26.11,
                        VipTwoPrice = 25.5,
                        Category = "273pk",
                        ShoppingItemId = 20001,
                        Status = "Promotion"
                    });

                    products.Add(new Product
                    {
                        Id = 10004,
                        Name = "Marlbro",
                        CreatedDate = new DateTime(),
                        OriginalPrice = 34.05,
                        VipOnePrice = 33.11,
                        VipTwoPrice = 32.5,
                        Category = "Red 25pk",
                        ShoppingItemId = 20001,
                        Status = "SoldOut"
                    });
                }
                return Ok(products.ToArray());
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse { Status = "false", Message = ex.Message });

            }
        }
        
        [Route("/addproduct")]
        public async Task<IActionResult> AddProduct(string name, double originalPrice, double vipOnePrice, 
            double vipTwoPrice, string category)
        {
            var product = new Product();
            product.Name = name;
            product.OriginalPrice = originalPrice;
            product.CreatedDate = new DateTime();
            product.VipOnePrice = vipOnePrice;
            product.VipTwoPrice = vipTwoPrice;
            _productRepository.Add(product);
            return null;
        }
        
    }
}