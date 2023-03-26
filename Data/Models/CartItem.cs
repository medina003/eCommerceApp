using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;
        public int Quantity { get; set; }
        public int ShoppingCartId { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }


    }
}
