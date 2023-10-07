using Microsoft.Ajax.Utilities;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
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
        public string GetQRCode(string mobileNumber ,string sessionId,string userID)
        {
            if (valueDAO.isSessionIDValid(sessionId))
            {
                string QrCode = valueDAO.GetQRCode(mobileNumber);
                return QrCode;
            }
            return "Invalid Session";
        }
        [HttpGet]
        [Route("GetUserDetail")]
        public IHttpActionResult GetUserDetail(string mobileNumber,string sessionId,string userID)
        {
            if (valueDAO.isSessionIDValid(sessionId))
            {
                UserInfoModel userDetail = valueDAO.GetUserDetail(mobileNumber);
                userDetail.Skills = valueDAO.GetSkillDetails(userDetail.UserID);
                return Json(userDetail);
            }
            return Json("Invalid Session");
        }

        // POST api/values
        [HttpPost]
        [Route("RegisterUser")]
        public IHttpActionResult Post([FromBody] UserInfoModel userInfo)
        {
            string errormsg = "";
            int SkillInsertionSuccess = 0;
            ResponseAndData mixedModel = new ResponseAndData();
            mixedModel.ResponseMsg = new ResponseMsg();
            mixedModel.UserInfo = new UserInfoModel();
            mixedModel.SessionInfo = new SessionModel();
            try
            {

                mixedModel.UserInfo = valueDAO.RegisterUser(userInfo, ref errormsg);

                if (mixedModel.UserInfo == null)
                {
                    mixedModel.ResponseMsg.Message = errormsg;
                    mixedModel.ResponseMsg.Status = "False";
                    return Json(mixedModel);
                }
                mixedModel.SessionInfo = valueDAO.CreateSessionID(Convert.ToString(mixedModel.UserInfo.UserID));
                SkillInsertionSuccess = valueDAO.InsertSkillDetailRowWise(userInfo.Skills,userInfo.Mobile);
                if (SkillInsertionSuccess == 1)
                {
                    mixedModel.UserInfo.Skills = userInfo.Skills;
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
        public IHttpActionResult UpdateUserRating([FromBody] UserRatingModel userRating,string sessionId)
        {
            if (!valueDAO.isSessionIDValid(sessionId))
            {
                return Json("Invalid Session");
            }

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
        public IHttpActionResult GetFeedBackInfo(string mobileNumber,string sessionId,string userID)
        {
            if (!valueDAO.isSessionIDValid(sessionId))
            {
                return Json("Invalid Session");
            }

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
        [HttpGet]
        [Route("EncryptSessionId")]
        public IHttpActionResult EncryptSessionId(string sessionId)
        {
            ResponseMsg response = new ResponseMsg();
            try
            {
                string key = ConfigurationManager.AppSettings["CypherText"];
                string encryptedtext = Security.EncryptString(key, sessionId);
                response.Message = encryptedtext;
                response.Status = "True";
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                response.Message = ex.Message;
                response.Status = "False";
            }
            return Json(response);
        }
        [HttpGet]
        [Route("CreateSession")]
        public IHttpActionResult CreateSession(string userID)
        {
            ResponseMsg response = new ResponseMsg();
            SessionModel sessionDetail = new SessionModel();
            try
            {
                sessionDetail = valueDAO.CreateSessionID(userID);
                
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                response.Message = ex.Message;
                response.Status = "False";
                return Json(response);

            }
            return Json(sessionDetail);
        }
        [HttpGet]
        [Route("DecryptSessionId")]
        public IHttpActionResult DecryptSessionId(string sessionId)
        {
            ResponseMsg response = new ResponseMsg();
            try
            {

                string key = ConfigurationManager.AppSettings["CypherText"];
                string decryptedText = Security.DecryptString(key, sessionId);
                response.Message = decryptedText;
                response.Status = "True";
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                response.Message = ex.Message;
                response.Status = "False";
            }
            return Json(response);
        }

        [HttpGet]
        [Route("UpdateProfile")]
        public IHttpActionResult UppdateProfile(string SessionID,UserInfoModel userInfo)
        {
            ResponseMsg response = new ResponseMsg();
            try
            {

                response.Message = "test";
                response.Status = "True";
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                response.Message = ex.Message;
                response.Status = "False";
            }
            return Json(response);
        }

        [HttpGet]
        [Route("SearchUser")]
        public IHttpActionResult SearchUser(string SessionID,string City,string lattitude,string longitude,string rangeInKM, string userID)
        {
            ResponseMsg response = new ResponseMsg();
            try
            {

                response.Message = "test";
                response.Status = "True";
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                response.Message = ex.Message;
                response.Status = "False";
            }
            return Json(response);
        }
    }
}
