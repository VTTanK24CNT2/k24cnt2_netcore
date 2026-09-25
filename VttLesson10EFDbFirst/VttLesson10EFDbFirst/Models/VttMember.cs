using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VttLesson10EFDbFirst.Models;

public partial class VttMember
{
    [Display(Name = "Mã ID")]
    public long Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
    [Display(Name = "Tên đăng nhập")]
    [StringLength(20, ErrorMessage = "Tên đăng nhập tối đa 20 ký tự")]
    public string? VttUserName { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [Display(Name = "Mật khẩu")]
    [DataType(DataType.Password)]
    [StringLength(50, ErrorMessage = "Mật khẩu tối đa 50 ký tự")]
    public string? VttPassword { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
    [Display(Name = "Họ và tên")]
    [StringLength(50, ErrorMessage = "Họ và tên tối đa 50 ký tự")]
    public string? VttFullName { get; set; }

    [Display(Name = "Địa chỉ Email")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    [StringLength(50, ErrorMessage = "Email tối đa 50 ký tự")]
    public string? VttEmail { get; set; }

    [Display(Name = "Số điện thoại")]
    [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
    [StringLength(12, ErrorMessage = "Số điện thoại tối đa 12 ký tự")]
    public string? VttPhone { get; set; }

    [Display(Name = "Trạng thái hoạt động")]
    public bool? VttStatus { get; set; }
}
