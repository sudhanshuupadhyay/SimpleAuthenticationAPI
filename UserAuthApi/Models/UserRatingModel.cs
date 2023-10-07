using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserAuthApi.Models
{
    public class UserRatingModel
    {
        public int ResponseID { get; set; }
        public string UserMobile { get; set; }
        public string RespondentName { get; set; }
        public string RespondentMobile { get; set; }
        public string RespondentEmail { get; set; }
        public string ReviewComment { get; set; }
        public int Rating { get; set; }
        public string AdditionalAttribute { get; set; }
        public string ReviewReason { get; set; }

    }
}
