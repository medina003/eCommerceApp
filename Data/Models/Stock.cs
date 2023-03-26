using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Stock
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int Quantity { get; set; }

    }
}
