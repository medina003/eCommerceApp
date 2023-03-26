using Data.DbContexts;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using static System.Reflection.Metadata.BlobBuilder;

namespace eCommerceApp.Services.Classes
{
   public static  class GetBooksService
    {
        public static  List<Book> GetBooks()
        {
            using (var db = new BookStoreDbContext())
            {

                //Language language = new() { Name="English" };
                //Category category = new Category(){ Name="Fiction"};
                //Format format = new Format() { Name= "Paperback" };
                //db.Books.Add(new Book { Title = "The Help", Author = "Kathryn Stockett", ISBN = "0425232204", Description = "In Jackson, Mississippi, in 1962, there are lines that are not crossed. With the civil rights movement exploding all around them, three women start a movement of their own, forever changing a town and the way women--black and white, mothers and daughters--view one another.", PageCount = 534, Weight = 0.83F, Publisher = "Penguin Publishing Group", Price = 8.74, ImageUrl = "https://images.bwbcovers.com/042/The-Help-9780425232200.jpg", Language = language, Category = category, Format = format });
                //db.SaveChanges();
                //var item = await db.Books.ToListAsync();
                //ObservableCollection<Book> b = new ObservableCollection<Book> ((IEnumerable<Book>)db.Books.ToListAsync());
                //var item = db.Books.ToListAsync().Wait(); 
                //var item =  db.Books.Include(a=>a.La);

                //return  item;
                //db.CartItems.Where(s => s.ShoppingCartId == 1).Include(c => c.Book);
                return db.Books.ToList();   

               
            }
        }
    }
}
