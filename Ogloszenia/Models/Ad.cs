namespace Ogloszenia.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class Ad
    {
        public long AdID { get; set; }
        public String Title { get; set; }

        public String Content { get; set; }

        public String ContentShort { get; set; }

        public DateTime ExpirationDate { get; set; }

        public virtual ApplicationUser Owner { get; set; }
        
        public virtual ICollection<Category> Category { get; set; }
    }
}