using System;

namespace Trojantrading.Models
{
    public class Product
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Name { get; set; }

        public double OriginalPrice { get; set; }

        public double AgentPrice { get; set; }

        public double ResellerPrice { get; set; }

        public string Category { get; set; }

        public ShoppingItem[] ShoppingItems{get;set;}

        public string Status { get; set; }  // 1.New 2.Promotion 3.SoldOut
    }
}