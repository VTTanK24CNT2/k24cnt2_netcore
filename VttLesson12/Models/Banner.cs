using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace VttLesson12.Models;

[Table("Banner")]
public class Banner
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Mã Banner")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên banner không được để trống")]
    [StringLength(150, ErrorMessage = "Tên banner tối đa 150 ký tự")]
    [Display(Name = "Tên banner")]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    [Display(Name = "Hình ảnh")]
    public string? Image { get; set; }

    [NotMapped]
    [Display(Name = "Chọn tệp hình ảnh banner")]
    public IFormFile? ImageUpload { get; set; }

    [StringLength(500, ErrorMessage = "Mô tả tối đa 500 ký tự")]
    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [Display(Name = "Ngày tạo")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [Display(Name = "Trạng thái")]
    public byte Status { get; set; } = 1; // 1: Hiển thị, 0: Ẩn
}
