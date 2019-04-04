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
    [Authorize(Roles = "Admin, User")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(ProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public IActionResult GetProducts()
        {
            return null;
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