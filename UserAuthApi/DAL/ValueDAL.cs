using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Drawing.Imaging;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using UserAuthApi.Models;
using UserAuthApi.Utility;
using log4net.Repository.Hierarchy;
using System.Reflection;
using System.Web.Configuration;

namespace USerAuthAPI.DAL
{
    public class ValueDAL
    {
        string conString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
        public UserInfoModel RegisterUser(UserInfoModel userInfo, ref string errormsg)
        {
            userInfo.QRCode = CreateQRCode(userInfo.Mobile);
            try
            {
                //insert query for user 
                string registerSP = ConfigurationManager.AppSettings["RegesterStoredproc"].ToString();

                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand(registerSP, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = userInfo.Name;
                        cmd.Parameters.Add("@Mobile", SqlDbType.VarChar).Value = userInfo.Mobile;
                        cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = userInfo.Address;
                        cmd.Parameters.Add("@Skills", SqlDbType.VarChar).Value = userInfo.Skills;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = userInfo.Email;
                        cmd.Parameters.Add("@QRCode", SqlDbType.VarChar).Value = userInfo.QRCode;

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                errormsg = ex.Message;
                return null;
            }
            return userInfo;

        }
        public int UpdateRating(UserRatingModel userRating)
        {
            int success = 0;
            try
            {
                //insert query for userRating 
                string insertRating = ConfigurationManager.AppSettings["InsertRatingStoredproc"].ToString();

                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand(insertRating, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Rating", SqlDbType.VarChar).Value = userRating.Rating;
                        cmd.Parameters.Add("@Mobile", SqlDbType.VarChar).Value = userRating.UserMobile;
                        cmd.Parameters.Add("@RespondentEmail", SqlDbType.VarChar).Value = userRating.RespondentEmail;
                        cmd.Parameters.Add("@RespondentMobile", SqlDbType.VarChar).Value = userRating.RespondentMobile;
                        cmd.Parameters.Add("@RespondentName", SqlDbType.VarChar).Value = userRating.RespondentName;
                        cmd.Parameters.Add("@ReviewComment", SqlDbType.VarChar).Value = userRating.ReviewComment;
                        cmd.Parameters.Add("@AdditionalAttribute", SqlDbType.VarChar).Value = userRating.AdditionalAttribute == null ? "": userRating.AdditionalAttribute;

                        con.Open();
                        cmd.ExecuteNonQuery();
                        success = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                return success;
            }


            return success;


        }
        public string CreateQRCode(string mobile)
        {
            string QrUri = "";
            try
            {
                QRCodeModel qRCode = new QRCodeModel();
                qRCode.URL = ConfigurationManager.AppSettings["QRURL"].ToString();
                qRCode.URL = qRCode.URL + mobile;
                byte[] BitmapArray;
                QRCodeGenerator QrGenerator = new QRCodeGenerator();
                QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(qRCode.URL, QRCodeGenerator.ECCLevel.Q);
                QRCode QrCode = new QRCode(QrCodeInfo);
                Bitmap QrBitmap = QrCode.GetGraphic(60);
                BitmapArray = QrBitmap.BitmapToByteArray();
                QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                return QrUri;
            }
            return QrUri;
        }
        public string GetQRCode(string mobile)
        {
            string QrCode = "";

            //insert query for userRating 
            string insertRating = ConfigurationManager.AppSettings["InsertRatingStoredproc"].ToString();

            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(insertRating, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select QRCode from UserInfo Where Mobile=@Mobile";
                    SqlParameter param = new SqlParameter("@Mobile", mobile);
                    cmd.Parameters.Add(param);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        QrCode = rdr["QRCode"].ToString();
                    }
                }
            }


            return QrCode;


        }
        public List<UserRatingModel> GetFeedBackInfo(string mobile)
        {
            UserRatingModel ratingModel = new UserRatingModel();
            List<UserRatingModel> ratingList = new List<UserRatingModel>();
            
            try
            {
                string sqlQuery = "Select * from UserRatings Where UserMobile=@Mobile";
                SqlDataAdapter adapter;
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sqlQuery;
                        SqlParameter param = new SqlParameter("@Mobile", mobile);
                        cmd.Parameters.Add(param);
                        con.Open();
                        //Adapter bind to query and connection object
                        adapter = new SqlDataAdapter(cmd);
                        //fill the dataset
                        adapter.Fill(dt);
                    }
                }
                ratingList = ConvertDatatableIntoFeedbackList(dt);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                return null;
            }


