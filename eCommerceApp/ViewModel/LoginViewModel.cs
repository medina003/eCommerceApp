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
using System.Linq;
using System.Windows;

namespace eCommerceApp.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public Error? Error { get; set; } = new();
        public User? CurrentUser { get; set; } = new();

        private INavigationService? _navigationService;
        public LoginViewModel(INavigationService? navigationService)
        {
            _navigationService = navigationService;

        }

        public RelayCommand SignInCommand
        {
            get => new(() =>
            {

                var errors = ValidationCheckService.IsValidForLoginVM(CurrentUser);

                Error = errors.Item1;
                if (errors.Item2)
                {
                    using (var db = new BookStoreDbContext())
                    {
                        var FoundUser = db.Users.SingleOrDefault(u => u.Email == CurrentUser!.Email);
                        if (FoundUser != null)
                        {
                            if (FoundUser?.Password == CurrentUser!.Password)
                            {
                                if (FoundUser.UserType == "User")
                                {
                                    if (!TemporaryCart.TempCartItems.IsNullOrEmpty())
                                    {
                                        var cart = db.ShoppingCarts.FirstOrDefault(s => s.UserId == FoundUser.Id);
                                        foreach (var item in TemporaryCart.TempCartItems!)
                                        {
                                            db.CartItems.Add(new() { Quantity = item.Quantity, BookId = item.BookId, ShoppingCartId = cart.Id });
                                        }
                                        TemporaryCart.TempCartItems.Clear();
                                    }
                                    db.SaveChanges();
                                    CurrentUser = new();
                                    _navigationService?.NavigateTo<UserPanelViewModel>(new ParameterMessage { Message = FoundUser });



                                }
                            }
                            else MessageBox.Show("Incorrect password!");
                        }
                        else MessageBox.Show("No such user");
                    }


                }




            });
        }
        public RelayCommand RegistrCommand
        {
            get => new(() =>
            {


                _navigationService?.NavigateTo<RegistrationViewModel>();




            });
        }
        public RelayCommand BackCommand
        {
            get => new(() =>
            {
                CurrentUser = new();
                Error = new();
                _navigationService?.NavigateTo<UserPanelViewModel>();




            });
        }

    }
}
