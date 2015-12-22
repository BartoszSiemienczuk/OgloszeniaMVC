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
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AdsContext context)
        {
            SeedAdmin(context);
            SeedUser(context);
            SeedCategories(context);
            SeedAds(context);
            SeedBannedWords(context);
        }

        private void SeedAdmin(IdentityDbContext<ApplicationUser> context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any(t => t.UserName == "admin@ogloszenia.sln"))
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@ogloszenia.sln",
                    Email = "admin@ogloszenia.sln",
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

            if (!context.Users.Any(t => t.UserName == "user1@ogloszenia.sln"))
            {
                var user1 = new ApplicationUser
                {
                    UserName = "user1@ogloszenia.sln",
                    Email = "user1@ogloszenia.sln",
                    PhoneNumber = "192 168 111",
                    adsPerPage = 20
                };
                userManager.Create(user1, "Passw0rd!");

                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole
                {
                    Name = "User"
                });
                context.SaveChanges();

                userManager.AddToRole(user1.Id, "User");

                var user2 = new ApplicationUser
                {
                    UserName = "user2@ogloszenia.sln",
                    Email = "user2@ogloszenia.sln",
                    PhoneNumber = "192 168 111",
                    adsPerPage = 20
                };
                userManager.Create(user2, "Passw0rd2!");

                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole
                {
                    Name = "User"
                });
                context.SaveChanges();

                userManager.AddToRole(user2.Id, "User");

            }
        }

        private void SeedCategories(AdsContext context)
        {
            if (!context.Categories.Any())
            {
                Category Bazowa = new Category { Name = "Kategoria bazowa" };
                Category Nieruchomości = new Category { Name = "Nieruchomości", ParentCategory = Bazowa };
                Category Motoryzacja = new Category { Name = "Motoryzacja", ParentCategory = Bazowa };
                Category Praca = new Category { Name = "Praca", ParentCategory = Bazowa };
                Category Sprzedam = new Category { Name = "Sprzedam", ParentCategory = Bazowa };
                Category Zamienię = new Category { Name = "Zamienię", ParentCategory = Bazowa };
                var categories = new List<Category>
            {
                Bazowa,
                Nieruchomości,
                Motoryzacja,
                Praca,
                Sprzedam,
                Zamienię
            };

                categories.ForEach((s) =>
                {
                    context.Categories.Add(s);
                }
                );

                context.SaveChanges();

                Category SamochodyOsobowe = new Category { Name = "Samochody osobowe", ParentCategory = context.Categories.First(c => c.Name == "Motoryzacja") };

                var subcategories = new List<Category>
            {
                new Category { Name="Mieszkania", ParentCategory=Nieruchomości  },
                new Category { Name="Domy jednorodzinne", ParentCategory=Nieruchomości  },
                SamochodyOsobowe,
                new Category { Name="Zabytkowe samochody osobowe", ParentCategory=SamochodyOsobowe  },
                new Category { Name="Samochody ciężarowe", ParentCategory=Motoryzacja  },
                new Category { Name="Praca sezonowa", ParentCategory=Praca  },
                new Category { Name="Praca stała", ParentCategory=Praca  },
                new Category { Name="Praca dorywcza", ParentCategory=Praca  },
            };

                subcategories.ForEach((s) =>
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
                    //REAL ESTATE
                new Ad {
                    Title ="Lorem ipsum ",
                    Category = new List<Category>{
                        context.Categories.First(c => c.Name== "Nieruchomości"),
                        context.Categories.First(c => c.Name== "Zamienię")
                    },
                    Content = "Curabitur vitae dui eleifend, lobortis lectus in, feugiat elit. Suspendisse potenti. Ut vel ultricies mauris. Donec pulvinar consectetur massa sed tristique. Integer velit libero, rutrum in purus sit amet, imperdiet vehicula purus. Phasellus ipsum magna, gravida sed metus a, ornare tristique felis.",
                    ContentShort = "Curabitur vitae dui eleifend, lobortis lectus in (...)",
                    ExpirationDate = new DateTime(2015, 11, 20, 18, 00, 00),
                    Owner = context.Users.First(u => u.UserName == "user1@ogloszenia.sln")
                    },
                new Ad {
                    Title ="Maecenas sed lacus ut tortor",
                    Category =  new List<Category>{
                        context.Categories.First(c => c.Name== "Mieszkania"),
                        context.Categories.First(c => c.Name== "Sprzedam"),
                        context.Categories.First(c => c.Name== "Nieruchomości")
                    },
                    Content = "Sed justo orci, viverra non arcu varius, auctor sagittis felis. Phasellus vitae facilisis urna. Nam sed posuere dui. Etiam tincidunt, enim id sodales ornare, tellus ex lobortis dolor, in ultrices enim lorem et lorem. Nunc dui nisl, finibus non malesuada vel, convallis eget ligula",
                    ContentShort = "Sed justo orci, viverra non arcu varius, auctor sagittis felis. (...)",
                    ExpirationDate = new DateTime(2015, 11, 20, 18, 00, 00),
                    Owner = context.Users.First(u => u.UserName == "user1@ogloszenia.sln")
                    },
                new Ad {
                    Title ="Curabitur",
                    Category =  new List<Category>{
                        context.Categories.First(c => c.Name== "Mieszkania"),
                        context.Categories.First(c => c.Name== "Sprzedam"),
                        context.Categories.First(c => c.Name== "Nieruchomości")
                    },
                    Content = "Aenean non convallis dolor. Mauris rhoncus ligula sed quam elementum volutpat. Nunc blandit eu quam a fringilla. Curabitur porttitor orci nisi, vitae viverra enim rutrum nec",
                    ContentShort = "Aenean non convallis dolor. Mauris rhoncus ligula sed quam (...)",
                    ExpirationDate = new DateTime(2015, 11, 20, 18, 00, 00),
                    Owner = context.Users.First(u => u.UserName == "user1@ogloszenia.sln")
                    },
                new Ad {
                    Title ="Aliquam varius porttitor quam",
                    Category =  new List<Category>{
                        context.Categories.First(c => c.Name== "Domy jednorodzinne"),
                        context.Categories.First(c => c.Name== "Sprzedam"),
                        context.Categories.First(c => c.Name== "Nieruchomości")
                    },
                    Content = "Aliquam varius porttitor quam, ut varius enim mollis nec. Sed hendrerit nisi et ante imperdiet, ac eleifend elit convallis. In in metus quis ante efficitur tincidunt et vitae metus.",
                    ContentShort = "Aliquam varius porttitor quam, ut varius enim mollis nec. (...)",
                    ExpirationDate = new DateTime(2015, 11, 20, 18, 00, 00),
                    Owner = context.Users.First(u => u.UserName == "user1@ogloszenia.sln")
                    },
                new Ad {
                    Title ="Suspendisse placerat enim ",
                    Category =  new List<Category>{
                        context.Categories.First(c => c.Name== "Domy jednorodzinne"),
                        context.Categories.First(c => c.Name== "Zamienię"),
                        context.Categories.First(c => c.Name== "Nieruchomości")
                    },
                    Content = "Suspendisse placerat enim ut ligula gravida laoreet. Donec ornare auctor neque, et maximus enim ornare quis. In hac habitasse platea dictumst. Nulla pulvinar, erat sit amet luctus pharetra, sem turpis facilisis diam, nec lacinia lectus leo at mauris. Integer sollicitudin leo et nisl posuere accumsan. ",
                    ContentShort = "Suspendisse placerat enim ut ligula gravida laoreet.(...)",
                    ExpirationDate = new DateTime(2015, 11, 20, 18, 00, 00),
                    Owner = context.Users.First(u => u.UserName == "user1@ogloszenia.sln")
                    },
                
                //MOTORIZATION
                new Ad {
                    Title ="Proin convallis metus eros",
                    Category =  new List<Category>{
                        context.Categories.First(c => c.Name== "Samochody osobowe"),
                        context.Categories.First(c => c.Name== "Motoryzacja"),
                        context.Categories.First(c => c.Name== "Sprzedam")
                    },
                    Content = "Proin convallis metus eros, quis luctus dolor gravida a. Nulla ac orci vel magna vestibulum dapibus ut vel eros. Phasellus sagittis augue odio, et blandit dolor auctor quis",
                    ContentShort = "Proin convallis metus eros, quis luctus dolor gravida a. (...)",
                    ExpirationDate = new DateTime(2015, 11, 20, 18, 00, 00),
                    Owner = context.Users.First(u => u.UserName == "user2@ogloszenia.sln")
                    },
                new Ad {
                    Title ="Vivamus",
                    Category =  new List<Category>{
                        context.Categories.First(c => c.Name== "Zabytkowe samochody osobowe"),
                        context.Categories.First(c => c.Name== "Zamienię"),
                        context.Categories.First(c => c.Name== "Motoryzacja"),
                    },
                    Content = "Vivamus vel magna eu mi hendrerit elementum. Nulla consequat turpis rhoncus tortor ullamcorper, ac tincidunt est aliquam. Phasellus risus erat, accumsan ut dui eget, mollis porta sapien.",
                    ContentShort = "Vivamus vel magna eu mi hendrerit elementum..(...)",
                    ExpirationDate = new DateTime(2015, 11, 20, 18, 00, 00),
                    Owner = context.Users.First(u => u.UserName == "user2@ogloszenia.sln")
                    },
                new Ad {
                    Title ="Suspendisse placerat enim ",
                    Category =  new List<Category>{
                        context.Categories.First(c => c.Name== "Samochody ciężarowe"),
                        context.Categories.First(c => c.Name== "Sprzedam")
                    },
                    Content = "Suspendisse placerat enim ut ligula gravida laoreet. Donec ornare auctor neque, et maximus enim ornare quis. In hac habitasse platea dictumst. Nulla pulvinar, erat sit amet luctus pharetra, sem turpis facilisis diam, nec lacinia lectus leo at mauris. Integer sollicitudin leo et nisl posuere accumsan. ",
                    ContentShort = "Suspendisse placerat enim ut ligula gravida laoreet.(...)",
                    ExpirationDate = new DateTime(2015, 11, 20, 18, 00, 00),
                    Owner = context.Users.First(u => u.UserName == "user2@ogloszenia.sln")
                    },
                //JOB
                new Ad {
                    Title ="Cras non turpis",
                    Category =  new List<Category>{
                        context.Categories.First(c => c.Name== "Praca sezonowa"),
                        context.Categories.First(c => c.Name== "Praca")
                    },
                    Content = "Cras non turpis ligula. Maecenas volutpat dapibus lacus, eu ornare leo maximus sed. Curabitur cursus vel nisl nec dictum. Integer sed pretium nulla. ",
                    ContentShort = "Cras non turpis ligula. Maecenas volutpat dapibus (...)",
                    ExpirationDate = new DateTime(2015, 11, 20, 18, 00, 00),
                    Owner = context.Users.First(u => u.UserName == "user2@ogloszenia.sln")
                    },
                new Ad {
                    Title ="Phasellus id ligula sit",
                    Category =  new List<Category>{
                        context.Categories.First(c => c.Name== "Praca stała"),
                        context.Categories.First(c => c.Name== "Praca")
                    },
                    Content = "Phasellus id ligula sit amet magna consectetur placerat. Ut a libero non urna condimentum cursus. Morbi sed nisl dictum, tempor felis eu, condimentum dui. ",
                    ContentShort = "Phasellus id ligula sit amet magna consectetur placerat. (...)",
                    ExpirationDate = new DateTime(2015, 11, 20, 18, 00, 00),
                    Owner = context.Users.First(u => u.UserName == "user2@ogloszenia.sln")
                    },
        };

                ads.ForEach(a => context.Ads.Add(a));
                context.SaveChanges();
            }
        }

        private void SeedBannedWords(AdsContext context)
        {
            if (!context.BannedWords.Any())
            {
                var words = new List<BannedWord>
                {
                    new BannedWord {Text = "Heroina" },
                    new BannedWord {Text = "Kokaina" },
                    new BannedWord {Text = "Khat" },
                    new BannedWord {Text = "Marihuana" },
                    new BannedWord {Text = "LSD" },
                    new BannedWord {Text = "Metaamfetamina" },
                    new BannedWord {Text = "Złodziej" },
                    new BannedWord {Text = "Oszust" },
                    new BannedWord {Text = "Kłamca"},
                    new BannedWord {Text = "Idiota" }
                };

                words.ForEach(w => context.BannedWords.Add(w));
                context.SaveChanges();
            }
        }

    }
}
