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
        public string IsCurrent { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
