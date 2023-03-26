using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class PaymentDetails
    {
        public int Id { get; set; }
        public string Number { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Month { get; set; } = null!; 
        public string Year { get; set; } = null!;
        public string CVV { get; set; } = null!; 
        public Order Order { get; set; } = null!;
    }
}
