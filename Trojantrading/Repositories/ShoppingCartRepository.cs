using Trojantrading.Models;
using System.Linq;

namespace Trojantrading.Repositories
{
    public interface IShoppingCartRepository
    {
        ShoppingCart GetCart(int id);
        void UpdateShoppingItems(ShoppingCart shoppingCart);
        void Empty(int id);
    }
    public class ShoppingCartRepository:IShoppingCartRepository
    {
        private readonly TrojantradingDbContext trojantradingDbContext;

        public ShoppingCartRepository(TrojantradingDbContext trojantradingDbContext)
        {
            this.trojantradingDbContext= trojantradingDbContext;
        }

        public ShoppingCart GetCart(int id)
        {
            var shoppingCart = trojantradingDbContext.ShoppingCarts
                .Where(s=>s.Id == id)
                .FirstOrDefault();
            return shoppingCart;
        }

        public void UpdateShoppingItems(ShoppingCart shoppingCart)
        {
            trojantradingDbContext.ShoppingCarts.Update(shoppingCart);
            trojantradingDbContext.SaveChanges();
        }

        public void Empty(int id)
        {
            trojantradingDbContext.ShoppingCarts
                    .Where(s => s.Id == id)
                    .FirstOrDefault().ShoppingItems.Clear();
            trojantradingDbContext.SaveChanges();
        }
    }
}