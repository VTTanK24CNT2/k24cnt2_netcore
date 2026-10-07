using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VttLesson14CoreA7.Models
{
    [Table("BannerBlog")]
    public class BannerBlog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Mã")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(200, ErrorMessage = "Tiêu đề tối đa 200 ký tự")]
        [Display(Name = "Tiêu đề / Tên")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mô tả chi tiết không được để trống")]
        [StringLength(350, ErrorMessage = "Mô tả tối đa 350 ký tự")]
        [Display(Name = "Mô tả chi tiết nội dung")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Đường dẫn ảnh tối đa 100 ký tự")]
        [Display(Name = "Hình ảnh")]
        public string? Image { get; set; }

        [Display(Name = "Phân loại")]
        [StringLength(50)]
        public string Type { get; set; } = "Banner"; // Banner hoặc Blog

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Trạng thái hiển thị")]
        public bool IsActive { get; set; } = true;
    }
}
