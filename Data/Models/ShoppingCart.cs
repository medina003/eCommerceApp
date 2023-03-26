using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }  
        public ICollection<CartItem>? CartItems { get; set; }

    }
}
