using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DCDGear.Common;
using DCDGear.DAO;
using DCDGear.Models;

namespace DCDGear.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        private DCDGearDbContext db = new DCDGearDbContext();

        // GET: Admin/ProductCategory
        public ActionResult Index()
        {
            return View(db.ProductCategories.OrderBy(d=>d.Name).ToList());
        }
        public ActionResult Create()
        {
            var dao = new ProductCategoryDAO();
            ViewBag.ParentID = new SelectList(db.ProductCategories.Where(d => d.ParentID == null), "ID", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            var dao = new ProductCategoryDAO();
            ViewBag.ParentID = new SelectList(db.ProductCategories.Where(d => d.ParentID == null), "ID", "Name");
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session["DUY"];
                productCategory.CreateBy = session.Name;
                productCategory.CreateDate = DateTime.Now;
                var create = dao.Create(productCategory);
                if (create == true)
                {
                    SetAlert("Thêm danh muc thanh cong", "success");
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Khong tao dc danh muc");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dao = new ProductCategoryDAO();
            var model = dao.FindByID(id);
            ViewBag.ParentID = new SelectList(db.ProductCategories.Where(d => d.ParentID == null), "ID", "Name",model.ParentID);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory)
        {
            var dao = new ProductCategoryDAO();
            ViewBag.ParentID = new SelectList(db.ProductCategories.Where(d => d.ParentID == null), "ID", "Name", productCategory.ParentID);
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session["DUY"];
                productCategory.ModifiedBy = session.Name;
                var edit = dao.Edit(productCategory);
                if(edit)
                {
                    SetAlert("Chỉnh sửa danh mục thành công", "success");
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Khong tao dc danh muc");
                }
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
            foreach(var item in productCategory.Products.ToList())
            {
                db.Products.Remove(item);
            }
            db.ProductCategories.Remove(productCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public void SetViewBag(long? selectedId = null)
        {
            var dao = new ProductCategoryDAO();
            ViewBag.ParentID = new SelectList(db.ProductCategories.Where(d=>d.ParentID==null), "ID", "Name", selectedId);
        }
    }
}
