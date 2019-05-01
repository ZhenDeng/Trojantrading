using System;
using System.Collections.Generic;
using System.Linq;
using Trojantrading.Models;

namespace Trojantrading.Repositories
{

    public interface IOrderRepository
    {
        ApiResponse AddOrder(ShoppingCart cart);
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
                    OrderStatus = "Order Submitted",
                    UserId = cart.UserId,
                    ShoppingCartId = cart.Id,
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
    }
}