using System;
namespace Trojantrading.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public int TotalItems { get; set; }

        public double TotalPrice { get; set; }

        public double Balance { get; set; }

        public string OrderStatus { get; set; }

        public string ClientMessage { get; set; }

        public string AdminMessage { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        public int ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}