using System;
using System.Collections.Generic;
using System.Linq;
using Trojantrading.Models;

namespace Trojantrading.Repositories
{

    public interface IProductRepository
    {
        ApiResponse Add(Product product);
        Product GetProductById(int id);
        List<Product> GetAllProducts();
        ApiResponse DeleteProduct(Product product);
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


        public ApiResponse Add(Product product)
        {
            try
            {
                trojantradingDbContext.Products.Add(product);
                trojantradingDbContext.SaveChanges();
                return new ApiResponse
                {
                    Status = "success",
                    Message = "Successfully add product"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Status = "fail",
                    Message = ex.Message
                };
            }
        }

        public Product GetProductById(int id)
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

            foreach (var item in allProducts)
            {
                List<PackagingList> packagingLists = trojantradingDbContext.PackagingLists.Where(pl => pl.ProductId == item.Id).ToList();
                if (packagingLists.Count > 0)
                {
                    item.PackagingLists = packagingLists;
                }
            }

            return allProducts;
        }

        public ApiResponse DeleteProduct(Product product)
        {
            try
            {
                trojantradingDbContext.Products.Remove(product);
                trojantradingDbContext.SaveChanges();
                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully delete product"
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

        public int GetTotalProducts()
        {
            var result = trojantradingDbContext.Products.Count();
            return result;
        }

        public ApiResponse UpdateProduct(Product product) {
            try
            {
                List<PackagingList> packagingLists = product.PackagingLists;
                trojantradingDbContext.Products.Update(product);
                if (packagingLists.Count < 1)
                {
                    List<PackagingList> lists = trojantradingDbContext.PackagingLists.Where(pl => pl.ProductId == product.Id).ToList();
                    if (lists.Count > 0) {
                        trojantradingDbContext.PackagingLists.RemoveRange(lists);
                    }
                }
                else {
                    List<PackagingList> lists = trojantradingDbContext.PackagingLists.Where(pl => pl.ProductId == product.Id).ToList();
                    foreach (var pack in packagingLists)
                    {
                        pack.ProductId = product.Id;
                    }
                    if (lists.Count > 0)
                    {
                        foreach (var pack in packagingLists) {
                            foreach (var list in lists)
                            {
                                
                            }
                        }
                    }
                    else {
                        trojantradingDbContext.PackagingLists.AddRange(packagingLists);
                    }
                }
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