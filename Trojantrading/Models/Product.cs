using System;

namespace Trojantrading.Models
{
    public class Product
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Name { get; set; }

        public double OriginalPrice { get; set; }

        public double VipOnePrice { get; set; }

        public double VipTwoPrice { get; set; }

        public string Category { get; set; }

        public int ShoppingItemId { get; set;  }

        public ShoppingItem ShoppingItem{get;set;}

        public string Status { get; set; }  // 1.New 2.Promotion 3.SoldOut
    }

   
}