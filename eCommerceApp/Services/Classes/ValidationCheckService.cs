using Data.DbContexts;
using Data.Models;
using eCommerceApp.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eCommerceApp.Services.Classes
{
   public  class ValidationCheckService
    {

        public static Error? Error { get; set; } = new();
        public static (Error?, bool) IsValidForRegistrationVM(User? User,string PasswordConfirmation)
        {
            Error = new();
            bool Flag = false;
            int? counter = 0;

            Error.Empty_error = "";
            Error.Email_error = "";
            Error.Password_error = "";
            Error.PasswordConfirmation_error = "";
            Error.FirstName_error = "";
            Error.LastName_error = "";
            if ((User?.Name == null || User?.Surname == null || User?.Email == null || User?.Password == null || PasswordConfirmation == null))
            {
                Error.Empty_error = "All fields must be filled in";
                counter++;
            }

            if (User?.Email != null)
            {
                if (!Regex.IsMatch((User?.Email!), @"^[^.][a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{1,63}$")) { Error.Email_error = "Invalid email"; counter++; }

            }
            if (User?.Password != null)
            {
                if (!Regex.IsMatch((User?.Password!), @"^(?=.*?[A-Za-z])(?=.*?[0-9]).{6,}$")) { Error.Password_error = "Must include at least an alphabet and one number and more than 6 characters "; counter++; }
            }
            if (User?.Name != null)
            {
                if (!Regex.IsMatch((User?.Name!), @"^[~`!@#$%^&*()_+=[\]\\{}|;':"",.\/<>? a-zA-Z0-9-]*$")) { Error.FirstName_error = "Only english letters allowed"; counter++; }
            }
            if (User?.Surname != null)
            {
                if (!Regex.IsMatch((User?.Surname!), @"^[~`!@#$%^&*()_+=[\]\\{}|;':"",.\/<>? a-zA-Z0-9-]*$")) { Error.LastName_error = "Only english letters allowed"; counter++; }
            }

            if (PasswordConfirmation != User?.Password) { Error.PasswordConfirmation_error = "The password confirmation does not match"; counter++; }

            using (var db = new BookStoreDbContext())
            {
                //db.Users.Add(new User() { Name = "Medina", Surname = "Abasova", Email = "medina.abasova@gmail.com", Password = "Medina123", UserType="User" });
                //db.Users.Load();
                //var c = db.Users.SingleOrDefault(u => u.Email == User.Email);
                if (db.Users.Any(u => u.Email == User.Email)) { Error.Email_error = "Email has already been taken"; counter++; }
            }

            if (counter == 0) { Flag = true; }


            return (Error, Flag);
        }

        public static (Error?, bool) IsValidForLoginVM(User? user)
        {
            Error = new();
            bool Flag = false;
            int counter = 0;
            Error.Email_error = "";
            Error.Password_error = "";
            if (user?.Email == "" || user?.Email == null) { Error.Email_error = "Fill in field"; counter++; }
            if (user?.Password == "" || user?.Password == null) { Error.Password_error = "Fill in field"; counter++; }
            if (counter == 0) { Flag = true; }
            return (Error, Flag);
        }



    }
}

