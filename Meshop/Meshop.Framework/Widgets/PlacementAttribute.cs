using System;

namespace Meshop.Framework.Widgets
{
    public enum PagePosition
    {
        Left, 
        Right, 
        Bottom, 
        Top, 
        Header, 
        Footer,
        Document
    }

    public enum PageTemplate
    {
        Index,
        Product,
        Catalog,
        Cart,
        Login,
        Address,
        Shipping,
        Payment,
        Search,
        All
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PlacementAttribute : Attribute
    {
        public virtual PagePosition Position { get; set; }
        public PageTemplate Template { get; set; }
        public int Order { get; set; }
    }

}
