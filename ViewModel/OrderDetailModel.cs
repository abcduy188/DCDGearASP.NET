using DCDGear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCDGear.ViewModel
{
    public class OrderDetailModel
    {
        private DCDGearDbContext db = new DCDGearDbContext();
        public long ID { set; get; }
        public string OrderUser { set; get; }
        public string ShipName { set; get; }
        public string ShipAddress { set; get; }
        public string ShipPhone { set; get; }
        public string ShipEmail { set; get; }
        public decimal? Price { set; get; }
        public string ProductOrder { set; get; }
        public int? Status { set; get; }
        public int? Quantity { set; get; }
        public List<OrderDetailModel> ListAll()
        {
            var model = (from p in db.Products
                         join od in db.OrderDetails on p.ID equals od.ProductID
                         join o in db.Orders on od.OrderID equals o.ID
                         join u in db.Users on o.UserID equals u.ID
                         select new OrderDetailModel
                         {
                             ID = o.ID,
                             OrderUser = u.Name,
                             ShipName = o.ShipName,
                             ShipAddress = o.ShipAddress,
                             ShipPhone = o.ShipPhone,
                             ShipEmail = o.ShipEmail,
                             Price = od.Price,
                             ProductOrder = p.Name,
                             Quantity = od.Quantity,
                             Status = o.Status,
                         });
            return model.OrderBy(d=>d.ShipName).ToList();
        }
    }
}