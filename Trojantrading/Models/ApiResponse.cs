using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trojantrading.Models
{
    public class UserResponse
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }

    public class ApiResponse {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
