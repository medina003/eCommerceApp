using eCommerceApp.Message;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Services.Interfaces
{
    public interface INavigationService
    {

            public void NavigateTo<T>() where T : ViewModelBase;

            public void NavigateTo<T>(ParameterMessage? message = null) where T : ViewModelBase;
            //public void NavigateTo<T>(UsersMessage? message = null) where T : ViewModelBase;

        
    }
}
