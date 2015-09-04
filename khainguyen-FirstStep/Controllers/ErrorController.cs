using khainguyen_FirstStep.Models;
using System;
using System.Web.Mvc;

namespace khainguyen_FirstStep.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index(int statusCode, Exception exception, bool isAjaxRequet)
        {
            try
            {
                Response.StatusCode = statusCode;

                // If it's not an AJAX request that triggered this action then just retun the view
                if (!isAjaxRequet)
                {
                    ErrorModel model = new ErrorModel { HttpStatusCode = statusCode, Exception = exception };

                    return View(model);
                }
                else
                {
                    // Otherwise, if it was an AJAX request, return an anon type with the message from the exception
                    var errorObjet = new { message = exception.Message };
                    return Json(errorObjet, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                throw (new Exception());
            }
        }

    }
}
