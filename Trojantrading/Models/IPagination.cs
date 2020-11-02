using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trojantrading.Models
{
    public interface IPagination
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}
