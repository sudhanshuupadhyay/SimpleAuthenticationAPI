using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UserAuthApi.Models;
using UserAuthApi.Utility;
using USerAuthAPI.DAL;

namespace UserAuthApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public ActionResult UserRatingView(string mobile)
        {
            ViewBag.Mobile = mobile;
            return View();
        }

        public ActionResult UpdateUserRating(UserRatingModel userRatingModel)
        {
            try
            {
                ValueDAL value = new ValueDAL();
                if (userRatingModel != null)
                {
                    value.UpdateRating(userRatingModel);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error Occured in UpdateUserRating - " + ex.Message + "->" + ex.StackTrace);
            }

            return View("ThankYouPage");
        }
        public ActionResult ThankYouPage()
        {
            
            return View();
        }

    }
}
