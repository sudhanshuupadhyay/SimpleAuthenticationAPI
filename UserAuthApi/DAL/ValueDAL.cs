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

namespace USerAuthAPI.DAL
{
    public class ValueDAL
    {
        string conString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
        public UserInfoModel RegisterUser(UserInfoModel userInfo,ref string errormsg)
        {
            userInfo.QRCode = CreateQRCode();
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
        public string CreateQRCode()
        {
            QRCodeModel qRCode = new QRCodeModel();
            qRCode.URL = ConfigurationManager.AppSettings["QRURL"].ToString();
            byte[] BitmapArray;
            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(qRCode.URL, QRCodeGenerator.ECCLevel.Q);
            QRCode QrCode = new QRCode(QrCodeInfo);
            Bitmap QrBitmap = QrCode.GetGraphic(60);
            BitmapArray = QrBitmap.BitmapToByteArray();
            string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
            return QrUri;
        }
        public string GetQRCode(int mobile)
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