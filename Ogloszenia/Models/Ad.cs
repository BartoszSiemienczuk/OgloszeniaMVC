namespace Ogloszenia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class Ad
    {
        public Ad()
        {
            Visits = 0;
        }

        public long AdID { get; set; }
        [DisplayName("Tytuł ogłoszenia")]
        [StringLength(50, ErrorMessage = "Maksymalna długość tytułu wynosi 50 znaków")]     
        public String Title { get; set; }

        [DisplayName("Treść ogłoszenia")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public String Content { get; set; }

        [DisplayName("Wyświetlenia")]
        public long Visits { get; set; }

        [DisplayName("Skrócona treść")]
        public String ContentShort { get; set; }

        [DisplayName("Ważne do")]
        [DataType(DataType.DateTime)]
        public DateTime ExpirationDate { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        [DisplayName("Kategorie")]
        public virtual ICollection<Category> Category { get; set; }
    }
}