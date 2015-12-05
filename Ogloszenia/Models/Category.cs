using Ogloszenia.DAL;
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

        public List<Category> getChildCategories()
        {
            AdsContext db = new AdsContext();
            List<Category> result = db.Categories.Where(c => c.ParentCategory.CategoryID==this.CategoryID).ToList();
            return result;
        }
        
        override public bool Equals(Object o)
        {
            if(o.GetType() == typeof(Category))
            {
                Category c = (Category)o;
                if (c.CategoryID == this.CategoryID)
                    return true;
            }
            return false;
        }
    }
}