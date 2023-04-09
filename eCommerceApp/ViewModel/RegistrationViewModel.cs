using Data.DbContexts;
using Data.Models;
using eCommerceApp.Message;
using eCommerceApp.Model;
using eCommerceApp.Services.Classes;
using eCommerceApp.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.ViewModel
{
    public class RegistrationViewModel : ViewModelBase
    {
        public User User { get; set; } = new();
        public string PasswordConfirmation { get; set; } 
        private INavigationService? _navigationService;
        public Error? Error { get; set; } = new();

        public RegistrationViewModel(INavigationService? navigationService)
        {

            _navigationService = navigationService;


        }

        public RelayCommand<object> RegisterCommand
        {

            get => new(param =>
            {


                var errors = ValidationCheckService.IsValidForRegistrationVM(User,PasswordConfirmation);

                Error = errors.Item1;
                if (errors.Item2)
                {
                    using (var db = new BookStoreDbContext())
                    {
                        User.UserType = "User";
                        db.Users.Add(User);
                        db.ShoppingCarts.Add(new() { UserId = User.Id, User = User });
                        db.SaveChanges();

                        if (!TemporaryCart.TempCartItems.IsNullOrEmpty())
                        {
                            foreach(var item in TemporaryCart.TempCartItems!)
                            {
                                db.CartItems.Add(new() { Quantity = item.Quantity, BookId = item.BookId, ShoppingCartId = db.ShoppingCarts.OrderBy(u => u.Id).Last().Id});
                            }
                            TemporaryCart.TempCartItems.Clear();
                        }
                        db.SaveChanges();
              
                    }
                    _navigationService?.NavigateTo<UserPanelViewModel>(new ParameterMessage() { Message = User });

                }

            });
        }
    }
}
