using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserAuthApi.Models
{
    public class UserInfoModel
    {
        public string Name { get; set; }
        public int UserID { get; set; }
        public string Email { get; set; }
        public List<SkillDetails> Skills { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string QRCode { get; set; }
    }
}
