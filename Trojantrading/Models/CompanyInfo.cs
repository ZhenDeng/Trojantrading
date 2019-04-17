using System;
using System.Collections.Generic;

namespace Trojantrading.Models
{
    public class CompanyInfo
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string EmailAddress {get;set;}
        
        public string Email { get; set; }

        public string EmailPassWord {get; set;}

        public string Phone { get; set; }

        public string BankAccount { get; set; }

        public string BankBsb { get; set; }

        public string BankName { get; set; }

        public string Fax {get;set;}

        public string Abn {get;set;}

        public string Acn {get;set;}

        public List<User> Users { get; set; }
    }
}