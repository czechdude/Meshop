using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Meshop.Framework.Model;
using Meshop.Framework.Module;
using Meshop.Framework.Translation;

namespace Meshop.Reviews.Models
{
    public class Review : PluginEntity
    {
        
        public int ReviewID { get; set; }
        [TranslateName("Description")]
        public string Text { get; set; }
        public int ProductID { get; set; }
        public virtual BasicProduct Product { get; set; }
        public string CustomerUserName { get; set; }
        public virtual Customer Customer { get; set; }

    }

    public class ReviewConfiguration : EntityTypeConfiguration<Review>
    {
        public ReviewConfiguration()
        {
            //Map the primary key
            HasKey(x => x.ReviewID);
            //Map the additional properties
            Property(m => m.Text);
            
            Property(m => m.CustomerUserName);
            
            HasOptional(m => m.Customer)
                .WithMany()
                .HasForeignKey(p => p.CustomerUserName);

            Property(x => x.ProductID).IsRequired();
            
            HasRequired(m => m.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductID);
        }
    }

    class ReviewsSeed : IDbSeed
    {
        public void Plant(DbContext context)
        {
            var c1 = context.Set<Customer>().First();
            var p1 = context.Set<BasicProduct>().First();

            var r1 = new Review
            {
                Customer = c1,
                CustomerUserName = c1.Username,
                Product = p1,
                ProductID = p1.ProductID,
                ReviewID = 1,
                Text = "Very good product\n I would buy again."
            };
            context.Set<Review>().Add(r1);

            context.SaveChanges();
        }
    }
}
