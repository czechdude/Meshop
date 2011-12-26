using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Meshop.Framework.Model;

namespace Meshop.Core.Models
{
    public class HomeViewModel
    {
        public HomeViewModel(List<BasicProduct> products)
        {
            this.Products = products;
        }

        public List<BasicProduct> Products { get; private set; }

    }
}