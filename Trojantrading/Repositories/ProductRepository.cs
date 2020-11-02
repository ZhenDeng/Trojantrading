using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trojantrading.Models;
using Trojantrading.Repositories.Generic;
using Trojantrading.Utilities;

namespace Trojantrading.Repositories
{

    public interface IProductRepository
    {
        Task<ApiResponse> Add(Product product);
        Task<Product> GetProductById(int id);
        Task<List<Product>> GetAllProducts();
        Task<ApiResponse> DeleteProduct(Product product);
        Task<int> GetTotalProducts();
        Task<ApiResponse> UpdateProduct(Product product);
        Task<ApiResponse> UpdatePackagingList(List<PackagingList> lists, int productId);
    }

    public class ProductRepository:IProductRepository
    {
        private readonly IRepositoryV2<Product> _productDataRepository;
        private readonly IRepositoryV2<PackagingList> _packagingListDataRepository;

        public ProductRepository(IRepositoryV2<Product> productDataRepository, IRepositoryV2<PackagingList> packagingListDataRepository)
        {
            _productDataRepository = productDataRepository;
            _packagingListDataRepository = packagingListDataRepository;
        }


        public async Task<ApiResponse> Add(Product product)
        {
            try
            {
                _productDataRepository.Create(product);
                await _productDataRepository.SaveChangesAsync();
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

        public async Task<Product> GetProductById(int id)
        {
            var product = await _productDataRepository.Queryable
                .Where(p => p.Id == id)
                .GetFirstOrDefaultAsync();
            return product;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            List<Product> allProducts = new List<Product>();

            allProducts = await _productDataRepository.Queryable.GetListAsync();

            foreach (var item in allProducts)
            {
                List<PackagingList> packagingLists = await _packagingListDataRepository.Queryable.Where(pl => pl.ProductId == item.Id).GetListAsync();
                if (packagingLists.Count > 0)
                {
                    item.PackagingLists = packagingLists;
                }
            }

            return allProducts;
        }

        public async Task<ApiResponse> DeleteProduct(Product product)
        {
            try
            {
                _productDataRepository.Delete(product);
                await _productDataRepository.SaveChangesAsync();
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

        public async Task<int> GetTotalProducts()
        {
            var result = await _productDataRepository.Queryable.CountAsync();
            return result;
        }

        public async Task<ApiResponse> UpdateProduct(Product product) {
            try
            {
                _productDataRepository.Update(product);
                await _productDataRepository.SaveChangesAsync();
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

        public async Task<ApiResponse> UpdatePackagingList(List<PackagingList> lists, int productId)
        {
            try
            {
                List<PackagingList> originalList = await _packagingListDataRepository.Queryable.Where(pl => pl.ProductId == productId).GetListAsync();
                _packagingListDataRepository.DeleteRange(originalList);
                await _packagingListDataRepository.SaveChangesAsync();
                _packagingListDataRepository.CreateRange(lists);
                await _packagingListDataRepository.SaveChangesAsync();

                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully update packaging"
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