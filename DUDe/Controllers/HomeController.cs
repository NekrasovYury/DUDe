using DUDe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DUDe.Controllers
{
    public class HomeController : Controller
    {
        DeviceContext db = new DeviceContext();
        public ActionResult Index()
        {
            return View(db.devices);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}