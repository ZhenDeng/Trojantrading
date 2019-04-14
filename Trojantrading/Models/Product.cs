using System;

namespace Trojantrading.Models
{
    public class Product
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Name { get; set; }

        public double OriginalPrice {get;set;}

        public double VipOnePrice {get;set;}

        public double VipTwoPrice {get;set;}

        public int Quantity { get; set; }

        public string Category { get; set; }

        public ShoppingItem ShoppingItem{get;set;}

        public int ShoppingItemId {get;set;}
    }
   
}