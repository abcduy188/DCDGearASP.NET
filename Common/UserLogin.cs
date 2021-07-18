using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DCDGear.Common
{
    [Serializable] 
    //lưu trữ dữ liệu
    public class UserLogin
    {
        public long UserID { set; get; } //các hàm properties
        public string UserName { set; get; }
        public string Name { set; get; }
        public bool Status { set; get; }
    }
}