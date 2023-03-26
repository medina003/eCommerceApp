using System.Collections.Generic;

namespace Data.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public string? Description { get; set; }
        public string Author { get; set; } = null!;
        public int PageCount { get; set; }
        public float Weight { get; set; }
        public string Publisher { get; set; } = null!;
        public double Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int LanguageId { get; set; }
        public Language Language { get; set; } = null!;

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public int FormatId { get; set; }
        public Format Format { get; set; } = null!;
        public ICollection<CartItem>? CartItems { get; set; } 
        public Stock? Stock { get; set; }
        public ICollection<OrderedProduct>? OrderedProducts { get; set; }


    }
}
