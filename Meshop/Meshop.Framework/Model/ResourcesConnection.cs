using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;

namespace Meshop.Framework.Model
{
    public class ResourcesConnection : DbContext
    {

        public ResourcesConnection() : base("DatabaseConnection2")
        {
            Configuration.AutoDetectChangesEnabled = false;
        }

        public DbSet<Resource> Resources { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
        }
    }

    public class Resource
    {
        public string resourceType { get; set; }

        [Key, Column(Order = 2)]
        public string cultureCode { get; set; }

        [Key, Column(Order = 1)]
        public string resourceKey { get; set; }

        public string resourceValue { get; set; }
    }
}