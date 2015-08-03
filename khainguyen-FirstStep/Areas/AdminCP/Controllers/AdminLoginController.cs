using Facebook;
using MvcLibrary.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace khainguyen_FirstStep.Areas.AdminCP
{
    public class AdminLoginController : Controller
    {
        //
        // GET: /AdminCP/AdminLogin/

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult CheckId(string username, string password, string role)
        {
            EntityAdmin user = new EntityAdmin();
            user.Username = " ";
            try
            {
                dbFirstStepDataContext db = new dbFirstStepDataContext();

                Security ser = new Security();
                string passHex = ser.GetHashPassword(password);

                var admin = db.EntityAdmins.Where(t => t.Username == username && t.Pass == passHex).First();

                if (admin != null)
                {
                    Response.Cookies["Admin"].Value = admin.Username;
                    Response.Cookies["Admin"].Expires = DateTime.Now.AddDays(1);
                    user.Username = "ok";
                    return Json(user);
                }
                else
                {
                    return Json(user);
                }

            }
            catch
            {
                return Json(user);
            }
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            var c = new HttpCookie("Admin");
            c.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(c);
            return RedirectToAction("Index", "AdminLogin");
        }
        // mat khau
        public ActionResult QuenMatKhau()
        {
            return View();
        }
        public string checkusername(string username, string role)
        {
            EntityAdmin user = new EntityAdmin();
            try
            {
                dbFirstStepDataContext db = new dbFirstStepDataContext();

                Security ser = new Security();
                var adminquery = db.EntityAdmins.FirstOrDefault(o => o.Username == username);

                return "true";

            }
            catch
            {
                return "false";
            }
        }


       
    }
}
