using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VttLesson14CoreA7.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Mã sản phẩm")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm tối đa 100 ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập giá bán")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá bán phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Giá bán")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        [StringLength(100, ErrorMessage = "Đường dẫn ảnh tối đa 100 ký tự")]
        [Display(Name = "Hình ảnh đại diện")]
        public string? Image { get; set; }

        // Navigation property
        [ForeignKey("CategoryId")]
        [Display(Name = "Danh mục")]
        public virtual Category? Category { get; set; }
    }
}
