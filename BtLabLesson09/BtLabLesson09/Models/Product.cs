using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BtLabLesson09.Models.ValidationAttributes;

namespace BtLabLesson09.Models
{
    public class Product : IValidatableObject
    {
        [Key]
        [Display(Name = "Mã sản phẩm")]
        public int Id { get; set; }

        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(150, MinimumLength = 6, ErrorMessage = "Tên sản phẩm ít nhất là 6 ký tự, nhiều nhất là 150 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Hình ảnh")]
        [Required(ErrorMessage = "Hình ảnh sản phẩm không được để trống")]
        [ProductImage(ErrorMessage = "Ảnh phải được chọn đúng đường dẫn thư mục wwwroot/products (VD: /products/macbook.jpg)")]
        public string Image { get; set; } = string.Empty;

        [Display(Name = "Giá gốc")]
        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [Range(100000f, float.MaxValue, ErrorMessage = "Giá sản phẩm phải nhỏ nhất 100,000 đ (kiểu số thực)")]
        [DisplayFormat(DataFormatString = "{0:N0} đ", ApplyFormatInEditMode = false)]
        public float Price { get; set; }

        [Display(Name = "Giá khuyến mãi")]
        [Required(ErrorMessage = "Giá khuyến mãi không được để trống")]
        [DisplayFormat(DataFormatString = "{0:N0} đ", ApplyFormatInEditMode = false)]
        public float SalePrice { get; set; }

        [Display(Name = "Mô tả sản phẩm")]
        [Required(ErrorMessage = "Mô tả sản phẩm không được để trống")]
        [StringLength(1500, ErrorMessage = "Mô tả sản phẩm không được vượt quá 1500 ký tự")]
        [NoSensitiveWords(ErrorMessage = "Mô tả sản phẩm không được chứa các từ nhạy cảm (như: vịt, dê, dâm, fuck,...)")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Danh mục sản phẩm")]
        [Required(ErrorMessage = "Vui lòng chọn danh mục cho sản phẩm")]
        [Range(1, int.MaxValue, ErrorMessage = "Danh mục sản phẩm không hợp lệ")]
        public int CategoryId { get; set; }

        // Custom validation đa trường (Cross-field validation)
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // 1. Kiểm tra SalePrice không âm
            if (SalePrice < 0)
            {
                yield return new ValidationResult(
                    "Giá khuyến mãi không được là số âm (phải >= 0).",
                    new[] { nameof(SalePrice) }
                );
            }

            // 2. Kiểm tra SalePrice phải nhỏ hơn Price
            if (Price > 0 && SalePrice >= Price)
            {
                yield return new ValidationResult(
                    "Giá khuyến mãi (SalePrice) phải nhỏ hơn giá gốc (Price).",
                    new[] { nameof(SalePrice) }
                );
            }

            // 3. Kiểm tra SalePrice giảm tối đa 10% so với Price (SalePrice >= Price * 0.9)
            if (Price > 0 && SalePrice > 0 && (Price - SalePrice) > (Price * 0.10001f))
            {
                yield return new ValidationResult(
                    $"Giá khuyến mãi chỉ được giảm tối đa 10% so với giá gốc. Giá tối thiểu cho phép là: {(Price * 0.9f):N0} đ.",
                    new[] { nameof(SalePrice) }
                );
            }
        }
    }
}
