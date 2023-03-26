using Data.Interfaces;
using eCommerceApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Message
{
    public class ParameterMessage
    {
        public ISendable? Message { get; set; }

    }

}
