using System;
namespace Trojantrading.Models
{
    public class ShoppingItem
    {
        public int Id { get; set; }
        
        public int Amount {get;set;}

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        public int ShoppingCartId { get; set; }
    }
}