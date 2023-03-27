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
                                db.CartItems.Remove(SelectedItem); db.SaveChanges();
                                Items = new(db.CartItems.Where(s => s.ShoppingCartId == User.ShoppingCart.Id).Include(c => c.Book).ToList());
                            }
                            else { db.CartItems.Find(SelectedItem).Quantity -= 1; db.SaveChanges(); }
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
      
                           db.CartItems.Find(SelectedItem).Quantity += 1; db.SaveChanges(); 
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

                            db.CartItems.Find(SelectedItem).Quantity += 1; db.SaveChanges();
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

                            db.CartItems.Find(SelectedItem).Quantity += 1; db.SaveChanges();
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


    }
}
