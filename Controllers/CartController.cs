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
    public class CartController : Controller
    {
        private DCDGearDbContext db = new DCDGearDbContext();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        public List<Cart> ListCart()
        {
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart == null)
            {
                listCart = new List<Cart>();
                Session["Cart"] = listCart;
            }
            return listCart;
        }
        public ActionResult AddCart(long ID, string strURL)
        {
            List<Cart> listCart = ListCart();
            Cart item = listCart.Find(n => n.ID == ID);
            if (item == null)
            {
                item = new Cart(ID);
                item.strURL = strURL;
                listCart.Add(item);
                return Redirect(strURL);
            }
            else
            {
                item.Quantity++;
                return Redirect(strURL);
            }
        }
        public int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart != null)
            {
                iTongSoLuong = listCart.Sum(n => n.Quantity);
            }
            return iTongSoLuong;
        }
        private decimal? TongTien()
        {
            decimal? iTongTien = 0;
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart != null)
            {
                iTongTien = listCart.Sum(n => n.TotalItem);
            }
            return iTongTien;
        }
        public ActionResult Cart()
        {
            List<Cart> listCart = ListCart();
            if (listCart.Count == 0)
            {

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(listCart);
        }
        [ChildActionOnly]
        public PartialViewResult CartPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        public ActionResult XoaCart(long ID)
        {
            List<Cart> listCart = ListCart();
            Cart sanpham = listCart.SingleOrDefault(n => n.ID == ID);
            if (sanpham != null)
            {
                listCart.RemoveAll(n => n.ID == ID);
                return RedirectToAction("Cart");
            }
            if (listCart.Count == 0)
            {
                return RedirectToAction("Index", "Book");
            }
            return RedirectToAction("Cart");
        }
        public ActionResult CapnhatCart(long ID, FormCollection f)
        {
            List<Cart> listCart = ListCart();
            Cart sanpham = listCart.SingleOrDefault(n => n.ID == ID);
            if (sanpham != null)
            {
                sanpham.Quantity = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Cart");
        }
        public ActionResult XoatatcaCart()
        {
            List<Cart> listCart = ListCart();
            listCart.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Payment()
        {
            var session = Session["Cart"];
            var list = new List<Cart>();
            if (session != null)
            {
                list = (List<Cart>)session;
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(list);
        }
        [HttpPost]
        public ActionResult Payment(string shipName, string shipPhone, string shipAddress, string shipEmail)
        {
            var order = new Order();
            var user = (UserLogin)Session["DUY"];
            order.UserID = user.UserID;
            order.CreateDate = DateTime.Now;
            order.Status = 1;
            order.ShipName = shipName;
            order.ShipPhone = shipPhone;
            order.ShipAddress = shipAddress;
            order.ShipEmail = shipEmail;
            try
            {
                var dao = db.Orders.Add(order);
                db.SaveChanges();
                var id = dao.ID;
                var car = (List<Cart>)Session["Cart"];
                var detailDAO = new OrderDetail();

                foreach (var item in car)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.ID;
                    orderDetail.OrderID = id;
                    orderDetail.Price = item.Price;
                    orderDetail.Quantity = item.Quantity;
                    var detail = db.OrderDetails.Add(orderDetail);
                    Product product = db.Products.Single(d=>d.ID == item.ID);
                    product.Quantity = product.Quantity - orderDetail.Quantity;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            List<Cart> listCart = ListCart();
            listCart.Clear();
            return Redirect("/Cart/Success");
        }
        public ActionResult Success()
        {
            return View();
        }
    }
}