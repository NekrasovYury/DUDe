using DUDe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DUDe.Controllers
{
    public class AdminController : Controller
    {
        DeviceContext db = new DeviceContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View(db.devices);
        }
    }
}