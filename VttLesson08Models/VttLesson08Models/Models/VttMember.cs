using System.ComponentModel;

namespace VttLesson08Models.Models
{
    public class VttMember
    {
        public string VttMemberId { get; set; }
        public string VttUserName { get; set; }
        public string VttPassword { get; set; }

        [DisplayName("Họ và tên")]
        public string VttFullName { get; set; }
        public string VttEmail { get; set; }
    }

}
