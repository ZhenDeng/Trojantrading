using Trojantrading.Models;
using System.Linq;
using System;

namespace Trojantrading.Repositories
{
    public interface IShoppingCartRepository
    {
        ShoppingCart GetCart(int userId);
        ApiResponse UpdateShoppingCart(int userId, ShoppingItem shoppingItem);
        ApiResponse AddShoppingCart(int userId);
        ShoppingCart GetCartWithShoppingItems(int userId);
        ApiResponse deleteShoppingItem(int shoppingItemId);
    }
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly TrojantradingDbContext trojantradingDbContext;
        private readonly IUserRepository userRepository;

        public ShoppingCartRepository(
            TrojantradingDbContext trojantradingDbContext,
            IUserRepository userRepository)
        {
            this.trojantradingDbContext = trojantradingDbContext;
            this.userRepository = userRepository;
        }

        public ApiResponse AddShoppingCart(int userId)
        {
            try
            {
                var shoppingCart = trojantradingDbContext.ShoppingCarts.Where(sc => sc.UserId == userId && sc.Status == "0").FirstOrDefault();
                if (shoppingCart == null)
                {
                    ShoppingCart sc = new ShoppingCart()
                    {
                        TotalItems = 0,
                        OriginalPrice = 0,
                        TotalPrice = 0,
                        UserId = userId,
                        Status = "0"
                    };

                    trojantradingDbContext.ShoppingCarts.Add(sc);
                    trojantradingDbContext.SaveChanges();
                    return new ApiResponse()
                    {
                        Status = "success",
                        Message = "Successfully add shopping cart"
                    };
                }
                else
                {
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
                    Message = ex.Message
                };
            }

        }

        public ShoppingCart GetCart(int userId)
        {
            var shoppingCart = trojantradingDbContext.ShoppingCarts
                .Where(s => s.UserId == userId && s.Status == "0")
                .FirstOrDefault();
            return shoppingCart;
        }

        public ShoppingCart GetCartWithShoppingItems(int userId)
        {
            var shoppingCart = trojantradingDbContext.ShoppingCarts.Where(sc => sc.UserId == userId && sc.Status == "0")
                                .GroupJoin(trojantradingDbContext.ShoppingItems, sc => sc.Id, si => si.ShoppingCartId, (shoppingCartModel, shoppingItems) => new { ShoppingCart = shoppingCartModel, ShoppingItems = shoppingItems })
                                .Select(join => new ShoppingCart()
                                {
                                    Id = join.ShoppingCart.Id,
                                    TotalItems = join.ShoppingCart.TotalItems,
                                    TotalPrice = join.ShoppingCart.TotalPrice,
                                    ShoppingItems = join.ShoppingItems.ToList(),
                                    OriginalPrice = join.ShoppingCart.OriginalPrice,
                                    UserId = userId,
                                    Status = "0"
                                }).FirstOrDefault();


            if (shoppingCart.TotalItems > 0)
            {
                shoppingCart.ShoppingItems = shoppingCart.ShoppingItems
                            .Join(trojantradingDbContext.Products, si => si.ProductId, p => p.Id, (shoppingItem, product) => new { ShoppingItem = shoppingItem, Product = product })
                            .Select(join => new ShoppingItem
                            {
                                Id = join.ShoppingItem.Id,
                                Amount = join.ShoppingItem.Amount,
                                Product = join.Product,
                                ProductId = join.Product.Id,
                                Status = "0"
                            }).ToList();
            }


            return shoppingCart;
        }

        public ApiResponse UpdateShoppingCart(int userId, ShoppingItem shoppingItem)
        {
            try
            {
                var shoppingCart = GetCart(userId);

                var user = userRepository.GetUserWithRole(userId);

                int shoppingItemExists = trojantradingDbContext.ShoppingItems.Where(si => si.ShoppingCartId == shoppingCart.Id && si.ProductId == shoppingItem.Product.Id && si.Status == "0").Count();

                if (shoppingItemExists > 0)
                {
                    var shoppingItemModel = trojantradingDbContext.ShoppingItems.Where(si => si.ShoppingCartId == shoppingCart.Id && si.ProductId == shoppingItem.Product.Id && si.Status == "0").FirstOrDefault();
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
                        Status = "0"
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
                    Message = "Successfully add " + shoppingItem.Product.Name + " to shopping cart"
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

        public ApiResponse deleteShoppingItem(int shoppingItemId)
        {
            try
            {
                var shoppingItem = trojantradingDbContext.ShoppingItems
                                    .Where(s => s.Id == shoppingItemId)
                                    .FirstOrDefault();
                trojantradingDbContext.ShoppingItems.Remove(shoppingItem);
                trojantradingDbContext.SaveChanges();
                return new ApiResponse()
                {
                    Status = "success"
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

        private void updateShoppingCartPrice(ShoppingCart shoppingCart, User user, ShoppingItem shoppingItem)
        {
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