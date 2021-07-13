using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DCDGear.ViewModel
{
    public class RegisterModel 
    {
        //dung 1 so thuoc tinh can thiet, khong can dung userModels
        [Key]
        public long ID { set; get; }
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Thông tin bắt buộc!!")]
        public string UserName { set; get; }
        [Display(Name = "Mật khẩu")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 kí tự!")]
        [Required(ErrorMessage = "Thông tin bắt buộc!!")]
        public string PassWord { set; get; }
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("PassWord", ErrorMessage = "Mật khẩu nhập lại không trùng khớp")]
        [Required(ErrorMessage = "Thông tin bắt buộc!!")]
        public string ConfirmPassWord { set; get; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Thông tin bắt buộc!!")]
        public string Email { set; get; }
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Thông tin bắt buộc!!")]
        public string Name { set; get; }

        [Display(Name = "Số điện thoại")]
        public string Phone { set; get; }

    }
}