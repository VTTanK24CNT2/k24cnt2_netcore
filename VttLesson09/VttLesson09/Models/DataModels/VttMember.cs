using System.Data;

namespace VttLesson09.Models.DataModels
{
    public class VttMember
    {
        public int VttMemberId { get; set; }    
        public string VttUserName { get; set; }
        public string VttPassWord { get; set; }
        public string VttEmail { get; set; }
        public string VttPhoneNumber { get; set; }
        public string VttFullName { get; set; }
        public DateTime VttBirthday { get; set; }
    }
}
