using Data.DbContexts;
using Data.Models;
using eCommerceApp.Message;
using eCommerceApp.Model;
using eCommerceApp.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace eCommerceApp.ViewModel
{
    public class ShoppingCartViewModel : ViewModelBase
    {
        public ObservableCollection<CartItem>? Items { get; set; } = new();
        public double Total { get; set; }
        public User? User { get; set; }
        public CartItem SelectedItem { get; set; }
        private INavigationService? _navigationService;
        private readonly IMessenger? _messenger;

        public ShoppingCartViewModel(INavigationService? navigationService, IMessenger? messenger = null)
        {
            _navigationService = navigationService;
            _messenger = messenger;
            _messenger?.Register<ParameterMessage>(this, param =>
            {
                User = (param?.Message) as User;
                if (User != null)
                {
                    using (var db = new BookStoreDbContext())
                    {
                        var FoundCart = db.ShoppingCarts.FirstOrDefault(c => c.UserId == User.Id);
                        Items = new(db.CartItems.Where(s => s.ShoppingCartId == FoundCart!.Id).Include(c => c.Book).ToList());
                    }
                }
                else
                {


                    if (User == null)
                    {

                        Items = new(TemporaryCart.TempCartItems!);
                    }


                }
            });
            //using (var db = new BookStoreDbContext())
            //{
            //   if(User == null)
            //    {
            //        Items = new();
            //        Items = TemporaryCart.TempCartItems;
            //    }

            //}


        }
        public RelayCommand MinusCommand
        {
            get => new(() =>
            {
                if (SelectedItem != null)
                {
                    if (User == null)
                    {
                        if (TemporaryCart.TempCartItems != null)
                        {
                            int index = TemporaryCart.TempCartItems!.FindIndex(a => a.BookId == SelectedItem.BookId);
                            if (TemporaryCart.TempCartItems[index].Quantity == 1)
                            {
                                TemporaryCart.TempCartItems.Remove(TemporaryCart.TempCartItems[index]);
                            }
                            else TemporaryCart.TempCartItems[index].Quantity -= 1;
                            Items = new(TemporaryCart.TempCartItems);
                        }
                    }
                    else
                    {
                        using (var db = new BookStoreDbContext())
                        {
                            if (SelectedItem.Quantity == 1)
                            {
                                db.CartItems.Remove(SelectedItem);
                            }
                            else { SelectedItem.Quantity -= 1;  db.CartItems.Update(SelectedItem);   }
                            Items = new(db.CartItems.Where(s => s.ShoppingCartId == User.ShoppingCart!.Id).Include(c => c.Book).ToList());
                            db.SaveChanges();
                        }
                    }
                }


            });
        }
        public RelayCommand PlusCommand
        {
            get => new(() =>
            {
                if (SelectedItem != null)
                {
                    if (User == null)
                    {
                        if (TemporaryCart.TempCartItems != null)
                        {
                            int index = TemporaryCart.TempCartItems!.FindIndex(a => a.BookId == SelectedItem.BookId);

                            TemporaryCart.TempCartItems[index].Quantity += 1;
                            Items = new(TemporaryCart.TempCartItems);

                            //int index = TemporaryCart.TempCartItems!.FindIndex(a => a.BookId == SelectedItem.BookId);
                            //using (var db = new BookStoreDbContext()) {
                            //int quantity = db.Stocks.Where(s=> s.BookId == SelectedItem.BookId).FirstOrDefault()!.Quantity; 
                            //if(quantity != TemporaryCart.TempCartItems[index].Quantity)
                            //    {
                            //TemporaryCart.TempCartItems[index].Quantity += 1;
                            //Items = new(TemporaryCart.TempCartItems);
                            //}
                            //else { MessageBox.Show("Have no more items in stock"); };

                            //}

                        }
                    }
                    else
                    {
                        using (var db = new BookStoreDbContext())
                        {
                            //db.Stocks.Add(new() { BookId = SelectedItem.BookId, Quantity = 300 });
                            //db.SaveChanges();

                            var stock = db.Stocks.FirstOrDefault(s => s.BookId == SelectedItem.BookId);

                            if (SelectedItem.Quantity == stock!.Quantity) 
                            {
                                MessageBox.Show("There are no more items available in the stock :( Try later :)");
                            }
                            else
                            {
                                SelectedItem.Quantity += 1; db.CartItems.Update(SelectedItem);
                                db.SaveChanges();

                                Items = new(db.CartItems.Where(s => s.ShoppingCartId == User.ShoppingCart!.Id).Include(c => c.Book).ToList());
                            }

                        }
                        
                    }
                }


            });
        }
        public RelayCommand RemoveCommand
        {
            get => new(() =>
            {
                if (SelectedItem != null)
                {
                    if (User == null)
                    {
                        if (TemporaryCart.TempCartItems != null)
                        {
                            //int index = TemporaryCart.TempCartItems!.FindIndex(a => a.BookId == SelectedItem.BookId);

                            TemporaryCart.TempCartItems.Remove(SelectedItem);
                            Items = new(TemporaryCart.TempCartItems);

                            //int index = TemporaryCart.TempCartItems!.FindIndex(a => a.BookId == SelectedItem.BookId);
                            //using (var db = new BookStoreDbContext()) {
                            //int quantity = db.Stocks.Where(s=> s.BookId == SelectedItem.BookId).FirstOrDefault()!.Quantity; 
                            //if(quantity != TemporaryCart.TempCartItems[index].Quantity)
                            //    {
                            //TemporaryCart.TempCartItems[index].Quantity += 1;
                            //Items = new(TemporaryCart.TempCartItems);
                            //}
                            //else { MessageBox.Show("Have no more items in stock"); };

                            //}

                        }
                    }
                    else
                    {
                        using (var db = new BookStoreDbContext())
                        {

                            db.CartItems.Remove(SelectedItem); db.SaveChanges();
                            Items = new(db.CartItems.Where(s => s.ShoppingCartId == User.ShoppingCart!.Id).Include(c => c.Book).ToList());

                        }
                    }
                }


            });
        }
        public RelayCommand ClearAllCommand
        {
            get => new(() =>
            {

                if (User == null)
                {
                    if (TemporaryCart.TempCartItems != null)
                    {
                        //int index = TemporaryCart.TempCartItems!.FindIndex(a => a.BookId == SelectedItem.BookId);

                        TemporaryCart.TempCartItems.Clear();
                        Items = new(TemporaryCart.TempCartItems);

                        //int index = TemporaryCart.TempCartItems!.FindIndex(a => a.BookId == SelectedItem.BookId);
                        //using (var db = new BookStoreDbContext()) {
                        //int quantity = db.Stocks.Where(s=> s.BookId == SelectedItem.BookId).FirstOrDefault()!.Quantity; 
                        //if(quantity != TemporaryCart.TempCartItems[index].Quantity)
                        //    {
                        //TemporaryCart.TempCartItems[index].Quantity += 1;
                        //Items = new(TemporaryCart.TempCartItems);
                        //}
                        //else { MessageBox.Show("Have no more items in stock"); };

                        //}

                    }
                }
                else
                {
                    using (var db = new BookStoreDbContext())
                    {
                        var cart = db.ShoppingCarts.First(u => u.UserId == User.Id);

                        db.CartItems.RemoveRange(db.CartItems.Where(i => i.ShoppingCartId == cart.Id));
                        db.SaveChanges();
                        Items = new(db.CartItems.Where(s => s.ShoppingCartId == User.ShoppingCart!.Id).Include(c => c.Book).ToList());

                    }
                }



            });
        }
        public RelayCommand BackCommand
        {
            get => new(() =>
            {
                _navigationService?.NavigateTo<UserPanelViewModel>(new ParameterMessage() { Message = User });



            });
        }

        public RelayCommand CheckoutCommand
        {
            get => new(() =>
            {
                

                if (User == null)
                {
                    if(TemporaryCart.TempCartItems!.Count != 0)
                    {
                    _navigationService?.NavigateTo<LoginViewModel>(new ParameterMessage() { Message = User });

                    }
                    else { MessageBox.Show("You don't have items in shopping cart");  }

                }
                else
                {
                    using (var db = new BookStoreDbContext())
                    {
                        ShoppingCart? foundCart = db.ShoppingCarts.FirstOrDefault(u=>u.UserId == User.Id);

                        if (db.CartItems.Any(u => u.ShoppingCartId == foundCart!.Id))
                        {
                            if(db.CustomerDetails.Any(u => u.UserId == User!.Id))
                            {
                                foreach(var item in db.CartItems)
                                {
                                    if (item.ShoppingCartId == User!.Id)
                                    {
                                   
                                    }
                                }
                            }
                        
                        
                        
                        
                        }
                        else
                        {
                            MessageBox.Show("No items in shopping cart");
                        }

                    }
                }



            });

        }


    }
}
