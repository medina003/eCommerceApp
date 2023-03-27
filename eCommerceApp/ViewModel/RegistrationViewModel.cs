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
            //first check if any empty

            get => new(param =>
            {
                //check
                //Empty_error = "";
                //Email_error = "";
                //Fin_error = "";
                //Serial_error = "";
                //Phone_error = "";
                //if ((FirstName == null || LastName == null || Email == null || Password == null || PasswordConfirmation == null || Address == null || Phone == null || Serial == null || FIN == null))
                //{
                //    Empty_error = "All fields must be filled in";
                //}
                // if (Users.All_users.Exists(u => ((u.Email)?.ToLower()) == Email?.ToLower())) { Email_error = "Email has already been taken"; }
                // if (Users.All_users.Exists(u => ((u.FIN)?.ToLower() == FIN?.ToLower()))) { Fin_error = "FIN has already been taken"; }
                // if (Users.All_users.Exists(u =>(u.Serial).ToLower() == Serial?.ToLower())) { Serial_error = "Serial number has already been taken"; }
                // if (Users.All_users.Exists(u => (u.Phone).ToLower() == Phone?.ToLower())) { Phone_error = "Phone number has already been taken"; }

                var errors = ValidationCheckService.IsValidForRegistrationVM(User,PasswordConfirmation);

                Error = errors.Item1;

                //User? user_info = new();
                //user_info.FirstName = FirstName;
                //user_info.LastName = LastName;  
                //user_info.Email= Email;
                //user_info.Password = Password;
                //user_info.Address = Address;
                //user_info.Phone = Phone;
                //user_info.Serial = Serial;
                //user_info.FIN = FIN;
                if (errors.Item2)
                {
                    //var user_index = WriteToFileService.WriteUserInfo(User);
                    using (var db = new BookStoreDbContext())
                    {
                        User.UserType = "User";
                        db.Users.Add(User);
                        db.ShoppingCarts.Add(new() { UserId = User.Id, User = User });
                        db.SaveChanges();
                        //db.Users.Load();
                        //var c = db.Users.SingleOrDefault(u => u.Email == User.Email);
                        //if (db.Users.Any(u => u.Email == User.Email)) { Error.Email_error = "Email has already been taken"; counter++; }
                    }
                    _navigationService?.NavigateTo<UserPanelViewModel>(new ParameterMessage() { Message = User });
                }

                //Users.All_users.Add(user_info);

                // MessageBox.Show("Enter all data");
                //Empty_error = "Required field";
                //if (FirstName == null || LastName == null || Email == null || Password == null || PasswordConfirmation == null || Address == null || Phone == null || Seria == null || FIN == null)
                //{ MessageBox.Show("Enter all data"); Empty_error = "Required field"; }
                ////else if (check func == true) proxodimsa po jsonu gde useri if user[i].fin ==  fin {vivodim message box cto takoy est uje } else if email else if  seria
                //else {                //App.Container.GetInstance<MainViewModel>().CurrentViewModel = App.Container.GetInstance<RegistrationViewModel>();
                //    //_navigationService?.NavigateTo<RegistrationViewModel>();
                //    }
                //Users? users = new();
                //User? user_info = new();
                //user_info.FirstName = FirstName;
                //user_info.Email = Email;
                //user_info.Password = Password;
                //Users.All_users.Add(user_info);
                //int? i = (int)(Users.All_users.Count) - 1;

                //using var fs = new FileStream("data.json", FileMode.OpenOrCreate);
                //JsonSerializer.Serialize(fs, Users.All_users);
                //_navigationService?.NavigateTo<LoginViewModel>(new ParameterMessage() { Message = user_info });


                //var json = JsonSerializer.Serialize(user_info);
                //bool check = File.Exists("myfile.json");
                //if (!check)
                //{
                //    var i = File.Create("myfile.json");
                //    i.Close();
                //}
                //FileStream f = new FileStream("myfile.json", FileMode.Truncate);
                //StreamWriter s = new StreamWriter(f);
                //s.WriteLine(json);
                //s.Close();
                //f.Close();
            });
        }
    }
}
