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
            List<News> recentNews = db.News.Where(n => n.ExpirationDate > DateTime.Now).OrderByDescending(n => n.ExpirationDate).Take(3).ToList();
            ViewData["news"] = recentNews;
            return View();
        }
        
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}