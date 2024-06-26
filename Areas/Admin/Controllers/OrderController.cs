﻿using DCDGear.Models;
using DCDGear.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCDGear.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        // GET: Admin/Order
        private DCDGearDbContext db = new DCDGearDbContext();
        public ActionResult Index()
        {
            var model = new OrderDetailModel();
            var list = model.ListAll();
            return View(list);
        }
        [HttpPost]
        public ActionResult Update(long id)
        {
            var model = new OrderDetailModel();
            var order = db.Orders.Find(id);
            order.Status = 0;
            db.SaveChanges();
            return Redirect("Index");
        }
        public ActionResult Delete(long id)
        {
            var user = db.Orders.Find(id);

            return View(user);
        }
        [HttpPost]
        public ActionResult Delete(Order entity)
        {
            Order order = db.Orders.Find(entity.ID);
            order.Status = -1;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}