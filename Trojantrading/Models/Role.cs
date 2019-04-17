using System.Collections.Generic;

namespace Trojantrading.Models
{
    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<User> Users { get; set; }
    }

    public enum RoleName {
        admin,
        agent,
        reseller
    }
}
