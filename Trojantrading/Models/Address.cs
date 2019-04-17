using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trojantrading.Models
{
    public class ShippingAddress
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public List<User> Users { get; set; }
    }

    public class BillingAddress
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public List<User> Users { get; set; }
    }
}
