using DCDGear.Common;
using DCDGear.DAO;
using DCDGear.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCDGear.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private DCDGearDbContext db = new DCDGearDbContext();
        // GET: Admin/Product
        public ActionResult Index()
        {
            var dao = new ProductDAO().ListAll() ;
            return View(dao);
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product products, HttpPostedFileBase fileUpload)
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", products.CategoryID);
            if (fileUpload == null)
            {
                //SetAlert("Vui lòng chọn ảnh", "warning");
                return View();
            }
            else
            if (ModelState.IsValid) // kiem tra co valid form hay khong
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
                var dao = new ProductDAO();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                products.CreateBy = session.UserName;
                products.Image = fileName;
                products.CreateDate = DateTime.Now;
                long id = dao.Create(products);
                if (id > 0)
                {
                    SetAlert("Thêm sanr pham thành công", "succes");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Khong tao dc sản phẩm");
                }
            }
            return View("Index");
        }
        public ActionResult Edit(long id)
        {
            var dao = new ProductDAO().GetByID(id);
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", id);
            return View(dao);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Product products, HttpPostedFileBase fileUpload)
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", products.ID);
            if (fileUpload == null)
            {
                //SetAlert("Vui lòng chọn ảnh", "warning");
                ViewBag.thongbao = "Vui lòng chọn ảnh đại diện";
                return View();
            }
            else
           if (ModelState.IsValid) // kiem tra co valid form hay khong
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
                var dao = new ProductDAO();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                products.ModifiedBy = session.UserName;
                products.Image = fileName;
                products.ModifiedDate = DateTime.Now;
                var result = dao.Update(products);
                if (result)
                {
                    //SetAlert("Sửa tin tức thành công", "succes");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Cap nhat khong thanh cong!!");
                }

            }
            return View("Index");
        }
        //public ActionResult Delete(long id)
        //{
        //    new ProductDAO().Delete(id);
        //    return Redirect("Index");
        //}
        public ActionResult delete(long ID)
        {
            return Json("delete", JsonRequestBehavior.AllowGet);
        }
    }
}