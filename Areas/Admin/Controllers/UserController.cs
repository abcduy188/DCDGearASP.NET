using DCDGear.Common;
using DCDGear.DAO;
using DCDGear.Models;
using DCDGear.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCDGear.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        public ActionResult Index()
        {
            var dao = new UserDAO().ListAll();
            return View(dao);
        }
        public ActionResult Create()
        {
            var dao = new UserDAO();
            ViewBag.GroupID = new SelectList(dao.ListParent(), "ID", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(User user)
        {
            var dao = new UserDAO();
            ViewBag.GroupID = new SelectList(dao.ListParent(), "ID", "Name", user.UserGroup);
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session["DUY"];
                user.CreateBy = session.Name;
                user.CreateDate = DateTime.Now;
                var create = dao.Create(user);
                if (create)
                {
                    SetAlert("Thêm tin tức thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Khong tao dc sản phẩm");
                }
            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dao = new UserDAO();
            var user = dao.GetByID(id);
            ViewBag.GroupID = new SelectList(dao.ListParent(), "ID", "Name", id);
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(User user)
        {

            var dao = new UserDAO();
            ViewBag.GroupID = new SelectList(dao.ListParent(), "ID", "Name", user.UserGroup);
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session["DUY"];
                user.ModifiedBy = session.Name;
                var result = dao.Edit(user);
                if (result)
                {
                    SetAlert("Cap nhat nguoi dung thanh cong", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Khong tao dc nguoi dung");
                }
            }
            return View("Index");
        }
        public ActionResult Delete(long id)
        {
            new UserDAO().Delete(id);
            return Redirect("index");
        }
    }
}