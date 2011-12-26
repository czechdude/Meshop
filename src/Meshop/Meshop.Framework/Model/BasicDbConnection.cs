using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Mvc;
using Meshop.Framework.DI;
using Meshop.Framework.Module;
using Meshop.Framework.Translation;

namespace Meshop.Framework.Model
{
    public class DatabaseConnection2 : DbContext
    {
        public DbSet<Resource> Resources { set; get; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Customer> Customers { set; get; }
        public DbSet<BasicProduct> Products { get; set; }
        public DbSet<BasicCategory> Categories { get; set; }
        public DbSet<CartItem> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : PluginEntity
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


            //register Plugin Entity Configurations
            var allPluginsDir = new DirectoryInfo(HostingEnvironment.MapPath(Modules.Path));

            foreach (var dir in allPluginsDir.GetDirectories())
            {
                // loop through all dll files
                foreach (var dll in dir.GetFiles("*.dll"))
                {
                    var assembly = Assembly.LoadFrom(dll.FullName);
                    var pluginConfigs = assembly.GetTypes()
                        .Where(type => !String.IsNullOrEmpty(type.Namespace))
                        .Where(
                            type =>
                            type.BaseType != null && type.BaseType.IsGenericType &&
                            type.BaseType.GetGenericTypeDefinition() == typeof (EntityTypeConfiguration<>));


                    foreach (var config in pluginConfigs)
                    {
                        modelBuilder.Configurations.Add(Activator.CreateInstance(config) as dynamic);
                    }
                }
            }
        }
    }

    public class Customer
    {
       /* [Key]
        public Guid Id { get; set; }*/

         [Key]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; }

        public bool Enabled { get; set; }

        [NotMapped] 
        public string FullName { get { return FirstName + " " + LastName; } }
    }

    public class BasicProduct
    {
        [Key]
        [ScaffoldColumn(false)]
        public int ProductID { get; set; }

        [Required]
        [TranslateName("Name")]
        public string Name { get; set; }

        [Required]
        [TranslateName("Price")]
        public decimal Price { get; set; }

        [TranslateName("Categories")]
        public List<BasicCategory> Categories { get; set; }
    }

    public class BasicCategory
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public string Name { get; set; }

        public BasicCategory Parent { get; set; }
        public List<BasicCategory> Children { get; set; }
        public virtual List<BasicProduct> Products { get; set; }
    }

    public class CartItem
    {
        [Key]
        public int RecordID { get; set; }
        public string CartID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual BasicProduct Product { get; set; }
    }
    [Bind(Exclude = "OrderId")]
    public class Order
    {
        [ScaffoldColumn(false)]
        public int OrderID { get; set; }
        [ScaffoldColumn(false)]
        public string Username { get; set; }

        [TranslateRequired(ErrorMessage = "First Name is required")]      
        [TranslateName("First Name")]
        [StringLength(160)]
        public string FirstName { get; set; }

        [TranslateRequired(ErrorMessage = "Last Name is required")]
        [TranslateName("Last Name")]
        [StringLength(160)]
        public string LastName { get; set; }

        [TranslateRequired(ErrorMessage = "Address is required")]
        [StringLength(70)]
        public string Address { get; set; }

        [TranslateRequired(ErrorMessage = "City is required")]
        [StringLength(40)]
        public string City { get; set; }

        [TranslateRequired(ErrorMessage = "Postal Code is required")]
        [TranslateName("Postal Code")]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [TranslateRequired(ErrorMessage = "Country is required")]
        [StringLength(40)]
        public string Country { get; set; }

        [TranslateRequired(ErrorMessage = "Phone is required")]
        [StringLength(24)]
        public string Phone { get; set; }

        [TranslateRequired(ErrorMessage = "Email Address is required")]
        [TranslateName("Email Address")]
        [TranslateRegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
        ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        public decimal Total { get; set; }

        [ScaffoldColumn(false)]
        public DateTime OrderDate { get; set; }

        //here the future EF 4.3+ release brings enum
        [ScaffoldColumn(false)]
        public string Shipping { get; set; }

        [ScaffoldColumn(false)]
        public string Payment { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public string FullName { get { return FirstName + " " + LastName; } }
    }

   
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual BasicProduct Product { get; set; }
        public virtual Order Order { get; set; }
    }


    public class Setting
    {
        [Key]
        public string Key { get; set; }

        [Required]
        [TranslateName("Value")]
        public string Value { get; set; }

        [Required]
        public string Type { get; set; }


        public string Description { get; set; }
    }
}