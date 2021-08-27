using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DCDGear.ViewModel
{
    public class UserModel
    {
        [Key]
        public long ID { set; get; }
        [Display(Name = "Mật khẩu cũ")]
        [Required(ErrorMessage = "Thông tin bắt buộc!!")]

        public string OldPassWord { set; get; }
        [Display(Name = "Mật khẩu mới")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 kí tự!")]
        [Required(ErrorMessage = "Thông tin bắt buộc!!")]
        public string NewPassWord { set; get; }
        [Display(Name = "Xác nhận mật khẩu mới")]
        [Compare("NewPassWord", ErrorMessage = "Mật khẩu nhập lại không trùng khớp")]
        [Required(ErrorMessage = "Thông tin bắt buộc!!")]
        public string ConfirmPassWord { set; get; }
    }
}