using System.Collections;
using System.Collections.Generic;

namespace Data.Models
{
    public class Format
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}