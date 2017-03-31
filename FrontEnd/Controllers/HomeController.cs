using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Azure Intro Lab";

            Worker.BackgroundJob.Current.Schedule(GetUrl("Index"));

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Title = "Azure Intro Lab";

            ViewBag.Message = "Your application description page.";

            Worker.BackgroundJob.Current.Schedule(GetUrl("About"));

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Title = "Azure Intro Lab";

            ViewBag.Message = "Your contact page.";

            Worker.BackgroundJob.Current.Schedule(GetUrl("Contact"));

            return View();
        }

        private string GetUrl(string value)
        {
            string url = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~/api/TestApi/" + value);
            return url;
        }
    }
}