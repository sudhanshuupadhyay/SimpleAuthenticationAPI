using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

    
namespace UserAuthApi.Models
{
    public class ResponseAndData
    {
        public ResponseMsg ResponseMsg { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public SessionModel SessionInfo { get; set; }
        public List<UserRatingModel> RatingInfoList { get; set; }
    }
}