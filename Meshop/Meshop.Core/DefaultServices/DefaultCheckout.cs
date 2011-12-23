using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Meshop.Framework.Services;

namespace Meshop.Core.DefaultServices
{
    public class DefaultCheckout :ICheckout
    {
        public string CheckoutAction
        {
            get { return "AddressAndPayment"; }
        }

        public string CheckoutController
        {
            get { return "Checkout"; }
        }
    }
}