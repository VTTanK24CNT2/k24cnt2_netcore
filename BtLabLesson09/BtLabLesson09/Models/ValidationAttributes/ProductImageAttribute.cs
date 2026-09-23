using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace BtLabLesson09.Models.ValidationAttributes
{
    /// <summary>
    /// Custom Validation kiểm tra đường dẫn hình ảnh phải thuộc thư mục wwwroot/products
    /// </summary>
    public class ProductImageAttribute : ValidationAttribute
    {
        private readonly string[] _validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg" };

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                // Để [Required] bắt lỗi nếu rỗng
                return ValidationResult.Success;
            }

            string path = value.ToString()!.Trim();
            // Chuẩn hóa dấu gạch chéo
            string normalizedPath = path.Replace("\\", "/");

            // Kiểm tra đường dẫn phải thuộc thư mục products hoặc wwwroot/products
            bool isInProductsDir = normalizedPath.StartsWith("/products/", StringComparison.OrdinalIgnoreCase)
                                || normalizedPath.StartsWith("products/", StringComparison.OrdinalIgnoreCase)
                                || normalizedPath.StartsWith("wwwroot/products/", StringComparison.OrdinalIgnoreCase)
                                || normalizedPath.StartsWith("/wwwroot/products/", StringComparison.OrdinalIgnoreCase);

            if (!isInProductsDir)
            {
                return new ValidationResult(
                    ErrorMessage ?? "Đường dẫn ảnh phải thuộc thư mục wwwroot/products (Ví dụ: /products/laptop.jpg hoặc products/laptop.jpg)",
                    new[] { validationContext.MemberName ?? "Image" }
                );
            }

            // Kiểm tra định dạng đuôi file ảnh
            string extension = Path.GetExtension(normalizedPath).ToLower();
            if (string.IsNullOrEmpty(extension) || !_validExtensions.Contains(extension))
            {
                return new ValidationResult(
                    "Định dạng ảnh không hợp lệ. Vui lòng sử dụng các định dạng: .jpg, .jpeg, .png, .gif, .webp, .svg",
                    new[] { validationContext.MemberName ?? "Image" }
                );
            }

            return ValidationResult.Success;
        }
    }
}
