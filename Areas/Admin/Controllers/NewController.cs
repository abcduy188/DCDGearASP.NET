using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DCDGear.Common;
using DCDGear.DAO;
using DCDGear.Models;

namespace DCDGear.Areas.Admin.Controllers
{
    public class NewController : BaseController
    {
        private DCDGearDbContext db = new DCDGearDbContext();

        // GET: Admin/New
        public ActionResult Index()
        {
            var dao = new NewDAO().ListAll();
            return View(dao);
        }

        // GET: Admin/New/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            return View();
        }

        #region Create with single img

        [HttpPost]
        [ValidateInput(false)]//chap nhan mã html
        public ActionResult Create(New @new, HttpPostedFileBase fileUpload)
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", @new.CategoryID);
            if (fileUpload == null)
            {
                SetAlert("Vui lòng chọn ảnh", "warning");
                return View();
            }
            else
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(fileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/Assets/Thumbnail/"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.thongbao = "Hình ảnh đã tồn tại";

                }
                else
                {
                    fileUpload.SaveAs(path);
                }
                var dao = new NewDAO();
                var session = (UserLogin)Session["DUY"];
                @new.CreateBy = session.UserName;
                @new.Image = fileName;
                @new.CreateDate = DateTime.Now;
                long id = dao.Create(@new);
                if (id > 0)
                {
                    SetAlert("Thêm tin tức thành công", "success");
                    return RedirectToAction("Index", "New");
                }
                else
                {
                    ModelState.AddModelError("", "Khong tao dc sản phẩm");
                }
            }
            return View("Index");
        }
        #endregion

        // GET: Admin/New/Edit/5
        public ActionResult Edit(long? id)
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
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", @new.CategoryID);
            return View(@new);
        }
        #region Edit with single img

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(New @new, HttpPostedFileBase fileUpload)
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", @new.CategoryID);
            var dao = new NewDAO();
            if (fileUpload == null)
            {
                var session = (UserLogin)Session["DUY"];
                @new.ModifiedBy = session.UserName;
                @new.ModifiedDate = DateTime.Now;
                var result = dao.Update(@new);
                if (result == 1)
                {
                    SetAlert("Sửa tin tứcthành công", "success");
                    return RedirectToAction("Index", "New");
                }
                else if (result == 0)
                {
                    SetAlert("Vui lòng chọn ảnh sản phẩm", "warning");
                }
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Assets/Thumbnail/"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.thongbao = "Hình ảnh đã tồn tại";

                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }

                    var session = (UserLogin)Session["DUY"];
                    @new.ModifiedBy = session.UserName;
                    @new.Image = fileName;
                    @new.ModifiedDate = DateTime.Now;
                    var result = dao.Update(@new);
                    if (result == 1)
                    {
                        SetAlert("Sửa tin tức thành công", "success");
                        return RedirectToAction("Index", "New");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cap nhat khong thanh cong!!");
                    }

                }
            }
            return View("Index");
        }

        #endregion

        // GET: Admin/New/Delete/5
        public ActionResult Delete(long? id)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            New @new = db.News.Find(id);
            db.News.Remove(@new);
            db.SaveChanges();
            SetAlert("Xóa tin tức thành công", "success");
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
