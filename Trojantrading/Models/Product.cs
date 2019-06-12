using System;
using System.Collections.Generic;

namespace Trojantrading.Models
{
    public class Product
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public string ItemCode { get; set; }
        public double OriginalPrice { get; set; }
        public int? MaxQty { get; set; }
        public int? MinQty { get; set; }
        public double AgentPrice { get; set; }

        public double WholesalerPrice { get; set; }

        public double PrepaymentDiscount { get; set; }

        public string Category { get; set; }
        public List<PackagingList> PackagingLists { get; set; }

        public string Status { get; set; }  // New, Hot, Limited, Out of Stock
    }

    public class PackagingList {
        public int Id { get; set; }
        public string PackageName { get; set; } // 1.OP 2.PP
        public int ProductId { get; set; }
    }
}