using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
            ViewBag.ParentID = new SelectList(db.Categories.Where(d=>d.ParentID==null), "ID", "Name");
            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,MetaTitle,ParentID,DisplayOrder,SeoTitle,CreateDate,CreateBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescriptions,Status,ShowOnHome")] Category category)
        {
           
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentID = new SelectList(db.Categories.Where(d => d.ParentID == null), "ID", "Name", category.ParentID);
            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(long? id)
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
            ViewBag.ParentID = new SelectList(db.Categories.Where(d => d.ParentID == null), "ID", "Name", category.ParentID);
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,MetaTitle,ParentID,DisplayOrder,SeoTitle,CreateDate,CreateBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescriptions,Status,ShowOnHome")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentID = new SelectList(db.Categories.Where(d => d.ParentID == null), "ID", "Name", category.ParentID);
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
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
