using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trojantrading.Models
{
    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<UserRole> UserRoles {get;set;}
    }
}
