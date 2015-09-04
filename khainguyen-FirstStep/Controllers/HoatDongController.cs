using khainguyen_FirstStep.Models;
using MvcLibrary.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace khainguyen_FirstStep.Controllers
{
    public class HoatDongController : Controller
    {
        //
        // GET: /HoatDong/
        #region "tin nhắn bc làm"
        private dbFirstStepDataContext db = new dbFirstStepDataContext();
        public ActionResult TinNhan(int type)
        {
         //   string kkk = Request.Url.AbsoluteUri;
            //ViewBag.type = type;
            int Id = Convert.ToInt16(Request.Cookies["ftid"].Value);
            IEnumerable<getSQL_Message> newli = null;
            IList<SendMessage> kq = new List<SendMessage>();
            string sql = "";
            #region "truy vấn"
            // tat ca tin nhan
            if (type == 0)
            {

                sql += " (select CASE WHEN idusergui=" + Id + " THEN idusernhan ELSE idusergui END iduser, MAX(date) datesend ,IdDuAn  from firststep.EntityTinNhan ";
                sql += " WHERE IdUserGui = " + Id + " OR Idusernhan = " + Id;
                sql += " Group by CASE WHEN idusergui=" + Id + " THEN idusernhan ELSE idusergui END,IdDuAn)";
                sql += " ORDER BY datesend desc";
            }
            // hop thu den
            else if (type == 1)
            {
               
                sql += " (select CASE WHEN idusergui=" + Id + " THEN idusernhan ELSE idusergui END iduser, MAX(date) datesend ,IdDuAn  from firststep.EntityTinNhan ";
                sql += " WHERE  IDusernhan = " + Id;
                sql += " Group by CASE WHEN idusergui=" + Id + " THEN idusernhan ELSE idusergui END,IdDuAn)";
                sql += " ORDER BY datesend desc";
            
            }
            // hop thu gui
            else if (type == 2)
            {
                sql += " (select CASE WHEN idusergui=" + Id + " THEN idusernhan ELSE idusergui END iduser, MAX(date) datesend ,IdDuAn  from firststep.EntityTinNhan ";
                sql += " WHERE  IdUserGui = " + Id;
                sql += " Group by CASE WHEN idusergui=" + Id + " THEN idusernhan ELSE idusergui END,IdDuAn)";
                sql += " ORDER BY datesend desc";

            }
            // chua doc thu
            else
            {
                sql += " (select CASE WHEN idusergui=" + Id + " THEN idusernhan ELSE idusergui END iduser, MAX(date) datesend ,IdDuAn  from firststep.EntityTinNhan ";
                sql += " WHERE (IDusernhan = " + Id + ")and TrangThai=0";
                sql += " Group by CASE WHEN idusergui=" + Id + " THEN idusernhan ELSE idusergui END,IdDuAn)";
                sql += " ORDER BY datesend desc";

            }
            #endregion
            newli = db.ExecuteQuery<getSQL_Message>(sql);
            int sl = 0;
            foreach (var item in newli)
            {
                //Lay tin nhan??
                sl++;
                SendMessage send = new SendMessage();
                var danhtinnhan = db.EntityTinNhans.Where(g =>g.IdDuAn == item.IdDuAn && (g.IdUserNhan == item.iduser || g.IdUserGui == item.iduser) && (g.IdUserNhan == Id || g.IdUserGui == Id)).ToList();
                if (danhtinnhan.Any(g => g.TrangThai == 0 && g.IdUserNhan==Id) == true)
                {
                    send.TrangThai = 0;
                }
                send.user = db.EntityUsers.Where(g => g.Id == item.iduser).First();
                EntityDuAn duan = db.EntityDuAns.Where(g => g.Id == item.IdDuAn).FirstOrDefault();
                send.IdDuAn = item.IdDuAn;
                send.TenDuAn =duan==null?"không xác định": duan.TenDuAn;
                send.IdSendMessge = sl;

                //Lay tin nhan moi nhat
                EntityTinNhan tinhanmoi = danhtinnhan.OrderByDescending(g => g.Date).FirstOrDefault();
                if (tinhanmoi != null)
                {
                    send.ThoiGianNew = tinhanmoi.Date.Value;
                    string NoiDung = tinhanmoi.NoiDung;
                    if (NoiDung != null)
                        if (NoiDung.Length > 20) send.NoiDungRutGon = NoiDung.Substring(0, 20);
                        else send.NoiDungRutGon = NoiDung;
                    
                }
                string noidung_big = "";
                foreach (var item1 in danhtinnhan)
                {
                    noidung_big = item1.NoiDung + " "+ noidung_big;
                }
                send.NoiDung = noidung_big;
                send.SoTinNhan = danhtinnhan.Count();

                kq.Add(send);
            }
            ViewBag.type = type;
            return View(kq);
        }
        public string checkChuaDoc(int IdDuAn)
        {
            int Id = Convert.ToInt16(Request.Cookies["ftid"].Value);
            EntityTinNhan tinnhan = db.EntityTinNhans.Where(g=>g.IdUserNhan==Id && g.IdDuAn== IdDuAn).FirstOrDefault();
            if (tinnhan != null)
            {
                tinnhan.TrangThai = 0;
                db.SubmitChanges();
            }
            return "t";
        }
        public string CheckSpam(int idcheck)
        {
            int Id = Convert.ToInt16(Request.Cookies["ftid"].Value);
            EntityTinNhan tinnhan = db.EntityTinNhans.Where(g => g.Id==idcheck).FirstOrDefault();
            if (tinnhan != null)
            {
                tinnhan.TrangThai = -1;
                db.SubmitChanges();
            }
            return "t";
        }
        public ActionResult TinNhan_Read(int Id, int IdDuAn)
        {
                int IdCook = Convert.ToInt16(Request.Cookies["ftid"].Value);
                var item1 = db.EntityTinNhans.Where(p => (p.IdUserGui == Id || p.IdUserNhan == Id) && p.IdDuAn == IdDuAn && (p.IdUserGui == IdCook || p.IdUserNhan == IdCook)).OrderByDescending(p => p.Date).ToList();
                ViewBag.Idlogin = IdCook;
                foreach (var item in item1.Where(g=>g.IdUserGui!= IdCook))
                {
                    if (item.TrangThai == 0)
                    {
                        item.TrangThai = 1;
                    }
                }
                db.SubmitChanges();
                return PartialView(item1);
        }

        public ActionResult TinNhan_Add(int Id,string noidung,int IdDuAn)
        {
            int IdCook = Convert.ToInt16(Request.Cookies["ftid"].Value);
            EntityTinNhan tn = new EntityTinNhan();
            tn.NoiDung = noidung;
            tn.IdUserGui = IdCook;
            tn.IdUserNhan = Id;
            tn.TrangThai = 0;
            tn.Date = DateTime.Now;
            tn.IdDuAn = IdDuAn;
            db.EntityTinNhans.InsertOnSubmit(tn);
            db.SubmitChanges();
            return PartialView(tn);
        }

      
        #endregion

        #region "lược sử dự án"
        public ActionResult LichSu()
        {
            int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
            var dautu = db.EntityDauTus.Where(g=>g.IdUser == idlogin).ToList();
            return View(dautu);
        }
        public ActionResult _Message_LichSu(int IdDauTu)
        {
            int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
            var dautu = db.EntityDauTus.Where(g => g.Id == IdDauTu).FirstOrDefault();
           var tinnhan = db.EntityTinNhans
                .Where(g=>g.IdDuAn== dautu.IdDuAn)
                .Where(g => (g.IdUserGui == dautu.EntityDuAn.IdUser && g.IdUserNhan == dautu.IdUser) || (g.IdUserNhan == dautu.EntityDuAn.IdUser && g.IdUserGui == dautu.IdUser))
                .Distinct().ToList();
            LichSuModel lichsu = new LichSuModel();
            lichsu.DauTuModel = dautu;
            lichsu.TinNhanModel = tinnhan;
            lichsu.CapNhatModel = db.EntityCapNhatDuAns.Where(g=>g.IdDuAn== dautu.IdDuAn).ToList();
            if (idlogin == dautu.IdUser)
            {
                ViewBag.ghichu = dautu.Note;
                ViewBag.UserNhan = dautu.EntityDuAn.IdUser;
            }
            else { ViewBag.UserNhan = dautu.EntityDuAn.IdUser; ViewBag.ghichu = dautu.NoteOwner; }
            return PartialView(lichsu);
            
        }

        public string Note_Edit(string NoiDung,int IdDauTu)
        {
            string kq = "Error";
            int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
            EntityDauTu dautu = db.EntityDauTus.Where(g => g.Id == IdDauTu).FirstOrDefault();
            if (dautu != null)
            {
                if (idlogin == dautu.IdUser)
                {
                    dautu.Note = NoiDung;
                }
                else
                {
                    dautu.NoteOwner = NoiDung;
                }
                db.SubmitChanges();
            }
            kq = NoiDung;
            return kq;
        }

        public string TrangThai_Edit(int IdDauTu)
        {
            string kq = "Error";
            EntityDauTu dautu = db.EntityDauTus.Where(g => g.Id == IdDauTu).FirstOrDefault();
            if (dautu != null)
            {
                dautu.TrangThai++;
                db.SubmitChanges();
                kq = "Successful";
                if (dautu.TrangThai == 1)
                {
                    GuiMailXacNhanDaNhanTien(dautu.EntityUser.HoTen, dautu.EntityUser.Email, dautu.EntityDuAn.TenDuAn, dautu.SoTienDauTu.ToString(), "/du-an/mv-hen-uoc-tay-ho-ky-duyen_77");
                }
                return kq;
            }
            return kq;
        }
        public void GuiMailXacNhanDaNhanTien(string HoTen, string Email, string TenDuAn, string sotien, string linkduan)
        {
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("<h1>[FirstStep]-Xác nhận đã nhận tiền</h1>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat("<p> Chào bạn: " + " " + HoTen + "   !  " + "</p>");
            mailBody.AppendFormat("<p>Firststep đã nhận được số tiền bạn tài trợ cho dự án :" + " " + TenDuAn + " với số tiền là " + sotien + " đồng. Bạn sẽ nhận được phần quà tương ứng với gói tài trợ " + sotien + " đồng.</p>");
            mailBody.AppendFormat("<p>Để dự án sớm thành công, bạn hãy chia sẻ thông tin trên các trang mạng xã hội, diễn đàn để thu hút cộng đồng chung tay cùng tài trợ cho dự án." + "</p>");
            mailBody.AppendFormat("<p>Để theo dõi những thông tin mới nhất về dự án, bạn vui lòng theo dõi mục cập nhật trong phần thông tin về dự án bằng cách nhấn vào link dưới đây." + "</p>");
            string[] mang = Request.Url.AbsoluteUri.ToString().Split('/');
            string url = mang[0] + "//" + mang[2];
            mailBody.AppendFormat("<p>" + url + linkduan + "</p>");
            mailBody.AppendFormat("<br></br>");
            mailBody.AppendFormat("<p>Đây là email hệ thống, vui lòng không gửi hồi đáp mail này.</p>");
            MailHelper.SendMailMessage("firststep.system.info@gmail.com", Email, null, null, "Xác Nhận Tài Trợ Dự Án - " + TenDuAn, mailBody.ToString(), "smtp.gmail.com", true, "firststep.system.info@gmail.com", "!@#Hien4567");
        }
        //
        #endregion

        public ActionResult _TinNhan_Read_list()
        {
            int Id = Convert.ToInt16(Request.Cookies["ftid"].Value);
            // tat ca tin nhan
            var item = db.EntityTinNhans.Where(p => p.IdUserGui == Id || p.IdUserNhan == Id).GroupBy(p => p.Id).Select(group => group.First()).OrderByDescending(p => p.Date).ToList();
            return PartialView(item);
        }
      
        public ActionResult ThongTin(string Link)
        {
            var item = db.EntityUsers.Where(p => p.VanityURL == Link).FirstOrDefault();
            if(item!= null)
            ViewBag.DuAn = db.EntityDuAns.Where(p => p.IdUser == Convert.ToInt16(Request.Cookies["ftid"].Value)).ToList();
            return View(item);
        }

        public ActionResult _MenuHoatDong()
        {
            int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
            EntityUser user = db.EntityUsers.Where(g=>g.Id == idlogin).First();
            ViewBag.Link = user.VanityURL;
            return PartialView();
        }

        #region hoạt động

        public ActionResult _HoatDong_Item(HoatDongModel hoatdong)
        {
           DetailHoatDongModel chitiethoatdong= HoatDongModel.Convert(hoatdong);
           return PartialView(chitiethoatdong);
        }
        public static List<HoatDongModel> tongdanhsach = new List<HoatDongModel>();
        public ActionResult HoatDong()
        {
            int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            EntityUser user = db.EntityUsers.Where(g => g.Id == idlogin).First();
            user.SoLuongHoatDong = 0;
            db.SubmitChanges();
            tongdanhsach = HoatDongModel.getListHoatDongModel(user);
            if (tongdanhsach.Count() == 0)
            {
                ViewBag.Tin = 0;
            }
            return View();
        }
        int pagesize = 3;
        public ActionResult _HoatDong(int page)
        {
            int currentPageIndex = page;

            if (page != 0)
                currentPageIndex = currentPageIndex * pagesize;
            ViewBag.page = page;

            return PartialView(tongdanhsach.Skip(currentPageIndex).Take(pagesize).ToList());
        }

        #region "_________________________________bo phan nay"
        //public ActionResult _HoatDong_HomNay(IEnumerable<EntityHoatDong> hoatdong)
        //{
        //    var db = new dbFirstStepDataContext();
           
        //    var item = hoatdong
        //        .Where(g => (g.Date.Day == DateTime.Now.Day)
        //            && g.Date.Month == DateTime.Now.Month
        //              && g.Date.Year == DateTime.Now.Year
        //            ).ToList();
        //    return PartialView(item);
        //}
        //public ActionResult _HoatDong_TuanTruoc(IEnumerable<EntityHoatDong> hoatdong)
        //{
        //    var db = new dbFirstStepDataContext();
        //    var item1 = hoatdong.Where(g => (DateTime.Now - g.Date).TotalDays > 1 && (DateTime.Now - g.Date).TotalDays<8);
        //    return PartialView(item1);
        //}
        //public ActionResult _HoatDong_ThangTruoc(IEnumerable<EntityHoatDong> hoatdong)
        //{
        //    var db = new dbFirstStepDataContext();
        //    var item1 = hoatdong.Where(g => (DateTime.Now - g.Date).TotalDays >=8 && (DateTime.Now - g.Date).TotalDays < 30);
        //    return PartialView(item1);
        //}
        //public ActionResult _HoatDong_NamTruoc(IEnumerable<EntityHoatDong> hoatdong)
        //{
        //    var db = new dbFirstStepDataContext();
        //    var item1 = hoatdong.Where(g => (DateTime.Now - g.Date).TotalDays >= 30);
        //    return PartialView(item1);
        //}
        #endregion
        #endregion
    }
}
