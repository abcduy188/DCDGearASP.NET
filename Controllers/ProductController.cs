using DCDGear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCDGear.Controllers
{
    public class ProductController : Controller
    {
        private DCDGearDbContext db = new DCDGearDbContext();
        // GET: Product
        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = db.ProductCategories.ToList();
            return PartialView(model);

        }
        public ActionResult Index()
        {
            return View();
        }
    }
}