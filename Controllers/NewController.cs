using DCDGear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCDGear.Controllers
{
    public class NewController : Controller
    {
        // GET: New
        private DCDGearDbContext db = new DCDGearDbContext();
        [ChildActionOnly]
        public PartialViewResult NewCategory()
        {
            var model = db.Categories.ToList();
            return PartialView(model);

        }
        public ActionResult Index()
        {
            return View();
        }
    }
}