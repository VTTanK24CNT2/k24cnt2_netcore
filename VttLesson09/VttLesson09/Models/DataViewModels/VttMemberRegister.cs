using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VttLesson09.Models.DataViewModels
{
    public class VttMemberRegister
    {
        public int VttMemberId { get; set; }
        [DisplayName("Tên đăng nhập")]
        [Required(ErrorMessage ="Tên đăng nhập không để trống")]
        [StringLength(20,MinimumLength=3,ErrorMessage ="Tên đăng nhập tối thiểu 8 ký tự + ký tự đặc biệt")]
        public string VttUserName { get; set; }
        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "mật khẩu không để trống")]
        [DataType(DataType.Password)]
        public string VttPassWord { get; set; }
        public string VttEmail { get; set; }
        public string VttPhoneNumber { get; set; }
        public string VttFullName { get; set; }
        public DateTime VttBirthday { get; set; }
    }
}
