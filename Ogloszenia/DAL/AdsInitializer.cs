using Ogloszenia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ogloszenia.DAL
{
    public class AdsInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<AdsContext>
    {

        protected override void Seed(AdsContext context)
        {
            var categories = new List<Category>
            {
                new Category {CategoryID=1, Name="Kategoria 1" },
                new Category {CategoryID=2, Name="Kategoria 2" },
                new Category {CategoryID=3, Name="Kategoria 1.1"},
            };

            categories.ForEach(s => context.Categories.Add(s));
            context.SaveChanges();

            var ads = new List<Ad>
            {
                new Ad {Title="Ogłoszenie 1", Content="Lorem ipsum", ExpirationDate=new DateTime(2015,11,20,18,00,00) }
            };

            ads.ForEach(a => context.Ads.Add(a));
            context.SaveChanges();
        }
    }
}