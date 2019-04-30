using System;
using System.Collections.Generic;
using System.Linq;
using Trojantrading.Models;

namespace Trojantrading.Repositories
{

    public interface IProductRepository
    {
        Product Add(Product product);
        Product Get(int id);
        List<Product> GetAllProducts();
        void Delete(int id);
        int GetTotalProducts();
        ApiResponse UpdateProduct(Product product);
    }

    public class ProductRepository:IProductRepository
    {
        private readonly TrojantradingDbContext trojantradingDbContext;
        
        public ProductRepository(TrojantradingDbContext trojantradingDbContext)
        {
            this.trojantradingDbContext = trojantradingDbContext;
        }


        public Product Add(Product product)
        {
            trojantradingDbContext.Products.Add(product);
            trojantradingDbContext.SaveChanges();
            return product;
        }

        public Product Get(int id)
        {
            var product = trojantradingDbContext.Products
                .Where(p => p.Id == id)
                .FirstOrDefault();
            return product;
        }

        public List<Product> GetAllProducts()
        {
            List<Product> allProducts = new List<Product>();

            allProducts = trojantradingDbContext.Products.ToList();

            return allProducts;
        }

        public void Delete(int id)
        {
            var product = Get(id);
            trojantradingDbContext.Products.Remove(product);
            trojantradingDbContext.SaveChanges();
        }

        public int GetTotalProducts()
        {
            var result = trojantradingDbContext.Products.Count();
            return result;
        }

        public ApiResponse UpdateProduct(Product product) {
            try
            {
                trojantradingDbContext.Products.Update(product);
                trojantradingDbContext.SaveChanges();
                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully update product"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Status = "fail",
                    Message = ex.Message
                };
            }
        }
    }
}