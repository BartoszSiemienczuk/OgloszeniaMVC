using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ogloszenia.Models
{
    public class Category
    {
        public long CategoryID { get; set; }
        public virtual Category ParentCategory { get; set; }
        public String Name { get; set; }
        public virtual ICollection<Category> ChildrenCollection { get; set; }
    }
}