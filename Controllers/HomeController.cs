using DCDGear.Common;
using DCDGear.Models;
using DCDGear.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCDGear.Controllers
{
    public class HomeController : Controller
    {
        private DCDGearDbContext db = new DCDGearDbContext();
        public ActionResult Index()
        {
            var banner = db.Banners.FirstOrDefault(d=>d.Type==1);
            ViewBag.ListProductsNew= db.Products.Where(d=>d.Status==true).OrderByDescending(d => d.CreateDate).Take(8).ToList();
            ViewBag.ListNewsNew = db.News.Where(d => d.Status == true).OrderByDescending(d => d.CreateDate).Take(8).ToList();
            ViewBag.ListNewsNew1 = db.News.OrderByDescending(d => d.CreateDate).Where(d=>d.Status==true).Take(1).ToList();
            ViewBag.ListRepresentative = db.Products.Where(d => d.Code=="01" && d.Status==true).OrderByDescending(d => d.CreateDate).Take(4).ToList();
            return View(banner);
        }
      
        public ActionResult Info()
        {
            var sess = (UserLogin)Session["DUY"];
            User user = db.Users.Find(sess.UserID);
            if (sess != null)
            {
                return View(user);
            }
            else
            {
                return Redirect("/dang-nhap");
            }
        }
        public ActionResult ChangePassWord()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassWord(UserModel model)
        {
            var user = new User();
            var sess = Session["DUY"] as UserLogin;
            user = db.Users.Single(d => d.UserName == sess.UserName);
            if(ModelState.IsValid)
            { 
                if(sess!=null)
                {
                    if (user.PassWord != Encryptor.MD5Hash(model.OldPassWord))
                    {
                        ModelState.AddModelError("", "Mật khẩu cũ không đúng");
                    }
                    else
                    {
                        user.PassWord = Encryptor.MD5Hash(model.NewPassWord);
                        user.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                        ViewBag.Success = "Mật khẩu đã được cập nhật";
                        return Redirect("/thong-tin-ca-nhan-" + user.ID);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bạn phải đăng nhập trước ");
                }    
                
            }
            return View();
        }
    }
}