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
        ShoppingCart GetCartWithShoppingItems(int userId);
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
                        TotalItems = 0,
                        OriginalPrice = 0,
                        TotalPrice = 0,
                        UserId = userId
                    };

                    trojantradingDbContext.ShoppingCarts.Add(sc);
                    trojantradingDbContext.SaveChanges();
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

        public ShoppingCart GetCartWithShoppingItems(int userId)
        {
            var shoppingCart = trojantradingDbContext.ShoppingCarts
                                .GroupJoin(trojantradingDbContext.ShoppingItems, sc => sc.Id, si => si.ShoppingCartId, (shoppingCartModel, shoppingItems) => new {  ShoppingCart = shoppingCartModel, ShoppingItems = shoppingItems})
                                .Select(join => new ShoppingCart() {
                                    Id = join.ShoppingCart.Id,
                                    TotalItems = join.ShoppingCart.TotalItems,
                                    TotalPrice = join.ShoppingCart.TotalPrice,
                                    ShoppingItems = join.ShoppingItems.ToList(),
                                    OriginalPrice = join.ShoppingCart.OriginalPrice
                                }).FirstOrDefault();

            return shoppingCart;
        }

        public ApiResponse UpdateShoppingCart(int userId, ShoppingItem shoppingItem)
        {
            try
            {
                var shoppingCart = GetCart(userId);

                var user = trojantradingDbContext.Users.Where(u => u.Id == userId).FirstOrDefault();

                int shoppingItemExists = trojantradingDbContext.ShoppingItems.Where(si => si.ShoppingCartId == shoppingCart.Id && si.ProductId == shoppingItem.Product.Id).Count();

                if (shoppingItemExists > 0)
                {
                    var shoppingItemModel = trojantradingDbContext.ShoppingItems.Where(si => si.ShoppingCartId == shoppingCart.Id && si.ProductId == shoppingItem.Product.Id).FirstOrDefault();
                    shoppingItemModel.Amount += shoppingItem.Amount;
                    trojantradingDbContext.ShoppingItems.Update(shoppingItemModel);
                    trojantradingDbContext.SaveChanges();
                }
                else
                {
                    ShoppingItem si = new ShoppingItem()
                    {
                        Amount = shoppingItem.Amount,
                        ProductId = shoppingItem.Product.Id,
                        ShoppingCartId = shoppingCart.Id,
                    };
                    trojantradingDbContext.ShoppingItems.Add(si);
                    trojantradingDbContext.SaveChanges();
                }

                shoppingCart.TotalItems += shoppingItem.Amount;
                updateShoppingCartPrice(shoppingCart, user, shoppingItem);
                trojantradingDbContext.ShoppingCarts.Update(shoppingCart);
                trojantradingDbContext.SaveChanges();
                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully add "+ shoppingItem.Product.Name +" to shopping cart"
                };
            }
            catch (Exception ex)
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