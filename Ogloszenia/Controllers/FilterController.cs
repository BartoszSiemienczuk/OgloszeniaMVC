using Ogloszenia.DAL;
using Ogloszenia.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ogloszenia.Controllers
{
    public class FilterController : Controller
    {
        private AdsContext db = AdsContext.Create();
        // GET: Admin
        public ActionResult Index()
        {
            var bannedWords = db.BannedWords.ToList();
            return View(bannedWords);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Text")] BannedWord word)
        {
            try
            {
                db.BannedWords.Add(word);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View(db.BannedWords.Find(id));
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "id,Text")] BannedWord word)
        {
            try
            {
                db.Entry(word).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View(db.BannedWords.Find(id));
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                db.BannedWords.Remove(db.BannedWords.Find(id));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