            return ratingList;


        }
        public List<UserRatingModel> ConvertDatatableIntoFeedbackList(DataTable dt)
        {
            List<UserRatingModel> ratingList = new List<UserRatingModel>();
            try
            {
                if (dt == null && dt.Rows.Count == 0) return null;

                

                foreach (DataRow dr in dt.Rows)
                {
                    UserRatingModel ratingModel = new UserRatingModel();
                    ratingModel.ResponseID = Convert.ToInt32(dr["ResponseID"].ToString());
                    ratingModel.UserMobile = dr["UserMobile"].ToString();
                    ratingModel.Rating = Convert.ToInt32(dr["Rating"].ToString());
                    ratingModel.ReviewComment = dr["ReviewComment"].ToString();
                    ratingModel.RespondentMobile = dr["RespondentMobile"].ToString();
                    ratingModel.RespondentName = dr["RespondentName"].ToString();
                    ratingModel.RespondentEmail = dr["RespondentEmail"].ToString();

                    ratingList.Add(ratingModel);


                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                return null;
            }
            return ratingList;
        }
        public SessionModel CreateSessionID()
        {
            SessionModel sessionModel = new SessionModel();
            try
            {
                //insert query for userRating 
                string insertRating = "";
                sessionModel.SessionID = Guid.NewGuid().ToString();
                sessionModel.CreatedDate = DateTime.Now;
                sessionModel.UpdatedDate = DateTime.Now;

                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand(insertRating, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter param = new SqlParameter("@sessionID", sessionModel.SessionID);
                        cmd.Parameters.Add(param);
                        param = new SqlParameter("@createdDate", sessionModel.CreatedDate);
                        cmd.Parameters.Add(param);
                        param = new SqlParameter("@updatedDate", sessionModel.UpdatedDate);
                        cmd.Parameters.Add(param);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                return sessionModel;
            }


            return sessionModel;


        }

        public SessionModel fetchSessionDetails(string sessionID)
        {
            SessionModel sessionModel = new SessionModel();
            try
            {
                string sqlQuery = "Select * from SessionDetail Where SessionID=@sessionID";

                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sqlQuery;
                        SqlParameter param = new SqlParameter("@sessionID", sessionID);
                        cmd.Parameters.Add(param);
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            sessionModel.SessionID = rdr["SessionID"].ToString();
                            sessionModel.CreatedDate = Convert.ToDateTime(rdr["CreatedDate"].ToString());
                            sessionModel.UpdatedDate = Convert.ToDateTime(rdr["UpdatedDate"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                return sessionModel;
            }

            return sessionModel;

        }
        public bool ValidateSession(string sessionID)
        {
            bool isValid = false;


            try
            {
                SessionModel model = new SessionModel();
                model = fetchSessionDetails(sessionID);

                if (model != null)
                {
                    var diffOfDates = model.UpdatedDate - DateTime.Now;
                    var allowedDiff = Convert.ToInt32(ConfigurationManager.AppSettings["AllowedDiff"].ToString());
                    if (diffOfDates.Hours > (allowedDiff - 1))
                    {
                        isValid = true;
                    }
                }
                else { isValid = false; }


            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "--> " + ex.StackTrace);
                return isValid;
            }


            return isValid;

        }

    }
    //Extension method to convert Bitmap to Byte Array
    public static class BitmapExtension
    {
        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }



}
