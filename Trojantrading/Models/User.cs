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
        public string Password { get; set; }
        public string BussinessName { get; set; }
        public string Trn { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public bool SendEmail { get; set; }
        public string BillingCustomerName { get; set; }
        public string BillingAddressLine1 { get; set; }
        public string BillingAddressLine2 { get; set; }
        public string BillingAddressLine3 { get; set; }
        public string BillingSuburb { get; set; }
        public string BillingState { get; set; }
        public string BillingPostCode { get; set; }
        public string ShippingCustomerName { get; set; }
        public string ShippingAddressLine1 { get; set; }
        public string ShippingAddressLine2 { get; set; }
        public string ShippingAddressLine3 { get; set; }
        public string ShippingSuburb { get; set; }
        public string ShippingState { get; set; }
        public string ShippingPostCode { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public string Fax { get; set; }
        public string Abn { get; set; }
        public string Acn { get; set; }
        public string Role { get; set; }
        public List<ShoppingCart> ShoppingCarts { get; set; }
        public List<Order> Orders { get; set; }
        public List<ShoppingItem> ShoppingItems { get; set; }
    }

    public class AppSettings
    {
        public string Secret { get; set; }
    }

    public enum RoleName {
        admin,
        agent,
        reseller
    }
}