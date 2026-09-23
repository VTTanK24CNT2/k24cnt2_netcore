using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BtLabLesson09.Models.ValidationAttributes
{
    /// <summary>
    /// Custom Validation kiểm tra nội dung không được chứa các từ nhạy cảm
    /// </summary>
    public class NoSensitiveWordsAttribute : ValidationAttribute
    {
        // Danh sách các từ nhạy cảm cần lọc theo yêu cầu bài lab
        private readonly string[] _sensitiveWords;

        public NoSensitiveWordsAttribute(params string[] customWords)
        {
            if (customWords != null && customWords.Length > 0)
            {
                _sensitiveWords = customWords;
            }
            else
            {
                // Danh sách mặc định theo yêu cầu đề bài
                _sensitiveWords = new[] { "vịt", "dê", "dâm", "fuck", "bitch", "sex", "lừa đảo" };
            }
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            string text = value.ToString() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(text))
            {
                return ValidationResult.Success;
            }

            string textLower = text.ToLower();
            foreach (var word in _sensitiveWords)
            {
                if (textLower.Contains(word.ToLower()))
                {
                    return new ValidationResult(
                        ErrorMessage ?? $"Nội dung chứa từ nhạy cảm không hợp lệ ('{word}'). Vui lòng sử dụng từ ngữ chuẩn mực.",
                        new[] { validationContext.MemberName ?? "Description" }
                    );
                }
            }

            return ValidationResult.Success;
        }
    }
}
