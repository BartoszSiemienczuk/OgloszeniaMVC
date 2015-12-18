using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ogloszenia.DAL;
using Ogloszenia.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Ogloszenia.Controllers
{
    public class AdController : Controller
    {
        private AdsContext db = AdsContext.Create();
        private int CONTENT_MAXLENGTH = 65;

        // GET: Ad
        [AllowAnonymous]
        public ActionResult Index(string search, int? pageNumber, int? categoryID)
        {
            //Wybór kategorii do wyświetlenia na pasku kategorii
            var baseCategoryId = db.Categories.First(c => c.Name == "Kategoria bazowa").CategoryID;
            ViewData["categories"] = db.Categories.Where(c => c.ParentCategory.CategoryID == baseCategoryId).OrderBy(c => c.Name).ToList();

            //Ustalenie docelowej ilości ogłoszeń na stronie
            int adsPerPage;
            if (User.Identity.GetUserId() != null)
            {
                adsPerPage = db.Users.Find(User.Identity.GetUserId()).adsPerPage;
            }
            else
            {
                adsPerPage = 15;
            }

            var ads = db.Ads.ToList().ToPagedList(pageNumber ?? 1, adsPerPage);

            //Znalezienie wszystkich ogłoszeń zawierających wpisany tekst 
            if (search != "" && search != null)
            {
                ads = findAllAds(search).ToPagedList(pageNumber ?? 1, adsPerPage);
            }

            //Wyświetlenie ogłoszeń wyłącznie z wybranej kategorii
            if(categoryID != null)
            {
                ads = ads.Where(a => a.Category.Contains(db.Categories.Find(categoryID))).ToList().ToPagedList(pageNumber ?? 1, adsPerPage);
            }
            
            ViewData["ads"] = ads;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult IndexAdmin()
        {
            ViewData["categories"] = db.Categories.ToList();
            ViewData["ads"] = db.Ads.ToList();
            return View();
        }

        // GET: Ad/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ads.Find(id);
            if (ad == null)
            {
                return HttpNotFound();
            }
            ad.Visits++;
            db.Entry(ad).State = EntityState.Modified;
            db.SaveChanges();
            return View(ad);
        }

        // GET: Ad/Create
        public ActionResult Create()
        {
            var categories = db.Categories.ToList();
            ViewData["categories"] = categories;
            ViewData["categoriesSelect"] = new SelectList(categories, "CategoryID", "Name");
            return View();
        }

        //GET: Ad/Search
        public ActionResult Search()
        {
            var categories = db.Categories.ToList();
            ViewData["categoriesSelect"] = new SelectList(categories, "CategoryID", "Name");
            return View();
        }

        // POST: Ad/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdID,Title,Content,ExpirationDate")] Ad ad)
        {
            if (ModelState.IsValid)
            {
                if (ad.Content.Length > CONTENT_MAXLENGTH)
                {
                    ad.ContentShort = createShortContent(ad.Content);
                }
                else
                {
                    ad.ContentShort = ad.Content;
                }
                ad.Owner = db.Users.Find(User.Identity.GetUserId());

                if (containsAnyBannedWord(ad.Content) || containsAnyBannedWord(ad.Title))
                {
                    return RedirectToAction("BannedWordInserted");
                }
                db.Ads.Add(ad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ad);
        }

        // GET: Ad/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ads.Find(id);
            if (ad == null)
            {
                return HttpNotFound();
            }
            return View(ad);
        }

        // POST: Ad/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdID,Title,Content,ExpirationDate")] Ad ad)
        {
            if (ModelState.IsValid)
            {
                if (ad.Content.Length > CONTENT_MAXLENGTH)
                {
                    ad.ContentShort = createShortContent(ad.Content);
                }
                else
                {
                    ad.ContentShort = ad.Content;
                }
                if (containsAnyBannedWord(ad.Content) || containsAnyBannedWord(ad.Title))
                {
                    return RedirectToAction("BannedWordInserted");
                }
                db.Entry(ad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ad);
        }

        // GET: Ad/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ads.Find(id);
            if (ad == null)
            {
                return HttpNotFound();
            }
            return View(ad);
        }

        // POST: Ad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Ad ad = db.Ads.Find(id);
            db.Ads.Remove(ad);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Ad/Manage/5
        public ActionResult Manage(long? id, int? pageNumber)
        {
            var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                var adsPerPage = db.Users.Find(userId).adsPerPage;
                var ads = db.Ads.Where(a => a.Owner.Id == userId).ToList().ToPagedList(pageNumber ?? 1, adsPerPage);
                return View(ads);
            }

            return RedirectToAction("Index", "Home", null);
        }

        private String createShortContent(String input)
        {
            String result = "";
            String[] words = input.Split(' ');
            int index = 0;
            while (result.Length <= CONTENT_MAXLENGTH)
            {
                result += words[index] + " ";
                index++;
            }
            result += " (...)";
            return result;
        }

        private Boolean containsAnyBannedWord(String content)
        {
            Boolean result = false;
            db.BannedWords.ToList().ForEach(w => result = content.Contains(w.Text));
            return result;
        }

        private List<Ad> findAllAds(string search)
        {
            List<Ad> result = new List<Ad>();
            string[] strings = search.Split(' ');

            foreach (string s in strings)
            {
                List<Ad> ads = db.Ads.Where(a => a.Content.Contains(s) || a.Title.Contains(s)).ToList();
                foreach (Ad a in ads)
                {
                    if (!result.Contains(a)) result.Add(a);
                }
            }

            return result;
        }
        public ActionResult BannedWordInserted()
        {
            return View();
        }
    }
}
