using System;
namespace Trojantrading.Models
{
    public class ShoppingItem
    {
        public int Id { get; set; }
        
        public int Amount {get;set;}

        public int ProductId { get; set; }
        public string Packaging { get; set; }
        public Product Product { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        public int ShoppingCartId { get; set; }

        public string Status { get; set; } // checkout -- 1, not check out -- 0
    }
}