namespace Ogloszenia.Migrations
{
    using DAL;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Ogloszenia.DAL.AdsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AdsContext context)
        {
            SeedAdmin(context);
            SeedUser(context);
            SeedCategories(context);
            SeedAds(context);
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
                    Email = "admin.bartek@ogloszenia.sln",
                    PhoneNumber = "127 001 007",
                    adsPerPage = 20
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
                    Email = "user.adam@ogloszenia.sln",
                    PhoneNumber = "192 168 111",
                    adsPerPage = 20
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
            if (!context.Categories.Any())
            {
                Category bazowa = new Category { CategoryID = 0, Name = "Kategoria bazowa" };
                var categories = new List<Category>
            {
                bazowa,
                new Category {CategoryID=1, Name="Kategoria 1", ParentCategory=bazowa },
                new Category {CategoryID=2, Name="Kategoria 2", ParentCategory=bazowa  },
                new Category {CategoryID=3, Name="Kategoria 1.1", ParentCategory=bazowa },
            };

                categories.ForEach((s) =>
                {
                    context.Categories.Add(s);
                }
                );

                context.SaveChanges();
            }
        }

        private void SeedAds(AdsContext context)
        {
            if (!context.Ads.Any())
            {
                var ads = new List<Ad>
            {
                new Ad {
                    Title ="Og³oszenie 1",
                    Content ="Lorem ipsum",
                    ContentShort ="Lorem ipsum (...)",
                    ExpirationDate =new DateTime(2015,11,20,18,00,00),
                    Owner = context.Users.First(u => u.UserName == "user.adam@ogloszenia.sln")
                    }
        };

                ads.ForEach(a => context.Ads.Add(a));
                context.SaveChanges();
            }
        }

        private void SeedBannedWords(AdsContext context)
        {
            if(!context.Ads.Any())
            {
                var words = new List<BannedWord>
                {
                    new BannedWord {Text = "Heroina" },
                    new BannedWord {Text = "Kokaina" },
                    new BannedWord {Text = "Haszysz" },
                    new BannedWord {Text = "LSD" }
                };

                words.ForEach(w => context.BannedWords.Add(w));
                context.SaveChanges();
            }
        }

    }
}
