using Data.DbContexts;
using Data.Models;
using eCommerceApp.Message;
using eCommerceApp.Model;
using eCommerceApp.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace eCommerceApp.ViewModel
{
    public class ShoppingCartViewModel : ViewModelBase
    {
        public ObservableCollection<CartItem>? Items { get; set; }
        public double Total { get; set; }
        public User? User { get; set; }
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
                        Items = new(db.CartItems.Where(s => s.ShoppingCartId == 1).Include(c => c.Book).ToList());
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


        
    }
}
