using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserAuthApi.Models
{
    public class ResponseMsg
    {
        public string Message { get; set; }
        public string Status { get; set; }
    }

    public class MessageCumInfo
    { 
       public List<ResponseMsg> ResponseMsgs { get; set; }
       public List<UserInfoModel> UserInfo { get; set; }
    }
}