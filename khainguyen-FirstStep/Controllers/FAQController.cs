using System;
using System.Linq;
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
            {
                string title = item.FirstOrDefault().TenGroup;
                ViewBag.Title = title + " » Nội dung hỗ trợ - Firststep";
                ViewBag.idgrouptitle = Utilities.Encode(title); 

            }
            else
            {
                string title = item.Single(p => Utilities.Encode(p.TenGroup) == name).TenGroup;
                ViewBag.Title = title + " » Nội dung hỗ trợ - Firststep";
                ViewBag.idgrouptitle = Utilities.Encode(title);
            }
            return View(item);
        }
        public ActionResult _IndexFAQ1_NoiDung(string idgrouptitle)
        {
            int idgroup;
            var item = db.EntityGroupFAQs.Where(p => p.IdLoaiFAQ == 1).OrderBy(p => p.ViTri).ToList();
            try
            {
                if (string.IsNullOrEmpty(idgrouptitle))
                    idgroup = item.FirstOrDefault().Id;
                else
                    idgroup = item.Single(p => Utilities.Encode(p.TenGroup) == idgrouptitle).Id;
            }
            catch
            {
                return HttpNotFound();
            }          

            var item_ask = db.EntityFAQ1s.Where(p => p.IdGroupFAQ == idgroup).OrderBy(p => p.ViTri).ToList();
            return PartialView(item_ask);
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
        public ActionResult _IndexFAQ3_GroupClick(string hashtag)
        {
            int idgroup;
            var xitem = db.EntityGroupFAQs.OrderBy(p => p.ViTri).ToList();
            try
            {
                if (string.IsNullOrEmpty(hashtag))
                    idgroup = xitem.FirstOrDefault().Id;
                else
                    idgroup = xitem.Single(p => Utilities.Encode(p.TenGroup) == hashtag).Id;
            }
            catch
            {
                return HttpNotFound();
            }
            var item = db.EntityGroupFAQs.Where(p => p.IdGroupFAQ == idgroup).OrderBy(p => p.ViTri).ToList();
            return PartialView(item);
        }
        public ActionResult _IndexFAQ3_TieuDeClick(string hashtag)
        {
            int idgroup;
            var xitem = db.EntityGroupFAQs.OrderBy(p => p.ViTri).ToList();
            try
            {
                if (string.IsNullOrEmpty(hashtag))
                    idgroup = xitem.FirstOrDefault().Id;
                else
                    idgroup = xitem.Single(p => Utilities.Encode(p.TenGroup) == hashtag).Id;
            }
            catch
            {
                return HttpNotFound();
            }
            var item = db.EntityFAQ1s.Where(p => p.IdGroupFAQ == idgroup).OrderBy(p => p.ViTri).ToList();
            return PartialView(item);
        }

        public ActionResult _IndexFAQ3_NoiDungClick(string hashtag)
        {
            var xitem = db.EntityFAQ1s.OrderBy(p => p.ViTri).ToList();
            EntityFAQ1 item;
            try
            {
                if (string.IsNullOrEmpty(hashtag))
                    item = xitem.FirstOrDefault();
                else
                    item = xitem.Single(p => Utilities.Encode(p.CauHoi) == hashtag);
            }
            catch
            {
                return HttpNotFound();
            }
            return PartialView(item);
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
