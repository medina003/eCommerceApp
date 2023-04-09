using Data.DbContexts;
using Data.Models;
using eCommerceApp.Message;
using eCommerceApp.Model;
using eCommerceApp.Services.Classes;
using eCommerceApp.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Windows;

namespace eCommerceApp.ViewModel
{
    public class UserPanelViewModel : ViewModelBase
    {
        public ObservableCollection<Data.Models.Book>? Books { get; set; }
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

                        //db.Books.Add(new Book { Title = "The Road", Author = "by Cormac McCarthy", ISBN = "0307387895", Description = "At once brutal and tender, despairing and rashly hopeful, spare of language and profoundly moving, this work is a fierce and haunting meditation on the tenuous divide between civilization and savagery, and the essential, sometimes terrifying power of filial love.", PageCount = 287, Weight = 0.83F, Publisher = "Knopf Doubleday Publishing Group", Price = 8.66, ImageUrl = "https://images.bwbcovers.com/030/The-Road-9780307387899.jpg", LanguageId = 1, CategoryId = 1, FormatId = 1 });
                        //db.SaveChanges();
                        if (TemporaryCart.TempCartItems!.Any(a => a.BookId == SelectedItem.Id))
                        {
                            int index = (TemporaryCart.TempCartItems!.FindIndex(a => a.BookId == SelectedItem.Id));
                            TemporaryCart.TempCartItems[index].Quantity += 1;
                        }
                        else
                        {
                            TemporaryCart.TempCartItems!.Add(new CartItem() { BookId = SelectedItem.Id, Book = SelectedItem, Quantity = 1 });
                        }


                    }
                    else
                    {
                        using (var db = new BookStoreDbContext())
                        {

                            //Language language = new() { Name="English" };
                            //Category category = new Category(){ Name="Fiction"};
                            //Format format = new Format() { Name= "Paperback" };
                            //db.Books.Add(new Book { Title = "The Road", Author = "by Cormac McCarthy", ISBN = "0307387895", Description = "At once brutal and tender, despairing and rashly hopeful, spare of language and profoundly moving, this work is a fierce and haunting meditation on the tenuous divide between civilization and savagery, and the essential, sometimes terrifying power of filial love.", PageCount = 287, Weight = 0.83F, Publisher = "Knopf Doubleday Publishing Group", Price = 8.66, ImageUrl = "https://images.bwbcovers.com/030/The-Road-9780307387899.jpg", LanguageId = 1, CategoryId = 1, FormatId = 1 });
                            //db.SaveChanges();
                            //var item = await db.Books.ToListAsync();
                            //ObservableCollection<Book> b = new ObservableCollection<Book> ((IEnumerable<Book>)db.Books.ToListAsync());
                            //var item = db.Books.ToListAsync().Wait(); 
                            //var item =  db.Books.Include(a=>a.La);

                            //return  item;
                            if (db.CartItems.Any(b => b.BookId == SelectedItem.Id && b.ShoppingCartId == CurrentUser.ShoppingCart!.Id))
                            {
                                db.CartItems.FirstOrDefault(b => b.BookId == SelectedItem.Id && b.ShoppingCartId == CurrentUser.ShoppingCart!.Id)!.Quantity += 1;
                                

                            }
                            else
                            {
                                db.CartItems.Add(new() { BookId = SelectedItem.Id, ShoppingCartId = CurrentUser.ShoppingCart!.Id, Quantity = 1 });
                               
                            }
                            db.SaveChanges();
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



            });

        }
        public RelayCommand AccountCommand
        {
            get => new(() =>
            {
                if (CurrentUser == null)
                {

                    _navigationService?.NavigateTo<LoginViewModel>();

                }
                else
                {

                }


            });
        }
        public RelayCommand LogOutCommand
        {
            get => new(() =>
            {
                if (CurrentUser != null)
                {
                    CurrentUser = null;

                    _navigationService?.NavigateTo<LoginViewModel>();

                }
                else MessageBox.Show("You haven't logged in yet");



            });
        }


    }
}
