using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Meshop.Framework.Model;

namespace Meshop.Framework.Services
{
    public interface ICart
    {
        ICart StartCart(HttpContextBase context);
        IEnumerable<CartItem> GetCart(); 
        void AddToCart(int productID);
        int RemoveFromCart(int id);
        void EmptyCart();
        List<CartItem> GetCartItems();
        int GetCount();
        decimal GetTotal();        
        int CreateOrder(Order order);
        string GetCartId(HttpContextBase context);
        void MigrateCart(string userName);
        bool ValidateOrder(string name, int id);
    }
}
