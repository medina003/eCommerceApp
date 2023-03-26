using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Model
{
    public static class TemporaryCart
    {
        public static List<CartItem>? TempCartItems { get; set; } = new();
    }
}
