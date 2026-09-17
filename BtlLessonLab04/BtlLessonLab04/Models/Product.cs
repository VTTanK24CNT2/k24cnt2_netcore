using System.ComponentModel.DataAnnotations;

namespace BtlLessonLab04.Models
{
    public class Product
    {
        [Display(Name = "Mã sản phẩm")]
        public int Id { get; set; }

        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Giá bán")]
        [DisplayFormat(DataFormatString = "{0:#,##0 VNĐ}")]
        public decimal Price { get; set; }

        [Display(Name = "Giá khuyến mãi")]
        [DisplayFormat(DataFormatString = "{0:#,##0 VNĐ}")]
        public decimal SalePrice { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Hình ảnh")]
        public string? Image { get; set; }

        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }
    }
}
