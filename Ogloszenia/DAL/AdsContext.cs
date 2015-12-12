using Microsoft.AspNet.Identity.EntityFramework;
using Ogloszenia.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Ogloszenia.DAL
{
    public class AdsContext : IdentityDbContext<ApplicationUser>
    {
        public AdsContext() : base("Ad") {}

        public DbSet<Ad> Ads { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}