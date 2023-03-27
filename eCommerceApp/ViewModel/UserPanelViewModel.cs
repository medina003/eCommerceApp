using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Data.DbContexts;
using System.Collections.ObjectModel;
using eCommerceApp.Services.Classes;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;
using System.Windows;
using eCommerceApp.Message;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using GalaSoft.MvvmLight.Messaging;
using eCommerceApp.Services.Interfaces;
using eCommerceApp.Model;

namespace eCommerceApp.ViewModel
{
    public class UserPanelViewModel : ViewModelBase
    {
        public ObservableCollection<Data.Models.Book>? Books { get; set; }
        //public static  ShoppingCart? TempShoppingCart { get; set; } = new();
        //public static CartItem? TempCartItem { get; set; }
        public User? CurrentUser { get; set; }
        public Book? SelectedItem { get; set; }
        private INavigationService? _navigationService;
        private readonly IMessenger? _messenger;
        public UserPanelViewModel(INavigationService? navigationService, IMessenger? messenger = null)
        {
            _navigationService = navigationService;
            
            _messenger = messenger;
            _messenger!.Register<ParameterMessage>(this, param =>
            {
                CurrentUser = param?.Message as User;

            });
            //using (var db = new BookStoreDbContext())
            //{

            //    //Language language = new() { Name="English" };
            //    //Category category = new Category(){ Name="Fiction"};
            //    //Format format = new Format() { Name= "Paperback" };
            //    //db.Books.Add(new Book { Title = "The Help", Author = "Kathryn Stockett", ISBN = "0425232204", Description = "In Jackson, Mississippi, in 1962, there are lines that are not crossed. With the civil rights movement exploding all around them, three women start a movement of their own, forever changing a town and the way women--black and white, mothers and daughters--view one another.", PageCount = 534, Weight = 0.83F, Publisher = "Penguin Publishing Group", Price = 8.74, ImageUrl = "https://images.bwbcovers.com/042/The-Help-9780425232200.jpg", Language = language, Category = category, Format = format });
            //    //db.SaveChanges();

            //}

            Books = new(GetBooksService.GetBooks());

        }
            public RelayCommand AddToCartCommand
        {
            get => new(() =>
            {
                if (SelectedItem != null)
                {

                    if (CurrentUser == null)
                    {
                        if (TemporaryCart.TempCartItems!.Any(a => a.BookId == SelectedItem.Id))
                        {
                            int index = (TemporaryCart.TempCartItems!.FindIndex(a => a.BookId == SelectedItem.Id));
                            TemporaryCart.TempCartItems[index].Quantity += 1;
                        }
                        else
                        {
                            TemporaryCart.TempCartItems!.Add(new CartItem() { BookId = SelectedItem.Id ,Book=SelectedItem,Quantity=1});
                        }


                    }
                    else
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
                            if (db.CartItems.Any(b => b.BookId == SelectedItem.Id && b.ShoppingCartId == CurrentUser.ShoppingCart!.Id) )
                            {
                                db.CartItems.FirstOrDefault(b => b.BookId == SelectedItem.Id && b.ShoppingCartId == CurrentUser.ShoppingCart!.Id)!.Quantity += 1;
                            }
                            else { db.CartItems.Add(new() { BookId = SelectedItem.Id, ShoppingCartId = CurrentUser.ShoppingCart!.Id, Quantity = 1 });
                                db.SaveChanges();
                                 }

                         }
   


                        }
                    }

                

            });

        }
        public RelayCommand CartCommand
        {
            get => new(() =>
            {
                _navigationService?.NavigateTo<ShoppingCartViewModel>(new ParameterMessage() { Message = CurrentUser });

                //if (CurrentUser!=null)
                //{

                //    _navigationService?.NavigateTo<ShoppingCartViewModel>(new ParameterMessage() { Message = CurrentUser });
                //}
                //else
                //{
                //    _navigationService?.NavigateTo<ShoppingCartViewModel>();

                //}


            });

        }
        public RelayCommand AccountCommand
        {
            get => new(() =>
            {
                if (CurrentUser == null)
                    _navigationService?.NavigateTo<LoginViewModel>();



            });
        }
    }
}
