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

        public List<ShoppingItem> ShoppingItems { get; set; }
    }
}