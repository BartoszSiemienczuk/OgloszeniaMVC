using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ogloszenia.Models
{
    public class News
    {
        public long NewsID { get; set; }
        public String Title { get; set; }

        public String Content { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}