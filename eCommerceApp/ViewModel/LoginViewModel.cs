using Data.DbContexts;
using Data.Models;
using eCommerceApp.Message;
using eCommerceApp.Model;
using eCommerceApp.Services.Classes;
using eCommerceApp.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace eCommerceApp.ViewModel
{
    public class LoginViewModel:ViewModelBase
    {
        public Error? Error { get; set; } = new();
        public User? CurrentUser { get; set; } = new();

        private INavigationService? _navigationService;
        private readonly IMessenger _messenger;
        public LoginViewModel(INavigationService? navigationService, IMessenger messenger = null)
        {
            _navigationService = navigationService;
            _messenger = messenger;
            _messenger.Register<ParameterMessage>(this, param =>
            {
                CurrentUser = param?.Message as User;

            });
        }

        public RelayCommand SignInCommand
        {
            get => new(() =>
            {

                var errors = ValidationCheckService.IsValidForLoginVM(CurrentUser);
                
                Error =  errors.Item1;
                //Password_error = IsValid_check.IsValidForLoginVM(Email, Password).Item2;
                if (errors.Item2)
                {
                    using(var db = new BookStoreDbContext())
                    {
                        var FoundUser = db.Users.SingleOrDefault(u => u.Email == CurrentUser!.Email);
                        if (FoundUser != null)
                        {
                            if (FoundUser?.Password == CurrentUser!.Password)
                            {
                                if (FoundUser.UserType == "User")
                                {
                                _navigationService?.NavigateTo<UserPanelViewModel>(new ParameterMessage { Message = FoundUser });

                                }
                            }
                            else MessageBox.Show("Incorrect password!");
                        }
                        else MessageBox.Show("No such user");
                    }
                    
                    //if (CurrentUser.Email == "a" && CurrentUser.Password == "a")
                    //{
                    //    _navigationService?.NavigateTo<AdminViewModel>(new UsersMessage() { ListOfUsers = Users.All_users });
                    //}


                    //else if (Users.All_users.Any(u => (u?.Email)?.ToLower() == User?.Email?.ToLower()))
                    //{
                    //    int index = Users.All_users.FindIndex(u => (u?.Email)?.ToLower() == User?.Email?.ToLower());
                    //    if (Users.All_users[index]?.Password == User?.Password)
                    //    {
                    //        Index = index;
                    //        _navigationService?.NavigateTo<UserdashboardViewModel>(new ParameterMessage() { Message = index });


                    //    }
                    //    else { Error.Password_error = "Wrong password"; }

                    //}
                    //else Error.Email_error = "No such user";
                }

                ////_navigationService?.NavigateTo<UserPanelViewModel>(new ParameterMessage() { Message = User });



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
                _navigationService?.NavigateTo<UserPanelViewModel>();




            });
        }

    }
}
