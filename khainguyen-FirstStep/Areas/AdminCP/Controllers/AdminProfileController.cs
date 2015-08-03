using MvcLibrary.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace khainguyen_FirstStep.Areas.AdminCP
{
    public class AdminProfileController : Controller
    {
        //
        // GET: /AdminCP/AdminProfile/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult changeAccount(string username, string password)
        {
            try
            {

                dbFirstStepDataContext db = new dbFirstStepDataContext();

                var adminquery = (from p in db.EntityAdmins
                                  where p.Username == Request.Cookies["Admin"].Value
                                  select p).Single();
                Security ser = new Security();
                string hex = ser.GetHashPassword(password);
                adminquery.Pass = hex;
                db.SubmitChanges();
                return Json(adminquery);
            }
            catch
            {
                EntityAdmin admin = new EntityAdmin();
                admin.Pass = "!"; // error
                return Json(admin);
            }


        }

    }
}
