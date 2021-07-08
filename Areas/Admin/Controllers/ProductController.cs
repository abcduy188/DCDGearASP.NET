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
            var dao = db.Products.OrderBy(d => d.Name).ToList();
            return View(dao);
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.ProductCategories.ToList(), "ID", "Name");
            return View();
        }

        public ActionResult Edit(long id)
        {
            Product products = db.Products.Find(id);
            ViewBag.CategoryID = new SelectList(db.ProductCategories.ToList(), "ID", "Name", products.CategoryID);
            return View(products);
        }

        [HttpDelete]
        public ActionResult Delete(long id)
        {
            new ProductDAO().Delete(id);
            return Redirect("Index");
        }
        #region Create with single img

        //[HttpPost]
        //[ValidateInput(false)]//chap nhan mã html
        //public ActionResult Create(Product products, HttpPostedFileBase fileUpload)
        //{
        //    ViewBag.CategoryID = new SelectList(db.ProductCategories.ToList(), "ID", "Name");
        //    if (fileUpload == null)
        //    {
        //        SetAlert("Vui lòng chọn ảnh", "warning");
        //        return View();
        //    }
        //    else
        //    if (ModelState.IsValid) // kiem tra co valid form hay khong
        //    {
        //        var fileName = Path.GetFileName(fileUpload.FileName);
        //        var path = Path.Combine(Server.MapPath("~/Assets/Thumbnail/"), fileName);
        //        if (System.IO.File.Exists(path))
        //        {
        //            ViewBag.thongbao = "Hình ảnh đã tồn tại";

        //        }
        //        else
        //        {
        //            fileUpload.SaveAs(path);
        //        }
        //        var session = (UserLogin)Session["DUY"];
        //        products.CreateBy = session.UserName;
        //        products.Image = fileName;
        //        products.CreateDate = DateTime.Now;
        //        db.Products.Add(products);
        //        db.SaveChanges();
        //        SetAlert("Thêm sản phẩm thành công", "success");
        //        return RedirectToAction("Index", "Product");
        //    }
        //    return View("Index");
        //}
        #endregion
        #region Edit with single img

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Edit(Product entity, HttpPostedFileBase fileUpload)
        //{
        //    ViewBag.CategoryID = new SelectList(db.ProductCategories, "ID", "Name", entity.CategoryID);
        //    var products = db.Products.Find(entity.ID);
        //    var session = (UserLogin)Session["DUY"];
        //    entity.ModifiedBy = session.UserName;
        //    entity.ModifiedDate = DateTime.Now;
        //    if (ModelState.IsValid)
        //    {
        //        if (fileUpload == null)
        //        {
        //            products.Name = entity.Name;
        //            products.CategoryID = entity.CategoryID;
        //            products.SeoTitle = entity.SeoTitle;
        //            products.Description = entity.Description;
        //            try
        //            {
        //                if (!string.IsNullOrEmpty(products.Image))
        //                {
        //                    products.Image = products.Image;
        //                }
        //                else
        //                {
        //                    SetAlert("Vui lòng chọn ảnh sản phẩm", "warning");
        //                    return RedirectToAction("Index", "Product");
        //                }
        //            }
        //            catch
        //            {
        //                ModelState.AddModelError("", "Cap nhat khong thanh cong!!");
        //            }
        //            products.Price = entity.Price;
        //            products.PromotionPrice = entity.PromotionPrice;
        //            products.LinkVideo = entity.LinkVideo;
        //            products.Detail = entity.Detail;
        //            products.Quantity = entity.Quantity;
        //            products.CPU = entity.CPU;
        //            products.OperatingSystem = entity.OperatingSystem;
        //            products.RAM = entity.RAM;
        //            products.GPU = entity.GPU;
        //            products.Screen = entity.Screen;
        //            products.SSDHardDrive = entity.SSDHardDrive;
        //            products.ConnectionPorts = entity.ConnectionPorts;
        //            products.Keyboard = entity.Keyboard;
        //            products.Pin = entity.Pin;
        //            products.Size = entity.Size;
        //            products.Weight = entity.Weight;
        //            products.Status = entity.Status;
        //            products.ModifiedDate = DateTime.Now;
        //            db.SaveChanges();
        //            SetAlert("Sửa tin tức thành công", "success");
        //            return RedirectToAction("Index", "Product");
        //        }
        //        else
        //        {
        //            var fileName = Path.GetFileName(fileUpload.FileName);
        //            var path = Path.Combine(Server.MapPath("~/Assets/Thumbnail/"), fileName);
        //            fileUpload.SaveAs(path);
        //            entity.Image = fileName;
        //            products.Name = entity.Name;
        //            products.CategoryID = entity.CategoryID;
        //            products.SeoTitle = entity.SeoTitle;
        //            products.Description = entity.Description;
        //            products.Image = entity.Image;
        //            products.Price = entity.Price;
        //            products.PromotionPrice = entity.PromotionPrice;
        //            products.LinkVideo = entity.LinkVideo;
        //            products.Detail = entity.Detail;
        //            products.Quantity = entity.Quantity;
        //            products.CPU = entity.CPU;
        //            products.OperatingSystem = entity.OperatingSystem;
        //            products.RAM = entity.RAM;
        //            products.GPU = entity.GPU;
        //            products.Screen = entity.Screen;
        //            products.SSDHardDrive = entity.SSDHardDrive;
        //            products.ConnectionPorts = entity.ConnectionPorts;
        //            products.Keyboard = entity.Keyboard;
        //            products.Pin = entity.Pin;
        //            products.Size = entity.Size;
        //            products.Weight = entity.Weight;
        //            products.Status = entity.Status;
        //            products.ModifiedDate = DateTime.Now;
        //            db.SaveChanges();
        //            SetAlert("Sửa tin tức thành công", "success");
        //            return RedirectToAction("Index", "Product");
        //        }
        //    }
        //    return View("Index");
        //}

        #endregion
        #region Create with multiple img
        [HttpPost]
        [ValidateInput(false)]//chap nhan mã html
        public ActionResult Create(Product products)
        {
            ViewBag.CategoryID = new SelectList(db.ProductCategories.ToList(), "ID", "Name");
            if (ModelState.IsValid) // kiem tra co valid form hay khong
            {
                HttpFileCollectionBase files = Request.Files;
                var x = "";
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase fileUpload = files[i];
                    if (i == 0 && fileUpload.ContentLength == 0)
                    {
                        SetAlert("Vui lòng thêm ảnh cho sản phẩm", "warning");
                        return RedirectToAction("Index", "Product");
                    }
                    else
                    {
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        x += fileName + ",";
                        var path = Path.Combine(Server.MapPath("~/Assets/Thumbnail/"), fileName);
                        fileUpload.SaveAs(path);
                    }
                }
                products.Image = x.Remove(x.Length - 1);
                var session = (UserLogin)Session["DUY"];
                products.CreateBy = session.UserName;
                products.CreateDate = DateTime.Now;
                db.Products.Add(products);
                db.SaveChanges();
                SetAlert("Thêm sản phẩm thành công", "success");
                return RedirectToAction("Index", "Product");
            }
            return View("Index");
        }
        #endregion
        #region Edit with multiple img
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Product entity)
        {
            ViewBag.CategoryID = new SelectList(db.ProductCategories.ToList(), "ID", "Name", entity.CategoryID);
            Product products = db.Products.Find(entity.ID);
            var session = (UserLogin)Session["DUY"];
            products.ModifiedBy = session.UserName;
            if (ModelState.IsValid)
            {
                HttpFileCollectionBase files = Request.Files;

                var x = "";
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase fileUpload = files[i];
                    if (i == 0 && fileUpload.ContentLength == 0)
                    {
                        products.Name = entity.Name;
                        products.CategoryID = entity.CategoryID;
                        products.SeoTitle = entity.SeoTitle;
                        products.Description = entity.Description;
                        try
                        {
                            if (entity.Image == null)
                            {
                                if (!string.IsNullOrEmpty(products.Image))
                                {
                                    products.Image = products.Image;
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Cap nhat khong thanh cong!!");
                                }
                            }
                            else
                            {
                                products.Image = entity.Image;
                            }
                        }
                        catch
                        {
                            ModelState.AddModelError("", "Cap nhat khong thanh cong!!");
                        }
                        products.Price = entity.Price;
                        products.PromotionPrice = entity.PromotionPrice;
                        products.LinkVideo = entity.LinkVideo;
                        products.Detail = entity.Detail;
                        products.Quantity = entity.Quantity;
                        products.CPU = entity.CPU;
                        products.OperatingSystem = entity.OperatingSystem;
                        products.RAM = entity.RAM;
                        products.GPU = entity.GPU;
                        products.Screen = entity.Screen;
                        products.SSDHardDrive = entity.SSDHardDrive;
                        products.ConnectionPorts = entity.ConnectionPorts;
                        products.Keyboard = entity.Keyboard;
                        products.Pin = entity.Pin;
                        products.Size = entity.Size;
                        products.Weight = entity.Weight;
                        products.Status = entity.Status;
                        products.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                        SetAlert("Sửa tin tức thành công", "success");
                        return RedirectToAction("Index", "Product");
                    }
                    else
                    {
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        x += fileName + ",";
                        var path = Path.Combine(Server.MapPath("~/Assets/Thumbnail/"), fileName);
                        fileUpload.SaveAs(path);
                    }
                }
                entity.Image = x.Remove(x.Length - 1);
                products.Name = entity.Name;
                products.CategoryID = entity.CategoryID;
                products.SeoTitle = entity.SeoTitle;
                products.Description = entity.Description;
                try
                {
                    if (entity.Image == null)
                    {
                        if (!string.IsNullOrEmpty(products.Image))
                        {
                            products.Image = products.Image;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Cap nhat khong thanh cong!!");
                        }
                    }
                    else
                    {
                        products.Image = entity.Image;
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Cap nhat khong thanh cong!!");
                }
                products.Price = entity.Price;
                products.PromotionPrice = entity.PromotionPrice;
                products.LinkVideo = entity.LinkVideo;
                products.Detail = entity.Detail;
                products.Quantity = entity.Quantity;
                products.CPU = entity.CPU;
                products.OperatingSystem = entity.OperatingSystem;
                products.RAM = entity.RAM;
                products.GPU = entity.GPU;
                products.Screen = entity.Screen;
                products.SSDHardDrive = entity.SSDHardDrive;
                products.ConnectionPorts = entity.ConnectionPorts;
                products.Keyboard = entity.Keyboard;
                products.Pin = entity.Pin;
                products.Size = entity.Size;
                products.Weight = entity.Weight;
                products.Status = entity.Status;
                products.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                SetAlert("Sửa tin tức thành công", "success");
                return RedirectToAction("Index", "Product");

            }
            return View("Index");
        }
        #endregion
    }
}