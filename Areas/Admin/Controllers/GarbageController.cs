using DCDGear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DCDGear.Areas.Admin.Controllers
{
    public class GarbageController : BaseController
    {
        // GET: Admin/Garbage
        private DCDGearDbContext db = new DCDGearDbContext();
        public ActionResult Product()
        {
            return View(db.Products.Where(d => d.Status == false).ToList());
        }
        public ActionResult New()
        {
            return View(db.Products.Where(d => d.Status == false).ToList());
        }
        public ActionResult Order()
        {
            return View(db.Orders.Where(d => d.Status == -1).ToList());
        }
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Product");
        }
        public ActionResult RestoreProduct(long ID)
        {
            Product product = db.Products.Find(ID);
            product.Status = true;
            db.SaveChanges();
            SetAlert("Khôi phục thành công!!", "success");
            return RedirectToAction("Product");
        }
        public ActionResult RestoreOrder(long ID)
        {
            Order product = db.Orders.Find(ID);
            product.Status = 0;
            db.SaveChanges();
            SetAlert("Khôi phục thành công!!", "success");
            return RedirectToAction("Order");
        }
        public ActionResult RestoreNew(long ID)
        {
            New product = db.News.Find(ID);
            product.Status = true;
            db.SaveChanges();
            SetAlert("Khôi phục thành công!!", "success");
            return RedirectToAction("New");
        }
        public ActionResult DeleteOrder(long id)
        {
            var user = db.Orders.Find(id);

            return View(user);
        }
        [HttpPost]
        public ActionResult DeleteOrder(Order entity)
        {
            Order user = db.Orders.Find(entity.ID);
            foreach (var item in user.OrderDetails.ToList())
            {
                db.OrderDetails.Remove(item);
            }
            db.Orders.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: Admin/New/Delete/5
        public ActionResult DeleteNew(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            New @new = db.News.Find(id);
            if (@new == null)
            {
                return HttpNotFound();
            }
            return View(@new);
        }

        // POST: Admin/New/Delete/5
        [HttpPost, ActionName("DeleteNew")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            New @new = db.News.Find(id);
            db.News.Remove(@new);
            db.SaveChanges();
            SetAlert("Xóa tin tức thành công", "success");
            return RedirectToAction("Index");
        }
    }
}