using eCommerceApp.Message;
using eCommerceApp.Services.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Services
{
    public class NavigationService:INavigationService
    {
        private readonly IMessenger _messenger;
        public NavigationService(IMessenger messenger)
        {
            _messenger = messenger;
        }
        //public void NavigateTo<T>() where T : ViewModelBase
        //{

        //    _messenger.Send(new NavigationMessage() { ViewModelType = typeof(T) });
        //}
        public void NavigateTo<T>() where T : ViewModelBase
        {
            _messenger.Send(new NavigationMessage() { ViewModelType = typeof(T) });
        }
        public void NavigateTo<T>(ParameterMessage? message = null) where T : ViewModelBase
        {
            _messenger.Send(message);
            _messenger.Send(new NavigationMessage() { ViewModelType = typeof(T) });
        }
        //public void NavigateTo<T>(UsersMessage? message = null) where T : ViewModelBase
        //{
        //    _messenger.Send(message);
        //    _messenger.Send(new NavigationMessage() { ViewModelType = typeof(T) });
        //}
    }
}
