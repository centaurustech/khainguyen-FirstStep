using khainguyen_FirstStep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace khainguyen_FirstStep.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        dbFirstStepDataContext db = new dbFirstStepDataContext();
        public ActionResult Index()
        {
            string[] mang = Request.Url.AbsoluteUri.ToString().Split('/');
            string url = mang[0] + "//" + mang[2];
            return View();
        }
        public ActionResult CheckLife()
        {
            if (Request.Cookies["ftid"] != null)
            {
                if (Session["fsduytrihoatdong"] == null)
                {
                    ViewBag.Life = Request.Url.AbsoluteUri.ToString();
                    return PartialView();
                }
            }
            ViewBag.Life = "";
           return PartialView();
        }
        public ActionResult Nhappass()
        {
            if (Request.Cookies["ftid"] != null)
            {
                if (Session["fstaoduan"] == null)
                {
                    ViewBag.Life = Request.Url.AbsoluteUri.ToString();
                    return PartialView();
                }
            }
            ViewBag.Life = "";
            return PartialView();
        }
        public ActionResult _Logined()
        {
            if (Request.Cookies["ftid"] != null)
            {
                if (Session["fsnhatkyhoatdong"] == null || Session["fstinnhan"] == null)
                {
                    Session["fsnhatkyhoatdong"] = "0";
                    
                    int Id = Convert.ToInt16(Request.Cookies["ftid"].Value);
                    EntityUser user = db.EntityUsers.Where(g => g.Id == Id).FirstOrDefault();
                    if (user != null)
                    {
                        Session["fsnhatkyhoatdong"] = user.SoLuongHoatDong.ToString();
                    }
                    IEnumerable<getSQL_Message> newli = null;
                    string sql = " (select CASE WHEN idusergui=" + Id + " THEN idusernhan ELSE idusergui END iduser, MAX(date) datesend ,IdDuAn  from firststep.EntityTinNhan ";
                    sql += " WHERE (IDusernhan = " + Id + ")and TrangThai=0";
                    sql += " Group by CASE WHEN idusergui=" + Id + " THEN idusernhan ELSE idusergui END,IdDuAn)";
                    sql += " ORDER BY datesend desc";
                    newli = db.ExecuteQuery<getSQL_Message>(sql);
                    Session["fstinnhan"] = newli.Count().ToString();
                }
                else
                {
                    
                }
                
            }
        
            return PartialView();
        }

        #region trang chu
        public ActionResult _TrangChu_NoiBat()
        {      
            var item = db.EntityDanhMucs.Where(p => p.IdRoot == 1 && db.EntityDuAns.Where(g=>g.DuAnDuocChon== 1&& g.IdDanhMuc==p.Id).Any()==true ).ToList();
            return PartialView(item);
        }
        public ActionResult _TrangChu_NoiBat_DuAn()
        { 
            var item = db.EntityDuAns.Where(p => p.DuAnDuocChon == 1 ).OrderBy(p=> p.IdDanhMuc).ToList();
            return PartialView(item);
        }
        public ActionResult _TrangChu_Slide()
        {
            var db = new dbFirstStepDataContext();
            var item = db.EntityBanners.ToList();
           
            return PartialView(item);
        }
        public ActionResult _TrangChu_YKien()
        {
       
            var item = db.EntityQuotes.ToList();
            return PartialView(item);
        }

        public ActionResult _TrangChu_Trend_Category()
        {
            ////var blend = db.EntityTrends.Where(g=>g.TrangThai== true).ToList();
            var cate = db.EntityHomePages.First();
            return PartialView(cate);
        }
        public ActionResult _TrangChu_Item_Trend_Category(EntityDanhMuc idtrend, int hinhnen, string loai, int catnumber)
        {
            ViewBag.loai = loai;
            ViewBag.hinhnen = hinhnen;
            ViewBag.catnumber = catnumber;
            if (idtrend.IdRoot==1)
            {
                ViewBag.Ten ="Dự án của "+ idtrend.TenDM.ToString();
                ViewBag.LinkViewMore = "/du-an?option=-category=" + idtrend.Id+ "-blend=0-";
                return PartialView(DuAnModel.GetDuAn_TrangChu(1, idtrend.Id));
            }
            else
            {
                
                ViewBag.LinkViewMore = "/du-an?option=-category=0-blend="+ idtrend.Id + "-";
                ViewBag.Ten = idtrend.TenDM;
                return PartialView(DuAnModel.GetDuAn_TrangChu(2, idtrend.Id));
            }
        }

        #endregion

        //#region ----------------------- phan nay sau nay bỏ

        //public ActionResult _TrangChu_NoiBat_Chon1()
        //{
       
        //    var item = db.EntityDuAns.Where(p => p.Chon1 == 1).OrderBy(p => p.IdDanhMuc).ToList();
        //    return PartialView(item);
        //}
        //public ActionResult _TrangChu_NoiBat_Chon2()
        //{
   
        //    var item = db.EntityDuAns.Where(p => p.Chon2 == 1).OrderBy(p => p.IdDanhMuc).ToList();
        //    return PartialView(item);
        //}
        //public ActionResult _TrangChu_NoiBat_Chon3()
        //{
        
        //    var item = db.EntityDuAns.Where(p => p.Chon3 == 1).OrderBy(p => p.IdDanhMuc).ToList();
        //    return PartialView(item);
        //}

        //#endregion

        #region Footer
        public ActionResult _Footer_FileDownload()
        {
            return View();
        }
        public ActionResult _Footer_TimhieuFirststep()
        {
            return View();
        }
        public ActionResult _Footer_ThongtinAlpe()
        {
            return View();
        }
        public ActionResult _Footer_KhamPha()
        {

            var item = db.EntityDanhMucs.Where(p => p.IdRoot == 1).OrderBy(p => p.Id).ToList();
            return PartialView(item);
        }
        public ActionResult _TaiKhoan_DuAn()
        {

            var item = db.EntityDuAns.Where(p => p.IdUser == Convert.ToInt16(Request.Cookies["ftid"].Value)).OrderBy(p => p.Id).ToList();
            return PartialView(item.Take(11));
        }
        #endregion 
    }
}
