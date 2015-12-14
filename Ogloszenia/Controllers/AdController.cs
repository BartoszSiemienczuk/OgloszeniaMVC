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

namespace Ogloszenia.Controllers
{
    public class AdController : Controller
    {
        private AdsContext db = AdsContext.Create();
        private int CONTENT_MAXLENGTH = 100;

        // GET: Ad
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewData["categories"] = db.Categories.ToList();
            ViewData["ads"] = db.Ads.ToList();
            return View();
        }

        [Authorize(Roles ="Admin")]
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
            ViewData["categoriesSelect"] = new SelectList(categories,"CategoryID","Name");
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
                if(ad.Content.Length > CONTENT_MAXLENGTH)
                {
                    ad.ContentShort = createShortContent(ad.Content);
                } else
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
        public ActionResult Manage(long? id)
        {
            return View(db.Ads.Include(a => a.Owner));
        }

        private String createShortContent(String input)
        {
            String result = "";
            String[] words = input.Split(' ');
            int index = 0;
            while(result.Length <= CONTENT_MAXLENGTH)
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

        public ActionResult BannedWordInserted()
        {
            return View();
        }
    }
}
