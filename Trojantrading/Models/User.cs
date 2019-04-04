using System;
using System.Collections.Generic;
namespace Trojantrading.Models
{
    public class User
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Account {get;set;}

        public byte[] PassswordHash {get;set;}

        public byte[] PasswordSalt { get; set; }

        public string Level {get;set;}

        public string BussinessName {get;set;}

        public string Address {get;set;}

        public string PostCode {get;set;}

        public string Abn {get;set;}

        public string Trn {get;set;}

        public string Email {get;set;}

        public string Phone {get;set;}

        public string Status {get;set;}

        public bool SendEmail{get;set;}

        public ShoppingCart ShoppingCart { get; set; }

        public List<Order> Orders{get;set;}

        public List<UserRole> UserRoles {get;set;}

        public List<ShoppingItem> ShoppingItems {get;set;}
    }
}