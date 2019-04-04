using System.Linq;
using Trojantrading.Models;

namespace Trojantrading.Repositories
{

    public interface IProductRepository
    {
        Product Add(Product product);
        Product Get(int id);
        void Delete(int id);
        int GetTotalProducts();
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
    }
}