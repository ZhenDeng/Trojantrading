using System;
using System.Collections.Generic;

namespace Trojantrading.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public int TotalItems {get; set;}

        public double TotalPrice {get;set;}

        public double OriginalPrice {get;set;}

        public int UserId { get; set; }

        public User User { get; set; }

        public Order Order { get; set; }

        public string Status { get; set; }// checkout -- 1, not check out -- 0

        public List<ShoppingItem> ShoppingItems { get; set; }
    }
}