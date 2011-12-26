using System.Collections.Generic;
using System.Data.Entity;
using Meshop.Framework.DI;
using Meshop.Framework.Model;
using Meshop.Framework.Module;

namespace Meshop.Core.Models.DeleteInFuture
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseConnection2>
    {
        protected override void Seed(DatabaseConnection2 context)
        {

            var settings = new List<Setting>
                              {
                                    new Setting{Description = "Currency",      Key = "CURRENCY",      Type = "String", Value = "CZK"},
                                    new Setting{Description = "Currency sign", Key = "CURRENCY_SIGN", Type = "String", Value = "Kč"},
                                    new Setting{Description = "Shop name",     Key = "SHOP_NAME",     Type = "String", Value = "Modular E-commerce"}
                              };
            settings.ForEach(s => context.Settings.Add(s));
            context.SaveChanges();

            var parentCat = new BasicCategory
                                {
                                    CategoryID = 1,
                                    Name = "Pens"
                                };
            var parentCat1 = new BasicCategory {CategoryID = 5, Name = "Notebooks"};
            var parentCat2 = new BasicCategory {CategoryID = 6, Name = "Paper Style", Parent = parentCat1};

            var categories = new List<BasicCategory>
                                 {
                                     parentCat,
                                     new BasicCategory {CategoryID = 2, Name = "Uniball", Parent = parentCat},
                                     new BasicCategory {CategoryID = 3, Name = "Papers"},
                                     new BasicCategory {CategoryID = 4, Name = "Scissors"},
                                     parentCat1,
                                     parentCat2,
                                     new BasicCategory {CategoryID = 7, Name = "Blue Colored", Parent = parentCat2},
                                     new BasicCategory {CategoryID = 8, Name = "Cloth Style", Parent = parentCat1}
                                 };

            categories.ForEach(s => s.Children = new List<BasicCategory>());
            parentCat.Children.Add(categories.Find(c => c.CategoryID == 2));
            parentCat1.Children.Add(categories.Find(c => c.CategoryID == 6));
            parentCat1.Children.Add(categories.Find(c => c.CategoryID == 8));
            parentCat2.Children.Add(categories.Find(c => c.CategoryID == 7));

            categories.ForEach(s => s.Products = new List<BasicProduct>());
            categories.ForEach(s => context.Categories.Add(s));
            context.SaveChanges();


            var p1 = new BasicProduct {Name = "Pen Uniball SD-102", Price = 25.50m,};
            context.Products.Add(p1);

            categories.Find(s => s.CategoryID == 1).Products.Add(p1);
            categories.Find(s => s.CategoryID == 2).Products.Add(p1);

            var p2 = new BasicProduct {Name = "Marker Centropen 8722", Price = 41.70m,};
            context.Products.Add(p2);

            categories.Find(s => s.CategoryID == 1).Products.Add(p2);

            var p3 = new BasicProduct { Name = "Koh I Noor", Price = 5.50m, };
            context.Products.Add(p3);

            categories.Find(s => s.CategoryID == 2).Products.Add(p3);

            context.SaveChanges();

            var c1 = new Customer
                         {
                             Username = "fak",
                             Email = "czechdude@czechdude.com",
                             Enabled = true,
                             FirstName = "Petr",
                             LastName = "Diviš",
                             Password = "vCPucvvEBR1XvGjSlj1nPnLBbHI=",
                             Role = "Admin"
                         };

            context.Customers.Add(c1);

            var c2 = new Customer
                         {
                             Username = "tester",
                             Email = "test@test.com",
                             Enabled = true,
                             FirstName = "Petr",
                             LastName = "Tester",
                             Password = "8IG48/uc1/twsSPndhWmaaQsDDQ=",
                             Role = "Admin"
                         };

            context.Customers.Add(c2);
            

            context.SaveChanges();


            var resources = new List<Resource>
                                {
                                    new Resource
                                        {
                                            resourceType = "Global",
                                            cultureCode = "cs",
                                            resourceKey = "Hello",
                                            resourceValue = "Ahoj"
                                        },
                                    new Resource
                                        {
                                            resourceType = "Global",
                                            cultureCode = "en",
                                            resourceKey = "Hello",
                                            resourceValue = "Hello"
                                        },
                                    new Resource
                                        {
                                            resourceType = "Global",
                                            cultureCode = "cs",
                                            resourceKey = "About",
                                            resourceValue = "O programu"
                                        },
                                    new Resource
                                        {
                                            resourceType = "Global",
                                            cultureCode = "en",
                                            resourceKey = "About",
                                            resourceValue = "About"
                                        },
                                    new Resource
                                        {
                                            resourceType = "Global",
                                            cultureCode = "cs",
                                            resourceKey = "Name",
                                            resourceValue = "Jméno"
                                        },
                                    new Resource
                                        {
                                            resourceType = "Global",
                                            cultureCode = "cs",
                                            resourceKey = "Title",
                                            resourceValue = "Název"
                                        },new Resource
                                        {
                                            resourceType = "Global",
                                            cultureCode = "cs",
                                            resourceKey = "Value",
                                            resourceValue = "Hodnota"
                                        },new Resource
                                        {
                                            resourceType = "Global",
                                            cultureCode = "cs",
                                            resourceKey = "Review",
                                            resourceValue = "Recenze"
                                        },new Resource
                                        {
                                            resourceType = "Global",
                                            cultureCode = "cs",
                                            resourceKey = "Last Name",
                                            resourceValue = "Příjmení"
                                        },new Resource
                                        {
                                            resourceType = "Global",
                                            cultureCode = "cs",
                                            resourceKey = "Description",
                                            resourceValue = "Popis"
                                        }
                                };

            resources.ForEach(r => context.Resources.Add(r));
            context.SaveChanges();


            Modules.InjectSeed(context);

        }
    }

}