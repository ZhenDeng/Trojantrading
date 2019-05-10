using System;
using System.Collections.Generic;
using System.Linq;
using Trojantrading.Models;

namespace Trojantrading.Repositories
{

    public interface IOrderRepository
    {
        ApiResponse AddOrder(ShoppingCart cart);
        Order Get(int id);
        ApiResponse DeleteOrder(int id);
        ApiResponse UpdateOder(Order order);
        Order GetOrdersWithShoppingItems(int orderId);
        List<Order> GetOrdersByUserID(int userId, string dateFrom, string dateTo);
        List<Order> GetOrdersByDate(string dateFrom, string dateTo);
        List<Order> GetOrderWithUser(int userId);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly TrojantradingDbContext trojantradingDbContext;
        private readonly IUserRepository userRepository;
        private readonly IShoppingCartRepository shoppingCartRepository;

        public OrderRepository(
            TrojantradingDbContext trojantradingDbContext,
            IUserRepository userRepository,
            IShoppingCartRepository shoppingCartRepository)
        {
            this.trojantradingDbContext = trojantradingDbContext;
            this.userRepository = userRepository;
            this.shoppingCartRepository = shoppingCartRepository;
        }

        public ApiResponse AddOrder(ShoppingCart cart)
        {
            try
            {
                Order order = new Order()
                {
                    CreatedDate = DateTime.Now,
                    TotalItems = cart.TotalItems,
                    TotalPrice = cart.TotalPrice,
                    OrderStatus = "Unprocessed",
                    UserId = cart.UserId,
                    ShoppingCartId = cart.Id,
                    InvoiceNo = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    ClientMessage = "",
                    AdminMessage = "",
                    Balance = 0
                };
                trojantradingDbContext.Orders.Add(order);
                foreach (var si in cart.ShoppingItems)
                {
                    si.Status = "1";
                    si.Product = null;
                }

                trojantradingDbContext.ShoppingItems.UpdateRange(cart.ShoppingItems);
                cart.Status = "1";
                trojantradingDbContext.ShoppingCarts.Update(cart);
                ShoppingCart sc = new ShoppingCart()
                {
                    TotalItems = 0,
                    OriginalPrice = 0,
                    TotalPrice = 0,
                    UserId = order.UserId,
                    Status = "0"
                };

                trojantradingDbContext.ShoppingCarts.Add(sc);
                trojantradingDbContext.SaveChanges();
                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully create order"
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

        public Order Get(int id)
        {
            var order = trojantradingDbContext.Orders
                .Where(o => o.Id == id)
                .FirstOrDefault();
            return order;
        }

        public ApiResponse DeleteOrder(int id)
        {
            try
            {
                var order = trojantradingDbContext.Orders.Where(u => u.Id == id).FirstOrDefault();
                trojantradingDbContext.Orders.Remove(order);
                trojantradingDbContext.SaveChanges();
                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully delete order"
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

        public ApiResponse UpdateOder(Order order)
        {
            try
            {
                trojantradingDbContext.Orders.Update(order);
                trojantradingDbContext.SaveChanges();
                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully update order"
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

        public List<Order> GetOrdersByUserID(int userId, string dateFrom, string dateTo)
        {

            DateTime fromDate = string.IsNullOrWhiteSpace(dateFrom) ? DateTime.Now.AddMonths(-1) : DateTime.Parse(dateFrom);
            DateTime toDate = string.IsNullOrWhiteSpace(dateTo) ? DateTime.Now.AddDays(1) : DateTime.Parse(dateTo).AddDays(1); // usage end date always next day midnight

            var orders = trojantradingDbContext.Orders
                .Where(x => x.UserId == userId && DateTime.Compare(x.CreatedDate, fromDate)>=0 && DateTime.Compare(x.CreatedDate, toDate) <= 0).ToList();

            foreach (var order in orders)
            {
                var userInfo = userRepository.GetUserByAccount(order.UserId);
                order.User = userInfo;
            }

            return orders;

        }

        public List<Order> GetOrdersByDate(string dateFrom, string dateTo)
        {
            List<Order> orders = new List<Order>();

            DateTime fromDate = string.IsNullOrWhiteSpace(dateFrom) ? DateTime.Now.AddMonths(-1).Date : DateTime.Parse(dateFrom).Date;
            DateTime toDate = string.IsNullOrWhiteSpace(dateTo) ? DateTime.Now.AddDays(1).Date : DateTime.Parse(dateTo).AddDays(1).Date; // usage end date always next day midnight

            orders = trojantradingDbContext.Orders
                .Where(x => DateTime.Compare(x.CreatedDate, fromDate) >= 0 && DateTime.Compare(x.CreatedDate, toDate) <= 0).ToList();

            foreach (var order in orders)
            {
                var userInfo = userRepository.GetUserByAccount(order.UserId);
                order.User = userInfo;
            }
            return orders;

        }

        public List<Order> GetOrderWithUser(int userId)
        {

            var user = userRepository.GetUserByAccount(userId);

            var joinOrders = trojantradingDbContext.Users.Where(u => u.Id == userId)
                        .GroupJoin(trojantradingDbContext.Orders, o => o.Id, u => u.UserId, (u, orders) => new { Orders = orders })
                        .SelectMany(selectOrders => selectOrders.Orders).ToList();

            foreach (var order in joinOrders)
            {
                order.User = user;
            }
            return joinOrders;
        }

        public Order GetOrdersWithShoppingItems(int orderId)
        {
            var order = trojantradingDbContext.Orders.Where(x => x.Id == orderId).FirstOrDefault();

            var shoppingCart = shoppingCartRepository.GetShoppingCartByID(order.ShoppingCartId, order.UserId);

            order.ShoppingCart = shoppingCart;

            return order;
            
        }
    }
}