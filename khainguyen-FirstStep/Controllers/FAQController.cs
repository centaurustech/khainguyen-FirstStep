using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace khainguyen_FirstStep.Controllers
{
    public class FAQController : Controller
    {
        //
        // GET: /FAQ/
        private dbFirstStepDataContext db = new dbFirstStepDataContext();
        public ActionResult IndexFAQ1(string name)
        {
            var item = db.EntityGroupFAQs.Where(p => p.IdLoaiFAQ == 1).OrderBy(p => p.ViTri).ToList();
            if (string.IsNullOrEmpty(name))
                ViewBag.idgroup = item.FirstOrDefault().Id;
            else
                ViewBag.idgroup = item.Single(p => Utilities.Encode(p.TenGroup) == name).Id;
            
            return View(item);
        }
        public ActionResult _IndexFAQ1_NoiDung(int Id)
        {
            var item = db.EntityFAQ1s.Where(p => p.IdGroupFAQ == Id).OrderBy(p => p.ViTri).ToList();
            return PartialView(item);
        }
        public ActionResult IndexFAQ2()
        {
            var item = db.EntityFAQs.Where(p => p.IdRoot == 2).OrderBy(p => p.ViTri).ToList();
            return View(item);
        }
        public ActionResult _IndexFAQ2_NoiDung(int Id)
        {
            var item = db.EntityFAQs.Where(p => p.IdRoot == Id).OrderBy(p => p.ViTri).ToList();
            return PartialView(item);
        }
        public ActionResult IndexFAQ3(string name)
        {
            if (string.IsNullOrEmpty(name)) { name = "Câu hỏi thường gặp"; }
            var item = db.EntityGroupFAQs.Where(p => p.TenGroup == name).FirstOrDefault();
            var list = db.EntityGroupFAQs.Where(p => p.IdGroupFAQ == item.Id).OrderBy(p => p.ViTri).ToList();
            ViewBag.Title = item.TenGroup;
            return View(list);
        }
        public ActionResult _IndexFAQ3_NoiDung(int Id)
        {
            var item = db.EntityFAQ1s.Where(p => p.IdGroupFAQ == Id).OrderBy(p => p.ViTri).ToList();
            return PartialView(item);
        }
        public ActionResult _IndexFAQ3_GroupClick(int id)
        {
            var item = db.EntityGroupFAQs.Where(p => p.IdGroupFAQ == id).OrderBy(p => p.ViTri).ToList();
            return PartialView(item);
        }
        public ActionResult _IndexFAQ3_TieuDeClick(int id)
        {
            var item = db.EntityFAQ1s.Where(p => p.IdGroupFAQ == id).OrderBy(p => p.ViTri).ToList();
            return PartialView(item);
        }
        public ActionResult _IndexFAQ3_NoiDungClick(int id)
        {
            if (id != 0)
            {
                var item = db.EntityFAQ1s.Where(p => p.Id == id).First();
                return PartialView(item);
            }
            else
            {
                return PartialView();
            }
        }
        public Boolean _IndexFAQ3_PhanHoi(int Id, int result)
        {
            try
            {
                EntityFeedbackFAQ feedback = new  EntityFeedbackFAQ();
                feedback.IdCauHoi = Id;
                feedback.Feedback = result;
                feedback.Time = DateTime.Now;
                db.EntityFeedbackFAQs.InsertOnSubmit(feedback);
                db.SubmitChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public ActionResult IndexFAQ4(string name)
        {
            //var item = db.EntityGroupFAQs.Where(p => p.TenGroup == name).FirstOrDefault();
            //var list = db.EntityGroupFAQs.Where(p => p.IdGroupFAQ == item.Id).ToList();
            //return View(list);
            
            //var item = db.EntityGroupFAQs.Where(p => Utilities.Encode(p.TenGroup) == name).FirstOrDefault();
            if(string.IsNullOrEmpty(name)){ name = "Sổ tay Firststep";}
            var item = db.EntityGroupFAQs.Where(p => p.TenGroup == name).FirstOrDefault();
            var list = db.EntityFAQ1s.Where(p => p.IdGroupFAQ == item.Id).OrderBy(p => p.ViTri).ToList();
            ViewBag.Title = name;
            return View(list);
        }

        public ActionResult _IndexFAQ4_TieuDePhai(int id)
        {
            var item = db.EntityGroupFAQs.Where(p => p.IdGroupFAQ == id).OrderBy(p => p.ViTri).ToList();
            return PartialView(item);
        }

        public ActionResult _IndexFAQ4_TieuDeTrai(int id)
        {
            var item = db.EntityGroupFAQs.Where(p => p.IdGroupFAQ == id).OrderBy(p => p.ViTri).ToList();
            return PartialView(item);
        }

        public ActionResult _IndexFAQ4_NoiDungTrai(int id)
        {
            var item = db.EntityFAQ1s.Where(p => p.IdGroupFAQ == id).OrderBy(p => p.ViTri).ToList();
            return PartialView(item);
        }

    }
}
