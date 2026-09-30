using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace VttLesson12.Models;

public partial class Product
{
    [Key]
    [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
    [StringLength(20, ErrorMessage = "Mã sản phẩm tối đa 20 ký tự")]
    [Display(Name = "Mã sản phẩm")]
    public string VttId { get; set; } = null!;

    [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
    [StringLength(100, ErrorMessage = "Tên sản phẩm tối đa 100 ký tự")]
    [Display(Name = "Tên sản phẩm")]
    public string VttName { get; set; } = null!;

    [Required(ErrorMessage = "Đơn giá không được để trống")]
    [Range(0, 999999999999, ErrorMessage = "Đơn giá phải lớn hơn hoặc bằng 0")]
    [Display(Name = "Đơn giá (VNĐ)")]
    public decimal VttPrice { get; set; }

    [Display(Name = "Giá khuyến mãi (VNĐ)")]
    [Range(0, 999999999999, ErrorMessage = "Giá khuyến mãi phải lớn hơn hoặc bằng 0")]
    public decimal? VttSalePrice { get; set; }

    [Required(ErrorMessage = "Trạng thái không được để trống")]
    [StringLength(50)]
    [Display(Name = "Trạng thái")]
    public string VttStatus { get; set; } = "Còn hàng";

    [Display(Name = "Ngày tạo")]
    public DateTime VttCreateDate { get; set; } = DateTime.Now;

    [Display(Name = "Hình ảnh")]
    public string? VttImages { get; set; }

    [NotMapped]
    [Display(Name = "Chọn tệp hình ảnh")]
    public IFormFile? ImageUpload { get; set; }

    [Display(Name = "Mã loại")]
    public string? VttCategoryId { get; set; }

    [Display(Name = "Mô tả")]
    [StringLength(500, ErrorMessage = "Mô tả tối đa 500 ký tự")]
    public string? VttDescription { get; set; }
}
