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
        public ActionResult Category(long id)
        {
            List<New> news = db.News.Where(d => d.CategoryID == id).ToList();
            return View(news);
        }
        public ActionResult DetailNew(long id)
        {
            New news = db.News.Find(id);
            return View(news);
        }
    }
}