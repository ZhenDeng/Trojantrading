using Trojantrading.Models;
using System.Linq;
using System;
using System.Threading.Tasks;
using Trojantrading.Repositories.Generic;
using Trojantrading.Utilities;

namespace Trojantrading.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetCart(int userId);
        Task<ApiResponse> UpdateShoppingCart(int userId, ShoppingItem shoppingItem);
        Task<ApiResponse> AddShoppingCart(int userId);
        Task<ShoppingCart> GetShoppingCartByID(int shoppingCartId, int userId);
        Task<ShoppingCart> GetCartWithShoppingItems(int userId);
        Task<ApiResponse> deleteShoppingItem(int shoppingItemId);
        Task<ShoppingCart> GetCartInIdWithShoppingItems(int shoppingCartId);
        Task<ApiResponse> UpdateShoppingCartPaymentMethod(int userId, string selectedPayment);
    }
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly TrojantradingDbContext _trojantradingDbContext;
        private readonly IUserRepository _userRepository;
        private readonly IRepositoryV2<ShoppingCart> _shoppingCartRepository;
        private readonly IRepositoryV2<ShoppingItem> _shoppingItemRepository;

        public ShoppingCartRepository(
            TrojantradingDbContext trojantradingDbContext,
            IUserRepository userRepository,
            IRepositoryV2<ShoppingCart> shoppingCartRepository,
            IRepositoryV2<ShoppingItem> shoppingItemRepository)
        {
            _trojantradingDbContext = trojantradingDbContext;
            _userRepository = userRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingItemRepository = shoppingItemRepository;
        }

        public async Task<ApiResponse> AddShoppingCart(int userId)
        {
            try
            {
                var shoppingCart = await _shoppingCartRepository.Queryable.Where(sc => sc.UserId == userId && sc.Status == "0").GetFirstOrDefaultAsync();
                if (shoppingCart == null)
                {
                    ShoppingCart sc = new ShoppingCart()
                    {
                        TotalItems = 0,
                        OriginalPrice = 0,
                        TotalPrice = 0,
                        UserId = userId,
                        Note = "",
                        Status = "0",
                        PaymentMethod = "onaccount"
                    };

                    _shoppingCartRepository.Create(sc);
                    await _shoppingCartRepository.SaveChangesAsync();
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

        public async Task<ShoppingCart> GetCart(int userId)
        {
            var shoppingCart = await _shoppingCartRepository.Queryable
                .Where(s => s.UserId == userId && s.Status == "0")
                .GetFirstOrDefaultAsync();
            return shoppingCart;
        }

        public async Task<ShoppingCart> GetShoppingCartByID(int shoppingCartId, int userId)
        {
            var shoppingCart = _trojantradingDbContext.ShoppingCarts.Where(s => s.Id == shoppingCartId)
                               .GroupJoin(_trojantradingDbContext.ShoppingItems, sc => sc.Id, si => si.ShoppingCartId, (shoppingCartModel, shoppingItems) => new { ShoppingCart = shoppingCartModel, ShoppingItems = shoppingItems })
                               .Select(join => new ShoppingCart()
                               {
                                   Id = join.ShoppingCart.Id,
                                   TotalItems = join.ShoppingCart.TotalItems,
                                   TotalPrice = join.ShoppingCart.TotalPrice,
                                   ShoppingItems = join.ShoppingItems.ToList(),
                                   OriginalPrice = join.ShoppingCart.OriginalPrice,
                                   UserId = userId,
                                   Status = join.ShoppingCart.Status,
                                   PaymentMethod = join.ShoppingCart.PaymentMethod
                               }).FirstOrDefault();

            if (shoppingCart.TotalItems > 0)
            {
                shoppingCart.ShoppingItems = shoppingCart.ShoppingItems
                            .Join(_trojantradingDbContext.Products, si => si.ProductId, p => p.Id, (shoppingItem, product) => new { ShoppingItem = shoppingItem, Product = product })
                            .Select(join => new ShoppingItem
                            {
                                Id = join.ShoppingItem.Id,
                                Amount = join.ShoppingItem.Amount,
                                Product = join.Product,
                                ProductId = join.Product.Id,
                                Status = join.ShoppingItem.Status,
                                Packaging = join.ShoppingItem.Packaging
                            }).ToList();
            }

            return shoppingCart;
        }

        public async Task<ShoppingCart> GetCartWithShoppingItems(int userId)
        {
            var shoppingCart = _trojantradingDbContext.ShoppingCarts.Where(sc => sc.UserId == userId && sc.Status == "0")
                                .GroupJoin(_trojantradingDbContext.ShoppingItems, sc => sc.Id, si => si.ShoppingCartId, (shoppingCartModel, shoppingItems) => new { ShoppingCart = shoppingCartModel, ShoppingItems = shoppingItems })
                                .Select(join => new ShoppingCart()
                                {
                                    Id = join.ShoppingCart.Id,
                                    TotalItems = join.ShoppingCart.TotalItems,
                                    TotalPrice = join.ShoppingCart.TotalPrice,
                                    ShoppingItems = join.ShoppingItems.ToList(),
                                    OriginalPrice = join.ShoppingCart.OriginalPrice,
                                    UserId = userId,
                                    Status = join.ShoppingCart.Status,
                                    PaymentMethod = join.ShoppingCart.PaymentMethod
                                }).FirstOrDefault();


            if (shoppingCart.TotalItems > 0)
            {
                shoppingCart.ShoppingItems = shoppingCart.ShoppingItems
                            .Join(_trojantradingDbContext.Products, si => si.ProductId, p => p.Id, (shoppingItem, product) => new { ShoppingItem = shoppingItem, Product = product })
                            .Select(join => new ShoppingItem
                            {
                                Id = join.ShoppingItem.Id,
                                Amount = join.ShoppingItem.Amount,
                                Product = join.Product,
                                ProductId = join.Product.Id,
                                Status = join.ShoppingItem.Status,
                                Packaging = join.ShoppingItem.Packaging
                            }).ToList();
            }


            return shoppingCart;
        }

        public async Task<ShoppingCart> GetCartInIdWithShoppingItems(int shoppingCartId)
        {
            var shoppingCart = _trojantradingDbContext.ShoppingCarts.Where(sc => sc.Id == shoppingCartId)
                                .GroupJoin(_trojantradingDbContext.ShoppingItems, sc => sc.Id, si => si.ShoppingCartId, (shoppingCartModel, shoppingItems) => new { ShoppingCart = shoppingCartModel, ShoppingItems = shoppingItems })
                                .Select(join => new ShoppingCart()
                                {
                                    Id = join.ShoppingCart.Id,
                                    TotalItems = join.ShoppingCart.TotalItems,
                                    TotalPrice = join.ShoppingCart.TotalPrice,
                                    ShoppingItems = join.ShoppingItems.ToList(),
                                    OriginalPrice = join.ShoppingCart.OriginalPrice,
                                    UserId = join.ShoppingCart.UserId,
                                    Status = join.ShoppingCart.Status,
                                    PaymentMethod = join.ShoppingCart.PaymentMethod
                                }).FirstOrDefault();


            if (shoppingCart.TotalItems > 0)
            {
                shoppingCart.ShoppingItems = shoppingCart.ShoppingItems
                            .Join(_trojantradingDbContext.Products, si => si.ProductId, p => p.Id, (shoppingItem, product) => new { ShoppingItem = shoppingItem, Product = product })
                            .Select(join => new ShoppingItem
                            {
                                Id = join.ShoppingItem.Id,
                                Amount = join.ShoppingItem.Amount,
                                Product = join.Product,
                                ProductId = join.Product.Id,
                                Status = join.ShoppingItem.Status,
                                Packaging = join.ShoppingItem.Packaging
                            }).ToList();
            }


            return shoppingCart;
        }

        public async Task<ApiResponse> UpdateShoppingCartPaymentMethod(int userId, string selectedPayment) {
            try
            {
                var shoppingCart = await GetCart(userId);
                if (!string.IsNullOrEmpty(selectedPayment))
                {
                    shoppingCart.PaymentMethod = selectedPayment;
                }
                _shoppingCartRepository.Update(shoppingCart);
                await _shoppingCartRepository.SaveChangesAsync();
                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully Update Shopping Cart Payment Method"
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

        public async Task<ApiResponse> UpdateShoppingCart(int userId, ShoppingItem shoppingItem)
        {
            try
            {
                var shoppingCart = await GetCart(userId);

                var user = await _userRepository.GetUserByAccount(userId);

                int shoppingItemExists = _shoppingItemRepository.Queryable.Where(si => si.ShoppingCartId == shoppingCart.Id && si.ProductId == shoppingItem.Product.Id && si.Status == "0").Count();

                if (shoppingItemExists > 0)
                {
                    var shoppingItemModel = await _shoppingItemRepository.Queryable.Where(si => si.ShoppingCartId == shoppingCart.Id && si.ProductId == shoppingItem.Product.Id && si.Status == "0").GetFirstOrDefaultAsync();
                    shoppingItemModel.Amount += shoppingItem.Amount;
                    shoppingItemModel.Packaging = shoppingItem.Packaging;
                    _shoppingItemRepository.Update(shoppingItemModel);
                    await _shoppingItemRepository.SaveChangesAsync();
                }
                else
                {
                    ShoppingItem si = new ShoppingItem()
                    {
                        Amount = shoppingItem.Amount,
                        ProductId = shoppingItem.Product.Id,
                        ShoppingCartId = shoppingCart.Id,
                        Status = "0",
                        Packaging = shoppingItem.Packaging
                    };
                    _shoppingItemRepository.Create(si);
                    await _shoppingItemRepository.SaveChangesAsync();
                }
                
                shoppingCart.TotalItems += shoppingItem.Amount;
                updateShoppingCartPrice(shoppingCart, user, shoppingItem);
                _shoppingCartRepository.Update(shoppingCart);
                await _shoppingCartRepository.SaveChangesAsync();
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

        public async Task<ApiResponse> deleteShoppingItem(int shoppingItemId)
        {
            try
            {
                var shoppingItem = _shoppingItemRepository.Queryable
                                    .Where(s => s.Id == shoppingItemId)
                                    .FirstOrDefault();
                _shoppingItemRepository.Delete(shoppingItem);
                await _shoppingItemRepository.SaveChangesAsync();
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
            shoppingCart.OriginalPrice += ((shoppingItem.Amount+0.1) * shoppingItem.Product.OriginalPrice);
            if (user.Role.ToLower() == "agent")
            {
                shoppingCart.TotalPrice += ((shoppingItem.Amount + 0.1) * shoppingItem.Product.AgentPrice);
            }
            else if (user.Role.ToLower() == "wholesaler")
            {
                shoppingCart.TotalPrice += ((shoppingItem.Amount + 0.1) * shoppingItem.Product.WholesalerPrice);
            }
        }
    }
}