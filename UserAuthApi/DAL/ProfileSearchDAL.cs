using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using UserAuthApi.Models;
using UserAuthApi.Utility;

namespace UserAuthApi.DAL
{
    public class ProfileSearchDAL
    {
        //public DataTable FetchUserDetailsBasedOnSearch(string userId, string City, string lattitude, string longitude, string rangeInKM)
        //{
        //    SessionModel sessionModel = new SessionModel();
        //    try
        //    {
        //        //insert query for userRating 
        //        string insertRating = ConfigurationManager.AppSettings["InsertSessionDetail"].ToString();
        //        sessionModel.SessionID = Guid.NewGuid().ToString();
        //        sessionModel.CreatedDate = DateTime.Now;
        //        sessionModel.UpdatedDate = DateTime.Now;
        //        sessionModel.UserID = userId;

        //        using (SqlConnection con = new SqlConnection(conString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand(insertRating, con))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                SqlParameter param = new SqlParameter("@sessionID", sessionModel.SessionID);
        //                cmd.Parameters.Add(param);
        //                param = new SqlParameter("@createdDate", sessionModel.CreatedDate);
        //                cmd.Parameters.Add(param);
        //                param = new SqlParameter("@updatedDate", sessionModel.UpdatedDate);
        //                cmd.Parameters.Add(param);
        //                param = new SqlParameter("@userId", sessionModel.UserID);
        //                cmd.Parameters.Add(param);
        //                con.Open();
        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message + "--> " + ex.StackTrace);
        //        throw ex;
        //    }


        //    return sessionModel;


        //}
    }
}