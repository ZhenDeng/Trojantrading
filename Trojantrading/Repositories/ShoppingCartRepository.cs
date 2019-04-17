using Trojantrading.Models;
using System.Linq;
using System;

namespace Trojantrading.Repositories
{
    public interface IShoppingCartRepository
    {
        ShoppingCart GetCart(int userId);
        ApiResponse UpdateShoppingCart(int userId, ShoppingItem shoppingItem);
        void Empty(int userId);
        ApiResponse AddShoppingCart(int userId);
    }
    public class ShoppingCartRepository:IShoppingCartRepository
    {
        private readonly TrojantradingDbContext trojantradingDbContext;

        public ShoppingCartRepository(TrojantradingDbContext trojantradingDbContext)
        {
            this.trojantradingDbContext= trojantradingDbContext;
        }

        public ApiResponse AddShoppingCart(int userId) {
            try
            {
                var shoppingCart = trojantradingDbContext.ShoppingCarts.Where(sc => sc.UserId == userId).FirstOrDefault();
                if (shoppingCart == null)
                {
                    ShoppingCart sc = new ShoppingCart()
                    {
                        UserId = userId
                    };

                    trojantradingDbContext.ShoppingCarts.Add(sc);
                    return new ApiResponse()
                    {
                        Status = "success",
                        Message = "Successfully add shopping cart"
                    };
                }
                else {
                    return new ApiResponse()
                    {
                        Status = "fail",
                        Message = "Shopping cart already exists"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Status = "fail",
                    Message = "fail to add shopping cart"
                };
            }
            
        }

        public ShoppingCart GetCart(int userId)
        {
            var shoppingCart = trojantradingDbContext.ShoppingCarts
                .Where(s=>s.UserId == userId)
                .FirstOrDefault();
            return shoppingCart;
        }

        public ApiResponse UpdateShoppingCart(int userId, ShoppingItem shoppingItem)
        {
            try
            {
                var shoppingCart = GetCart(userId);

                var user = trojantradingDbContext.Users.Where(u => u.Id == userId).FirstOrDefault();

                int shoppingItemExists = shoppingCart.ShoppingItems.Where(si => si.Id == shoppingItem.Id).Count();

                if (shoppingItemExists > 0) {
                    shoppingCart.ShoppingItems.Where(si => si.Id == shoppingItem.Id).FirstOrDefault().Amount += shoppingItem.Amount;
                    updateShoppingCartPrice(shoppingCart, user, shoppingItem);
                }
                else {
                    shoppingCart.ShoppingItems.Add(shoppingItem);
                    shoppingCart.TotalItems += 1;
                    updateShoppingCartPrice(shoppingCart, user, shoppingItem);
                }
                trojantradingDbContext.ShoppingCarts.Update(shoppingCart);
                trojantradingDbContext.SaveChanges();
                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully add "+ shoppingItem.Product.Name +" to shopping cart"
                };
            }
            catch (Exception)
            {
                return new ApiResponse()
                {
                    Status = "fail",
                    Message = "fail to add " + shoppingItem.Product.Name + " to shopping cart"
                };
            }
        }

        public void Empty(int userId)
        {
            trojantradingDbContext.ShoppingCarts
                    .Where(s => s.UserId == userId)
                    .FirstOrDefault().ShoppingItems.Clear();
            trojantradingDbContext.SaveChanges();
        }

        private void updateShoppingCartPrice(ShoppingCart shoppingCart, User user, ShoppingItem shoppingItem) {
            shoppingCart.OriginalPrice += (shoppingItem.Amount * shoppingItem.Product.OriginalPrice);
            if (user.Role.Name == RoleName.agent.ToString())
            {
                shoppingCart.TotalPrice += (shoppingItem.Amount * shoppingItem.Product.AgentPrice);
            }
            else if (user.Role.Name == RoleName.reseller.ToString())
            {
                shoppingCart.TotalPrice += (shoppingItem.Amount * shoppingItem.Product.ResellerPrice);
            }
        }
    }
}