using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using NextMile02.Models;
using Facebook;

namespace NextMile02.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/
        public ActionResult Index()
        {
            return View();
        }

        // NOTE: We don't think this is being called.  We might be wrong.
        //public ActionResult PassUidtoHomeController()
        //{
        //    String uid = getUid();
        //    return RedirectToAction("IndexForLoggedInUser", "Home", new { userId = uid });
        //}

        [HttpPost]
        public JsonResult login(FacebookLoginModel model)
        {
            Session["uid"]          = model.uid;
            Session["accessToken"]  = model.accessToken;
            if (Session["flag"]    != "0")
            {
                Session["flag"] = model.flag;
            }
            return Json(new {success = true});
        }

        [HttpPost]
        public JsonResult logout()
        {
            Session["uid"]         = null;
            Session["accessToken"] = null;
            Session["flag"]        = "-1";
            return Json(new {success = true});
        }

        [HttpGet]
        public ActionResult UserDetails()
        {
            if (Session["accessToken"] == null)
            {
                return View("Welcome");
            }
            var client = new FacebookClient(Session["accessToken"].ToString());
            dynamic fbresult = client.Get("me?fields=id,name,picture.type(large)");
            FacebookUserModel facebookUser = Newtonsoft.Json.JsonConvert.DeserializeObject<FacebookUserModel>(fbresult.ToString());

            return View(facebookUser);
        }

        [HttpGet]
        public String getUid()
        {
            return (String)Session["uid"];
        }


        [HttpPost]
        public JsonResult setFlag()
        {
            Session["flag"] = "0";
            return Json(new {success = true});
        }

        [HttpGet]
        public String getFlag()
        {
            return (String)Session["flag"];
        }

	}
}