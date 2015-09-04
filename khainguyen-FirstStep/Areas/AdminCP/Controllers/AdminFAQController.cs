using khainguyen_FirstStep.Areas.AdminCP.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace khainguyen_FirstStep.Areas.AdminCP.Controllers
{
    public class AdminFAQController : Controller
    {
        //
        // GET: /AdminCP/AdminFAQ/
        dbFirstStepDataContext db = new dbFirstStepDataContext();
        public ActionResult Index()
        { 
            var item = db.EntityFAQs.Where(p => p.Loai == 0).OrderBy(p => p.Id).ToList();
            return View(item);
        }

        public ActionResult DanhSachLoaiFAQ(int Id)
        {
            var item = db.EntityGroupFAQs.Where(p => p.IdLoaiFAQ == Id);
            ViewBag.IdLoaiFAQ = Id;
            return View(item);
        }
        public ActionResult DanhSachTongHop(int IdGroupFAQ)
        {
            List<AdminFAQModel> tnew = new List<AdminFAQModel>();
            var item = db.EntityGroupFAQs.Where(p => p.IdGroupFAQ == IdGroupFAQ).ToList();
            foreach (var i in item)
            {
                tnew.Add(AdminFAQModel.LayModelGroup(i.Id));
            }
            var item1 = db.EntityFAQ1s.Where(p => p.IdGroupFAQ == IdGroupFAQ).ToList();
            foreach (var i in item1)
            {
                tnew.Add(AdminFAQModel.LayModelFAQ(i.Id));
            }
            var tnew1 = tnew.OrderBy(p => p.ViTri);
            var group = db.EntityGroupFAQs.Where(p => p.Id == IdGroupFAQ).FirstOrDefault();
            ViewBag.NameGroup = group.TenGroup;
            if (group.IdGroupFAQ != null)
            {                
                ViewBag.IdGroupFAQBack = group.IdGroupFAQ;                
            }
            else
            {
                ViewBag.IdGroupFAQ = group.Id;
                ViewBag.IdLoaiFAQ = group.IdLoaiFAQ;
            }
            ViewBag.IdGroupFAQ = IdGroupFAQ;
            return View(tnew1);
        }

        #region "insert"
        public ActionResult InsertGroupFAQ(int IdLoaiFAQ)
        {
            EntityGroupFAQ tnew = new EntityGroupFAQ();
            ViewBag.IdLoaiFAQ = IdLoaiFAQ;
            //dbFirstStepDataContext db = new dbFirstStepDataContext();
            return View(tnew);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult InsertGroupFAQ(EntityGroupFAQ DM)
        {
            try
            {
                // dbFirstStepDataContext db = new dbFirstStepDataContext();
                EntityGroupFAQ ban = new EntityGroupFAQ();
                ban.TenGroup = DM.TenGroup;
                ban.IdLoaiFAQ = DM.IdLoaiFAQ;
                ban.ViTri = DM.ViTri;
                db.EntityGroupFAQs.InsertOnSubmit(ban);
                db.SubmitChanges();
                return RedirectToAction("DanhSachLoaiFAQ", "AdminFAQ", new { Id = DM.IdLoaiFAQ });
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }
        public ActionResult InsertTongHop(int IdGroupFAQ)
        {
            AdminFAQModel tnew = new AdminFAQModel();
            ViewBag.IdGroupFAQ = IdGroupFAQ;
            tnew.Type = 1;
            //dbFirstStepDataContext db = new dbFirstStepDataContext();
            return View(tnew);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult InsertTongHop(AdminFAQModel DM)
        {
            try
            {
                // dbFirstStepDataContext db = new dbFirstStepDataContext();
                if (DM.Type == 1)
                {
                    EntityGroupFAQ ban = new EntityGroupFAQ();
                    ban.TenGroup = DM.TenGroup;
                    ban.ViTri = DM.ViTri;
                    ban.IdGroupFAQ = DM.IdGroupFAQ;
                    db.EntityGroupFAQs.InsertOnSubmit(ban);
                    db.SubmitChanges();
                }
                else
                {
                    EntityFAQ1 ban = new EntityFAQ1();
                    ban.CauHoi = DM.CauHoi;
                    ban.CauTraLoi = DM.CauTraLoi;
                    ban.ViTri = DM.ViTri;
                    ban.IdGroupFAQ = DM.IdGroupFAQ;
                    db.EntityFAQ1s.InsertOnSubmit(ban);
                    db.SubmitChanges();
                }                
                return RedirectToAction("DanhSachTongHop", "AdminFAQ", new { IdGroupFAQ = DM.IdGroupFAQ });
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }
        #endregion //insert

        #region "Edit"
        public ActionResult EditGroupFAQ(int IdGroupFAQ)
        {
            return View(db.EntityGroupFAQs.Where(p => p.Id == IdGroupFAQ).FirstOrDefault());
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditGroupFAQ(EntityGroupFAQ DM)
        {
            try
            {
                // AdminFAQModel.Edit(DM);
                var item = db.EntityGroupFAQs.Where(p => p.Id == DM.Id).First();
                item.TenGroup = DM.TenGroup;
                item.ViTri = DM.ViTri;
                item.IdGroupFAQ = DM.IdGroupFAQ;
                item.IdLoaiFAQ = DM.IdLoaiFAQ;
                db.SubmitChanges();
                if(item.IdGroupFAQ != null)
                    return RedirectToAction("DanhSachTongHop", "AdminFAQ", new { IdGroupFAQ = item.IdGroupFAQ });
                else
                    return RedirectToAction("DanhSachLoaiFAQ", "AdminFAQ", new { Id = item.IdLoaiFAQ });
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }
        public ActionResult EditFAQ(int IdFAQ)
        {
            return View(db.EntityFAQ1s.Where(p => p.Id == IdFAQ).FirstOrDefault());
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditFAQ(EntityFAQ1 DM)
        {

            try
            {
                // AdminFAQModel.Edit(DM);
                var item = db.EntityFAQ1s.Where(p => p.Id == DM.Id).First();
                item.CauHoi = DM.CauHoi;
                item.CauTraLoi = DM.CauTraLoi;
                item.ViTri = DM.ViTri;
                item.IdGroupFAQ = DM.IdGroupFAQ;
                db.SubmitChanges();
                return RedirectToAction("DanhSachTongHop", "AdminFAQ", new { IdGroupFAQ = item.IdGroupFAQ });
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }
        #endregion //Edit

        #region "Delete"
        public string DeleteGroupFAQ(int IdGroupFAQ)
        {
            try
            {
                // EntityFAQ.Delete(Id);
                var item = db.EntityGroupFAQs.Where(p => p.Id == IdGroupFAQ).First();
                db.EntityGroupFAQs.DeleteOnSubmit(item);
                db.SubmitChanges();
                return item.TenGroup;
            }
            catch
            {
                return "error";
            }
        }

        public string DeleteFAQ(int IdFAQ)
        {
            try
            {
               // EntityFAQ.Delete(Id);
                var item = db.EntityFAQ1s.Where(p => p.Id == IdFAQ).First();
                db.EntityFAQ1s.DeleteOnSubmit(item);
                db.SubmitChanges();
                return item.CauHoi;
            }
            catch
            {
                return "error";
            }
        }
        #endregion //Delete        

    }
}
