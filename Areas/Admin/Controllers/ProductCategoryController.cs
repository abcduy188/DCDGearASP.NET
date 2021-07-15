using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DCDGear.Common;
using DCDGear.Models;

namespace DCDGear.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        private DCDGearDbContext db = new DCDGearDbContext();

        // GET: Admin/ProductCategory
        public ActionResult Index()
        {
            return View(db.ProductCategories.OrderBy(d => d.Name).ToList());
        }
        public ActionResult Create()
        {
            ViewBag.ParentID = new SelectList(db.ProductCategories.Where(d => d.ParentID == null).ToList(), "ID", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            ViewBag.ParentID = new SelectList(db.ProductCategories.Where(d => d.ParentID == null).ToList(), "ID", "Name");
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session["DUY"];
                productCategory.CreateBy = session.Name;
                productCategory.CreateDate = DateTime.Now;
                db.ProductCategories.Add(productCategory);
                db.SaveChanges();
                SetAlert("Thêm danh muc thanh cong", "success");
                return RedirectToAction("Index", "ProductCategory");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            ProductCategory model = db.ProductCategories.Find(id);
            ViewBag.ParentID = new SelectList(db.ProductCategories.Where(d => d.ParentID == null).ToList(), "ID", "Name", model.ParentID);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(ProductCategory entity)
        {
            ProductCategory productCategory = db.ProductCategories.Find(entity.ID);
            
            ViewBag.ParentID = new SelectList(db.ProductCategories.Where(d => d.ParentID == null).ToList(), "ID", "Name", entity.ParentID);
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session["DUY"];
                productCategory.ModifiedBy = session.Name;
                productCategory.Name = entity.Name;
                productCategory.SeoTitle = entity.SeoTitle;
                productCategory.ParentID = entity.ParentID;
                productCategory.Status = entity.Status;
                productCategory.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                SetAlert("Chỉnh sửa danh mục thành công", "success");
                return RedirectToAction("Index", "ProductCategory");
            }
            return View("Index");
        }
        // GET: Admin/ProductCategory/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: Admin/ProductCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ProductCategory productCategory = db.ProductCategories.Find(id);
            foreach (var item in productCategory.Products.ToList())
            {
                db.Products.Remove(item);
            }
            db.ProductCategories.Remove(productCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
