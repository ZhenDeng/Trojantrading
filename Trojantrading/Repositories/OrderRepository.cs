using System.Linq;
using Trojantrading.Models;
using Trojantrading.Util;

namespace Trojantrading.Repositories
{
    
    public interface IOrderRepository
    {
        void Add(Order order);
        void Delete(int id);
        void Update(Order order);
        Order Get(int id);
        int GetToatalOrderNumber();
        int GetNewOrderNumber();
    }
    
    public class OrderRepository:IOrderRepository
    {
        private readonly TrojantradingDbContext trojantradingDbContext;
        
        public OrderRepository(TrojantradingDbContext trojantradingDbContext)
        {
            this.trojantradingDbContext = trojantradingDbContext;
        }

        public Order Get(int id)
        {
            var order = trojantradingDbContext.Orders
                .Where(o => o.Id == id)
                .FirstOrDefault();
            return order;
        }

        public int GetToatalOrderNumber()
        {
            var result = trojantradingDbContext.Orders.Count();
            return result;
        }

        public int GetNewOrderNumber()
        {
            var result = trojantradingDbContext.Orders
                .Where(o => o.OrderStatus == Constrants.ORDER_STATUS_RECEIVED).Count();
            return result;
        }

        public void Add(Order order)
        {
            trojantradingDbContext.Orders.Add(order);
            trojantradingDbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var order = Get(id);
            trojantradingDbContext.Orders.Remove(order);
            trojantradingDbContext.SaveChanges();
        }

        public void Update(Order order)
        {
            trojantradingDbContext.Orders.Update(order);
            trojantradingDbContext.SaveChanges();
        }
        
        
    }
}