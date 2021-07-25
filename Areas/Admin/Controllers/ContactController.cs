using DCDGear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCDGear.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        private DCDGearDbContext db = new DCDGearDbContext();
        // GET: Admin/Contact
        public ActionResult Index()
        {
            return View(db.FeedBacks.ToList());
        }
        public ActionResult Detail(long id)
        {
            var feedback = db.FeedBacks.Find(id);
            feedback.Status = false;
            db.SaveChanges();
            return View(feedback);
        }
        public ActionResult Delete(long id)
        {
            var feedback = db.FeedBacks.Find(id);
            return View(feedback);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(FeedBack product)
        {
            FeedBack entity = db.FeedBacks.Find(product.ID);
            db.FeedBacks.Remove(entity);
            db.SaveChanges();
            SetAlert("Xóa thành công", "success");
            return RedirectToAction("Index");
        }
    }
}