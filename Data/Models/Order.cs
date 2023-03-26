using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public double TotalPrice { get; set; }
        public DateTime dateTime { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public bool IsDelivered { get; set; }
        public int PaymentDetailsId { get; set; }
        public PaymentDetails PaymentDetails { get; set; } = null!;
        public ICollection<OrderedProduct> OrderedProducts { get; set; } = null!;
        
    }
}
