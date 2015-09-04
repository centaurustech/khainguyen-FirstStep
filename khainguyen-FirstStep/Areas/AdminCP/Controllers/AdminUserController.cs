using System.Linq;
using System.Web.Mvc;

namespace khainguyen_FirstStep.Areas.AdminCP.Controllers
{
    public class AdminUserController : Controller
    {
        //
        // GET: /AdminCP/AdminUser/

        public ActionResult Index()
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var ban = db.EntityUsers.ToList();
            return View(ban);
        }
        public ActionResult Ban(int id, int t)
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var item = db.EntityUsers.Where(p => p.Id == id).First();
            item.TrangThai = t;
            db.SubmitChanges();
            return RedirectToAction("Index", "AdminUser");
        }

    }
}
