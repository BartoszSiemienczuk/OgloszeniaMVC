using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ogloszenia.Models
{
    public class News
    {
        public long NewsID { get; set; }
        public String Title { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public String Content { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}