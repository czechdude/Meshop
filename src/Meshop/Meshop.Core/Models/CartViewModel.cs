using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Meshop.Framework.Model;
using Meshop.Framework.Services;

namespace Meshop.Core.Models
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal CartTotal { get; set; }

        public ICheckout CheckoutService { get; set; }
    }

    public class CartRemoveViewModel
    {
        public string Message { get; set; }
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public int DeleteId { get; set; }
    }
}