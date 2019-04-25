using System;
using System.Collections.Generic;
using System.Linq;
using Trojantrading.Models;
using Trojantrading.Util;

namespace Trojantrading.Repositories
{
    
    public interface IOrderRepository
    {
        ApiResponse AddOrder(ShoppingCart cart);
        List<Order> GetOrderWithUser(int userId);
    }
    
    public class OrderRepository:IOrderRepository
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
                    OrderStatus = "Order Submitted",
                    UserId = cart.UserId,
                    ShoppingCartId = cart.Id,
                    ClientMessage = "",
                    AdminMessage = "",
                    Balance = 0
                };
                trojantradingDbContext.Orders.Add(order);
                foreach (var si in cart.ShoppingItems) {
                    si.Product = null;
                }
                trojantradingDbContext.ShoppingItems.RemoveRange(cart.ShoppingItems);
                trojantradingDbContext.ShoppingCarts.Remove(cart);
                ShoppingCart sc = new ShoppingCart()
                {
                    TotalItems = 0,
                    OriginalPrice = 0,
                    TotalPrice = 0,
                    UserId = order.UserId
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

        public List<Order> GetOrderWithUser(int userId) {

            var user = userRepository.GetUserWithAddress(userId);

            var joinOrders = trojantradingDbContext.Users.Where(u => u.Id == userId)
                        .GroupJoin(trojantradingDbContext.Orders, o => o.Id, u => u.UserId, (u, orders) => new { orders = orders})
                        .SelectMany(orders => orders.orders).ToList();

            foreach (var order in joinOrders) {
                order.User = user;
            }
            return joinOrders;
        }
    }
}