using DCDGear.Common;
using DCDGear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCDGear.Controllers
{
    public class HomeController : Controller
    {
        private DCDGearDbContext db = new DCDGearDbContext();
        public ActionResult Index()
        {
            var banner = db.Banners.FirstOrDefault(d=>d.Type==1);
            ViewBag.ListProductsNew= db.Products.Where(d=>d.Status==true).OrderByDescending(d => d.CreateDate).Take(8).ToList();
            ViewBag.ListNewsNew = db.News.Where(d => d.Status == true).OrderByDescending(d => d.CreateDate).Take(8).ToList();
            ViewBag.ListNewsNew1 = db.News.OrderByDescending(d => d.CreateDate).Where(d=>d.Status==true).Take(1).ToList();
            ViewBag.ListRepresentative = db.Products.Where(d => d.Code=="01" && d.Status==true).OrderByDescending(d => d.CreateDate).Take(4).ToList();
            return View(banner);
        }
      
        public ActionResult Info()
        {
            var sess = (UserLogin)Session["DUY"];
            User user = db.Users.Find(sess.UserID);
            if (sess != null)
            {
                return View(user);
            }
            else
            {
                return Redirect("/dang-nhap");
            }
            
        }
    }
}