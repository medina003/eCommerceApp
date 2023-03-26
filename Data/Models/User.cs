using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class User:ISendable
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserType { get; set; } = null!;
        public ICollection<Order>? Orders { get; set; }
        public CustomerDetails? CustomerDetails { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }

    }
}
