using Data.DbContexts;
using Data.Models;
using eCommerceApp.Message;
using eCommerceApp.Model;
using eCommerceApp.Services.Classes;
using eCommerceApp.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.ViewModel
{
    public class PaymentViewModel:ViewModelBase
    {
        public Error? Error {  get; set; }
        public User? User { get; set; }
        public PaymentDetails PaymentDetails { get; set; } = new();
        private readonly IMessenger? _messenger;
        private INavigationService? _navigationService;
        public PaymentViewModel(INavigationService? navigationService, IMessenger? messenger = null)
        {
            _navigationService = navigationService;
            _messenger = messenger;
            _messenger?.Register<ParameterMessage>(this, param =>
            {
                User = (param?.Message) as User;
            });

            
        }
        public RelayCommand PayCommand
        {
            get => new(() =>
            {

                var errors = ValidationCheckService.IsValidForPayment(PaymentDetails);

                Error = errors.Item1;

                if(errors.Item2) { PaymentService.Payment(PaymentDetails, User);}    });

        }
    }
}
