using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace khainguyen_FirstStep.Controllers
{
    public class userController : Controller
    {
        //
        // GET: /user/
        dbFirstStepDataContext db = new dbFirstStepDataContext();

        public ActionResult Index(string Link)
        {
            var item = db.EntityUsers.Where(p => p.VanityURL == Link).FirstOrDefault();
            if (item==null)
            {
                item = db.EntityUsers.Where(p => p.Id == Convert.ToInt16(Request.Cookies["ftid"].Value)).FirstOrDefault();
            }
           
            if (item != null)
            {
                int iduser = item.Id;
                int idlogin = 0;
                if (Request.Cookies["ftid"] != null)
                    idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
                ViewBag.idlogin = idlogin;

                if (iduser == idlogin)
                {
                    ViewBag.idlogin = 0;// k được gởi
                    ViewBag.DuAn = db.EntityDuAns.Where(p => p.IdUser == iduser && (p.TrangThai == 0|| p.TrangThai==2)).ToList();
                }
                else
                {
                   
                    ViewBag.DuAn = db.EntityDuAns.Where(p => p.IdUser == iduser && p.TrangThai == 2).ToList();
                }
               var query  = (from op in db.EntityDuAns
                                 join pg in db.EntityDauTus on op.Id equals pg.IdDuAn
                                 where  op.TrangThai==2
                                 select  op).Distinct().ToList();
              
               ViewBag.DauTu = query;
               ViewBag.BinhLuan = db.EntityBinhLuans.Where(g => g.IdUser == iduser && g.Public == 0).ToList();// cho hiển thị
            }
            ViewBag.IdFriend = item.Id;
            return View(item);
        }

        public ActionResult _LoadBinhLuan(IEnumerable<EntityBinhLuan> binhluan)
        {
                return PartialView(binhluan);
        }

        public ActionResult _LoadSendTinNhan(int IdUser)
        {
            ViewBag.IdUser = IdUser;

            IEnumerable<EntityDuAn> duan = db.EntityDuAns.Where(p => p.IdUser == IdUser && p.TrangThai == 1).ToList();
            
            return PartialView(duan);
        }
       
        

    }
}
