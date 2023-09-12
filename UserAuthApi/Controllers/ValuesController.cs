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
        [HttpGet]
        [Route("GetQRCode")]
        public string GetQRCode(string mobileNumber)
        {
            string QrCode = valueDAO.GetQRCode(mobileNumber);
            return QrCode;
        }

        // POST api/values
        [HttpPost]
        [Route("RegisterUser")]
        public IHttpActionResult Post([FromBody] UserInfoModel userInfo)
        {
            string errormsg = "";
            ResponseAndData mixedModel = new ResponseAndData();
            mixedModel.ResponseMsg = new ResponseMsg();
            mixedModel.UserInfo = new UserInfoModel();
            try
            {

                mixedModel.UserInfo = valueDAO.RegisterUser(userInfo, ref errormsg);

                if (mixedModel.UserInfo == null)
                {
                    mixedModel.ResponseMsg.Message = errormsg;
                    mixedModel.ResponseMsg.Status = "False";
                    return Json(mixedModel);
                }
                mixedModel.ResponseMsg.Message = "User has been registered successfully";
                mixedModel.ResponseMsg.Status = "True";
                
                return Json(mixedModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                mixedModel.ResponseMsg.Message = ex.Message;
                mixedModel.ResponseMsg.Status = "False";
                return Json(mixedModel.ResponseMsg);
            }
        }
        [HttpPost]
        [Route("UpdateUserRating")]
        public IHttpActionResult UpdateUserRating([FromBody] UserRatingModel userRating)
        {
            
            ResponseMsg response = new ResponseMsg();

            ResponseAndData mixedModel = new ResponseAndData();
            mixedModel.ResponseMsg = new ResponseMsg();
            mixedModel.RatingInfoList = new List<UserRatingModel>();
            int success = 0;
            try
            {

                success = valueDAO.UpdateRating(userRating);

                if (success == 0)
                {
                    mixedModel.ResponseMsg.Message = "Invalid feedback";
                    mixedModel.ResponseMsg.Status = "False";
                    return Json(response);
                }
                mixedModel.ResponseMsg.Message = "Feedback has been registered successfully";
                mixedModel.ResponseMsg.Status = "True";
                mixedModel.RatingInfoList.Add(userRating);


                return Json(mixedModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                mixedModel.ResponseMsg.Message = ex.Message;
                mixedModel.ResponseMsg.Status = "False";
                return Json(mixedModel.ResponseMsg);
            }
        }

        [HttpGet]
        [Route("GetFeedBackInfo")]
        public IHttpActionResult GetFeedBackInfo(string mobileNumber)
        {
            
            ResponseAndData mixedModel = new ResponseAndData();
            mixedModel.ResponseMsg = new ResponseMsg();
            mixedModel.RatingInfoList = new List<UserRatingModel>();

            try
            {

                mixedModel.RatingInfoList = valueDAO.GetFeedBackInfo(mobileNumber);
                if (mixedModel.RatingInfoList == null)
                {
                    mixedModel.ResponseMsg.Message = "No feedback to show.";
                    mixedModel.ResponseMsg.Status = "False";
                    return Json(mixedModel.ResponseMsg);
                }
                else
                {
                    mixedModel.ResponseMsg.Message = "Success";
                    mixedModel.ResponseMsg.Status = "true";

                }

                return Json(mixedModel);
            }
             catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                mixedModel.ResponseMsg.Message = ex.Message;
                mixedModel.ResponseMsg.Status = "False";
                return Json(mixedModel.ResponseMsg);
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
