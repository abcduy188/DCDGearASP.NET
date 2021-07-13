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
    public class CategoryController : BaseController
    {
        private DCDGearDbContext db = new DCDGearDbContext();

        // GET: Admin/Category
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.ParentID = new SelectList(db.Categories.Where(d=>d.ParentID==null).ToList(), "ID", "Name");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            ViewBag.ParentID = new SelectList(db.Categories.Where(d => d.ParentID == null).ToList(), "ID", "Name");
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session["DUY"];
                category.CreateBy = session.Name;
                category.CreateDate = DateTime.Now;
                db.Categories.Add(category);
                db.SaveChanges();
                SetAlert("Thêm danh muc thanh cong", "success");
                return RedirectToAction("Index", "ProductCategory");
            }
            return View();
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(long? id)
        {
            Category category = db.Categories.Find(id);
            ViewBag.ParentID = new SelectList(db.Categories.Where(d => d.ParentID == null), "ID", "Name", category.ParentID);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
            if (category == null)
            {
                return HttpNotFound();
            }
           
            return View(category);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,MetaTitle,ParentID,DisplayOrder,SeoTitle,CreateDate,CreateBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescriptions,Status,ShowOnHome")] Category category)
        {
            ViewBag.ParentID = new SelectList(db.Categories.Where(d => d.ParentID == null), "ID", "Name", category.ParentID);
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View(category);
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Category category = db.Categories.Find(id);
            foreach(var item in category.News.ToList())
            {
                db.News.Remove(item);
            }
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
