using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace khainguyen_FirstStep.Areas.AdminCP.Controllers
{
    public class AdminChonDuAnController : Controller
    {
        //
        // GET: /AdminCP/AdminChonDuAn/
        dbFirstStepDataContext db = new dbFirstStepDataContext();
        public ActionResult Index()
        {
            var item = db.EntityDuAns.ToList();
            return View(item);
        }
        public ActionResult ChonDuAn(int Id, int stt, int loai)
        {
            var item = db.EntityDuAns.Where(p => p.Id == Id).FirstOrDefault();
            if (loai == 1)
            {
                if (stt == 1)
                {
                    item.Chon1 = 0;
                }
                else
                {
                    item.Chon1 =1;
                }
            }
            else if (loai == 2)
            {
                if (stt == 1)
                {
                    item.Chon2 = 0;
                }
                else
                {
                    item.Chon2 = 1;
                }
            }
            else
            {
                if (stt == 1)
                {
                    item.Chon3 = 0;
                }
                else
                {
                    item.Chon3 = 1;
                }
            }
            db.SubmitChanges();
            return RedirectToAction("Index", "AdminChonDuAn");
        }

    }
}
