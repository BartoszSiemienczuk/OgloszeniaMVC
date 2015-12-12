namespace Ogloszenia.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Ogloszenia.Models;
    using Ogloszenia.DAL;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Ogloszenia.DAL.AdsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AdsContext context)
        {
            SeedAdmin(context);
            SeedUser(context);
            SeedAds(context);
            SeedCategories(context);
        }

        private void SeedAdmin(IdentityDbContext<ApplicationUser> context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any(t => t.UserName == "admin.bartek@ogloszenia.sln"))
            {
                var user = new ApplicationUser
                {
                    UserName = "admin.bartek@ogloszenia.sln",
                    Email = "admin.bartek@ogloszenia.sln"
                };
                userManager.Create(user, "Admin123!");

                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole
                {
                    Name = "Admin"
                });
                context.SaveChanges();

                userManager.AddToRole(user.Id, "Admin");

            }
        }

        private void SeedUser(IdentityDbContext<ApplicationUser> context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any(t => t.UserName == "user.adam@ogloszenia.sln"))
            {
                var user = new ApplicationUser
                {
                    UserName = "user.adam@ogloszenia.sln",
                    Email = "user.adam@ogloszenia.sln"
                };
                userManager.Create(user, "Passw0rd!");

                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole
                {
                    Name = "User"
                });
                context.SaveChanges();

                userManager.AddToRole(user.Id, "User");

            }
        }

        private void SeedCategories(AdsContext context)
        {
            Category bazowa = new Category { CategoryID = 0, Name = "Kategoria bazowa" };
            var categories = new List<Category>
            {
                bazowa,
                new Category {CategoryID=1, Name="Kategoria 1", ParentCategory=bazowa },
                new Category {CategoryID=2, Name="Kategoria 2", ParentCategory=bazowa  },
                new Category {CategoryID=3, Name="Kategoria 1.1", ParentCategory=bazowa },
            };

            categories.ForEach((s) => {
                context.Categories.Add(s);
            }
            );

            context.SaveChanges();
        }

        private void SeedAds(AdsContext context)
        {
            var ads = new List<Ad>
            {
                new Ad {Title="Og³oszenie 1", Content="Lorem ipsum", ExpirationDate=new DateTime(2015,11,20,18,00,00) }
            };

            ads.ForEach(a => context.Ads.Add(a));
            context.SaveChanges();
        }
    }
}
