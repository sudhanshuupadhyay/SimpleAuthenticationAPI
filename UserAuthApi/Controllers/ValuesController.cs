using Microsoft.Ajax.Utilities;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using UserAuthApi.Models;
using UserAuthApi.Utility;
using USerAuthAPI.DAL;

namespace USerAuthAPI.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        ValueDAL valueDAO = new ValueDAL();
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //GET api/values/5
        public string GetQRCode(int mobileNumber)
        {
            string QrCode = valueDAO.GetQRCode(mobileNumber);
            return QrCode;
        }

        // POST api/values
        public IHttpActionResult Post([FromBody] UserInfoModel userInfo)
        {
            string errormsg = "";
            UserInfoModel userInfoModel = new UserInfoModel();
            MessageCumInfo mixedModel = new MessageCumInfo();
            ResponseMsg resp = new ResponseMsg();
            try
            {
               
                userInfoModel = valueDAO.RegisterUser(userInfo, ref errormsg);

                if (userInfoModel == null)
                {
                    resp.Message = errormsg;
                    resp.Status = "False";
                    return Json(resp);
                }
                resp.Message = "User has been registered successfully";
                resp.Status = "True";
                mixedModel.ResponseMsgs.Add(resp);
                mixedModel.UserInfo.Add(userInfoModel);
                return Json(mixedModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                resp.Message = ex.Message;
                resp.Status = "False";
                return Json(resp);
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {

        }
    }
}
