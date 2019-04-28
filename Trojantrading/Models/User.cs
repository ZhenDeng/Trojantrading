using System;
using System.Collections.Generic;

namespace Trojantrading.Models
{
    public class User
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Account { get; set; }
        public string PassswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Password { get; set; }
        public string BussinessName { get; set; }
        public string PostCode { get; set; }
        public string Trn { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public bool SendEmail { get; set; }
        public List<ShoppingCart> ShoppingCarts { get; set; }
        public List<Order> Orders { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public int ShippingAddressId { get; set; }
        public int BillingAddressId { get; set; }
        public List<ShoppingItem> ShoppingItems { get; set; }
        public CompanyInfo CompanyInfo { get; set; }
        public int CompanyInfoId { get; set; }
    }

    public class AppSettings
    {
        public string Secret { get; set; }
    }
}