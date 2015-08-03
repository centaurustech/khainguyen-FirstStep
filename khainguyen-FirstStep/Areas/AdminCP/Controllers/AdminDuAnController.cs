using khainguyen_FirstStep.Areas.AdminCP.Models;
using MvcLibrary.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace khainguyen_FirstStep.Areas.AdminCP.Controllers
{
    public class AdminDuAnController : Controller
    {
        //
        // GET: /AdminCP/AdminDuAn/

        public ActionResult Index()
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var ban = db.EntityDuAns.ToList();
            return View(ban);
        }
        public ActionResult Block(int Id)
        {
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Block(AdminDuAnModel DA)
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var item = db.EntityDuAns.Where(p => p.Id == DA.Id).First();
            item.TrangThai = 0;
            item.LyDoBlock = DA.LyDoBlock;
            db.SubmitChanges();
            return RedirectToAction("Index", "AdminDuAn");
        }
        public ActionResult Open(int id)
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var item = db.EntityDuAns.Where(p => p.Id == id).First();
            item.TrangThai = 2;
            item.ThoiGianBatDau = DateTime.Now;
            item.LyDoBlock = null;
            db.SubmitChanges();
            return RedirectToAction("Index", "AdminDuAn");
        }
        public bool Email_DuAnKetThuc(int id)
        {
            try
            {
                dbFirstStepDataContext db = new dbFirstStepDataContext();
                var duan = db.EntityDuAns.Where(p => p.Id == id).First();
                var dautu = db.EntityDauTus.Where(p => p.IdDuAn == id);

                //Gui mail cho chu du an
                StringBuilder mailBody = new StringBuilder();
                mailBody.AppendFormat("<h1>[FirstStep]-Thông báo dự án đã kết thúc</h1>");
                mailBody.AppendFormat("<br />");
                mailBody.AppendFormat("<p>Chào : " + " " + duan.EntityUser.HoTen + "," + "</p>");
                mailBody.AppendFormat("<p>Dự án " + " " + duan.TenDuAn + " của bạn đã kết thúc  thời gian huy động. Tính đến thời điểm kết thúc, dự án chưa đạt được mục tiêu đề ra nhưng đây là dự án huy động vốn linh hoạt, bạn vẫn sẽ nhận được tất cả số tiền đã huy động được." + "</p>");
                mailBody.AppendFormat("<p>Hãy mau chóng hoàn thành ý tưởng của bạn và gửi những phần thưởng tương xứng tới người tài trợ theo đúng lịch trình đã đề ra.</p>");
                mailBody.AppendFormat("<p>Sử dụng chức năng Tin Nhắn của website để trao đổi thêm thông tin với những người tài trợ nếu cần thiết.</p>");
                mailBody.AppendFormat("<br></br>");
                mailBody.AppendFormat("<p>Đây là email hệ thống, vui lòng không gửi hồi đáp mail này.</p>");
                mailBody.AppendFormat("<p>Nếu bạn muốn thông tin cho FirstStep, hãy gửi mail tới địa chỉ: info@firststep.vn .</p>");
                MailHelper.SendMailMessage("firststep.system.info@gmail.com", duan.EntityUser.Email, null, null, "Thông báo dự án " + duan.TenDuAn + " đã kết thúc", mailBody.ToString(), "smtp.gmail.com", true, "firststep.system.info@gmail.com", "!@#Hien4567");

                //Gui mail cho nguoi dau tu
                foreach (var item in dautu)
                {
                    StringBuilder mailBody1 = new StringBuilder();
                    mailBody1.AppendFormat("<h1>[FirstStep]-Thông báo dự án đã kết thúc</h1>");
                    mailBody1.AppendFormat("<br />");
                    mailBody1.AppendFormat("<p>Chào : " + " " + item.EntityUser.HoTen + "," + "</p>");
                    mailBody1.AppendFormat("<p>Dự án " + " " + duan.TenDuAn + "đã kết thúc thời gian huy động. Tính đến thời điểm kết thúc, dự án chưa huy động đủ mục tiêu đề ra nhưng đây là dự án huy động vốn linh hoạt, số tiền tài trợ của bạn vẫn được chuyển cho chủ dự án." + "</p>");
                    mailBody1.AppendFormat("<p>Hãy chờ chủ dự án hoàn thành ý tưởng và gửi lại bạn phần thưởng của mình.</p>");
                    mailBody1.AppendFormat("<p>Sử dụng chức năng Tin Nhắn của website để trao đổi thêm thông tin với chủ dự án nếu cần thiết.</p>");
                    mailBody1.AppendFormat("<br></br>");
                    mailBody1.AppendFormat("<p>Đây là email hệ thống, vui lòng không gửi hồi đáp mail này.</p>");
                    mailBody1.AppendFormat("<p>Nếu bạn muốn thông tin cho FirstStep, hãy gửi mail tới địa chỉ: info@firststep.vn .</p>");
                    MailHelper.SendMailMessage("firststep.system.info@gmail.com", item.EntityUser.Email, null, null, "Thông báo dự án " + duan.TenDuAn + " đã kết thúc", mailBody1.ToString(), "smtp.gmail.com", true, "firststep.system.info@gmail.com", "!@#Hien4567");
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region "Manage_Index"
        public ActionResult Manager_Index()
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            EntityHomePage homepage = db.EntityHomePages.First();
            ViewBag.DanhMuc = db.EntityDanhMucs.Where(p => p.IdRoot == 1 || p.IdRoot == 2).ToList();
            return View(homepage);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Manager_Index(int IdModule1, int IdModule2, int IdModule3)
        {
            try
            {
                dbFirstStepDataContext db = new dbFirstStepDataContext();
                EntityHomePage homepage = db.EntityHomePages.First();
                homepage.IdModuleDanhMuc1 = IdModule1;
                homepage.IdModuleDanhMuc2 = IdModule2;
                homepage.IdModuleDanhMuc3 = IdModule3;
                db.SubmitChanges();
                ViewBag.DanhMuc = db.EntityDanhMucs.Where(p => p.IdRoot == 1 || p.IdRoot == 2).ToList();
                //AdminDanhMucModel.Edit(DM);
                //return RedirectToAction("Index", "AdminDanhMuc");
                return View(homepage);
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }
        #endregion //Edit

        //public ActionResult Manager_Index()
        //{
        //    dbFirstStepDataContext db = new dbFirstStepDataContext();
        //    var ban = db.EntityDanhMucs.Where(p => p.IdRoot == 1 || p.IdRoot == 2).ToList();
        //    return View(ban);
        //}
        //public string InsertManager_Index(string ketqua)
        //{
        //    try
        //    {
        //        string[] mang = ketqua.Split('*');
        //        List<AdminManagerIndexModel> ds = new List<AdminManagerIndexModel>();
        //        dbFirstStepDataContext db = new dbFirstStepDataContext();
        //        var Trend = db.EntityTrends.ToList();
        //        int vt = 0;
        //        int sl = 0;
        //        for (int i = 0; i < mang.Count() - 2; i += 2)
        //        {
        //            if (mang[i] != "")
        //            {
        //                Trend[vt].TrendName = mang[i];
        //                if (Trend[vt].IdCategory > 0)
        //                    Trend[vt].EntityDanhMuc.TenDM = mang[i];
        //            }
        //            if (sl <= 2)
        //            {
        //                Trend[vt].TrangThai = Convert.ToBoolean(int.Parse(mang[i + 1]));
        //                if (Trend[vt].TrangThai == true)
        //                    sl++;
        //            }
        //            else
        //            {
        //                Trend[vt].TrangThai = false;
        //            }
        //            vt++;
        //        }
        //        if (sl == 3)
        //            db.SubmitChanges();
        //        return "suc";
        //    }
        //    catch
        //    {
        //        return "Error";
        //    }
        //}

    }
}
