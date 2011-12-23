using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Meshop.Framework.Model;
using Meshop.Framework.Services;

namespace Meshop.Core.DefaultServices
{
    public class DefaultCart : ICart
    {

        private DatabaseConnection2 _db = new DatabaseConnection2();
        private HttpContextBase _context;
        private string CartID { get; set; }
        public const string CartSessionKey = "CartID";

        public ICart StartCart(HttpContextBase context)
        {
            _context = context;
            CartID = GetCartId(context);
            return this;
        }

        public IEnumerable<CartItem> GetCart()
        {
            
            var cart = _db.Carts.Where(c => c.CartID == CartID).AsEnumerable();
            return cart;
        }

        public void AddToCart(int productID)
        {
            // Get the matching cart and product instances
            var cartItem = _db.Carts.SingleOrDefault(c => c.CartID == CartID && c.ProductID == productID);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new CartItem
                               {
                                   ProductID = productID,
                                   CartID = CartID,
                                   Quantity = 1,
                                   DateCreated = DateTime.Now
                               };
                _db.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Quantity++;
            }
            // Save changes
            _db.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = _db.Carts.Single(cart => cart.CartID == CartID && cart.RecordID == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    itemCount = cartItem.Quantity;
                }
                else
                {
                    _db.Carts.Remove(cartItem);
                }
                // Save changes
                _db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = _db.Carts.Where(cart => cart.CartID == CartID);

            foreach (var cartItem in cartItems)
            {
                _db.Carts.Remove(cartItem);
            }
            // Save changes
            _db.SaveChanges();
        }

        public List<CartItem> GetCartItems()
        {
            return _db.Carts.Where(cart => cart.CartID == CartID).ToList();
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in _db.Carts
                          where cartItems.CartID == CartID
                          select (int?) cartItems.Quantity).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            // Multiply product price by count of that product to get 
            // the current price for each of those albums in the cart
            // sum all product price totals to get the cart total
            decimal? total = (from cartItems in _db.Carts
                              where cartItems.CartID == CartID
                              select (int?) cartItems.Quantity*
                                     cartItems.Product.Price).Sum();

            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order)
        {
            _db.Orders.Add(order);
            //needed for generated ID
            _db.SaveChanges();
            
            decimal orderTotal = 0;

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                                      {
                                          ProductID = item.ProductID,
                                          OrderID = order.OrderID,
                                          UnitPrice = item.Product.Price,
                                          Quantity = item.Quantity
                                      };
                // Set the order total of the shopping cart
                orderTotal += (item.Quantity*item.Product.Price);

                _db.OrderDetails.Add(orderDetail);

            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
            _db.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderID;
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session == null || context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = _db.Carts.Where(c => c.CartID == CartID);

            foreach (var item in shoppingCart)
            {
                item.CartID = userName;
            }

            _context.Session[CartSessionKey] = userName;

            _db.SaveChanges();
        }

        public bool ValidateOrder(string name, int id)
        {
            return _db.Orders.Any(o => o.OrderID == id && o.Username == name);
        }
    }
}
