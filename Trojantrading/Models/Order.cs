using System;
using System.Collections.Generic;
namespace Trojantrading.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public int TotalItems {get;set;}

        public double TotalPrice {get;set;}

        public double Balance {get;set;}

        public string OrderStatus {get;set;}

        public string ClientMessage {get;set;}

        public string AdminMessage {get;set;}

        public User User {get;set;}

        public int UserId{get;set;}

        public List<ShoppingItem> ShoppingItems{get;set;}
    }
}