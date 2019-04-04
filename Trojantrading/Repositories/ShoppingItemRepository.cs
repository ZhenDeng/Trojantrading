using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trojantrading.Repositories
{

    public interface IShoppingItemRepository
    {
    }

    public class ShoppingItemRepository:IShoppingItemRepository
    {
        private readonly TrojantradingDbContext trojantradingDbContext;

        public ShoppingItemRepository(TrojantradingDbContext trojantradingDbContext)
        {
            this.trojantradingDbContext = trojantradingDbContext;
        }
    }
}
