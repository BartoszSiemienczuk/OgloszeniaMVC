using Ogloszenia.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ogloszenia.Models;

namespace Ogloszenia.Controllers
{
    public class HomeController : Controller
    {
        private AdsContext db = AdsContext.Create();

        public ActionResult Index()
        {
            List<News> recentNews = db.News.Where(n => n.ExpirationDate > DateTime.Now).OrderBy(n => n.ExpirationDate).Take(3).ToList();
            ViewData["news"] = recentNews;
            return View();
        }
        
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult YellowSkin()
        {
            HttpCookie cookie = new HttpCookie("OgloszeniaSkin");
            cookie.Value = "yellow";
            HttpContext.Response.SetCookie(cookie);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult BlueSkin()
        {
            HttpCookie cookie = new HttpCookie("OgloszeniaSkin");
            cookie.Value = "blue";
            HttpContext.Response.SetCookie(cookie);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult RedSkin()
        {
            HttpCookie cookie = new HttpCookie("OgloszeniaSkin");
            cookie.Value = "red";
            HttpContext.Response.SetCookie(cookie);
            return RedirectToAction("Index", "Home");
        }
    }
}