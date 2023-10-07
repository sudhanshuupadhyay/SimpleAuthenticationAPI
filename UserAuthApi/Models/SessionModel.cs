using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserAuthApi.Models
{
    public class SessionModel
    {
        public string SessionID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UserID { get; set; }
    }
}