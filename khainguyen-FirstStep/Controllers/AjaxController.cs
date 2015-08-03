using MvcLibrary.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace khainguyen_FirstStep.Controllers
{
    public class AjaxController : Controller
    {
        //
        // GET: /Ajax/
        dbFirstStepDataContext db = new dbFirstStepDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search_Main()
        {
            return View();
        }
        #region "Cập nhật dự án - hỗ trợ controller DuAn"
        public string CheckTrangThaiDuAn(int IdDuAn, int TrangThai)
        {
            if (TrangThai == 0 || TrangThai == -1)
            {
                EntityDuAn duan = db.EntityDuAns.Where(g => g.Id == IdDuAn).FirstOrDefault();
                if (duan != null)
                {
                    duan.TrangThai = TrangThai;
                    db.SubmitChanges();
                }
            }
            return "s";
        }
        #endregion

        #region "Hỗ trợ usercontroller"
        public string _SendTinNhan(string NoiDung, int IdUser, int IdDuAn)//người nhận
        {
            string ketqua = "Error";
            try
            {
                //_ChiTietDuAn_GuiBinhLuan
                EntityTinNhan tinnhan = new EntityTinNhan();
                tinnhan.NoiDung = NoiDung;
                tinnhan.IdUserNhan = IdUser;
                tinnhan.IdUserGui = Convert.ToInt16(Request.Cookies["ftid"].Value);
                tinnhan.NoiDung = NoiDung;
                tinnhan.Date = DateTime.Now;
                tinnhan.IdDuAn = IdDuAn;
                tinnhan.TrangThai = 0;// chưa đọc
                db.EntityTinNhans.InsertOnSubmit(tinnhan);
                db.SubmitChanges();
                ketqua = "Ok";
            }
            catch { }

            return ketqua;
        }

        #endregion

        #region "disconnect facebook - xóa tài khoản"
        public string disconnect_facebook(string pass)
        {
            int idlogin = 0;
            if (Request.Cookies["ftid"] != null)
                idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
            EntityUser user = db.EntityUsers.Where(g => g.Id == idlogin).FirstOrDefault();
            Security ser = new Security();
            string Passhex = ser.GetHashPassword(pass);
            if (user.Pass == Passhex && pass.Length > 5)
            {
                user.IdFacebook = null;
                db.SubmitChanges();
                return "Hủy kết nối với facebook thành công";
            }
            else
            {
                return "Mật khẩu không đúng xin vui lòng nhập lại";
            }
        }

        public string delete_account(string pass)
        {
            int idlogin = 0;
            if (Request.Cookies["ftid"] != null)
                idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
            EntityUser user = db.EntityUsers.Where(g => g.Id == idlogin).FirstOrDefault();
            Security ser = new Security();
            string Passhex = ser.GetHashPassword(pass);
            if (user.Pass == Passhex && pass.Length > 5)
            {
                user.TrangThai = 3; //tài khoản bị hủy
                db.SubmitChanges();
                return "Xóa tài khoản thành công";
            }
            else
            {
                return "Xin vui lòng kiểm tra lại mật khẩu";
            }
        }
        #endregion

        #region "Thêm thành viên - hỗ trợ controller DuAn"
        public void GuiMailXacNhan_ThemThanhVien(EntityUser NguoiGui, EntityUser NguoiNhan,EntityDuAn duan, string HasCode)
        {
            string[] mang = Request.Url.AbsoluteUri.ToString().Split('/');
            string url = mang[0] + "//" + mang[2];
            string linkduan = url+"/" + Url.Action("ChiTietDuAn", "DuAn", new { Title = Utilities.Paste_Int64(Utilities.Encode(duan.TenDuAn.ToString()), duan.Id) });
            string linkkichhoat = url + "/" + Url.Action("kichhoat_team", "Account", new { HasCode = HasCode });
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("<h1>[FirstStep]-Xác nhận thêm thành viên</h1>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat("<p> Chào : " + " " + NguoiNhan.HoTen + "   !  " + "</p>");
            mailBody.AppendFormat("<p>Một thành viên của <a href=\"http://firststep.vn\">FirstStep</a> đã thêm bạn vào team của dự án : " + " <a href=\"" + linkduan + "\">" + duan.TenDuAn + "</a> " + "</p>");

            mailBody.AppendFormat("<p>Bạn vui lòng nhấp vào link này để xác nhận, nếu bạn không đồng ý xin đừng làm gì:  " + " <a href=\"" + linkkichhoat + "\">" + linkkichhoat + "</a> " + "</p>");
            mailBody.AppendFormat("<br></br>");
            mailBody.AppendFormat("<p>Không trả lời thư vào email này ! Xin cảm ơn.</p>");
            MailHelper.SendMailMessage("firststep.system.info@gmail.com", NguoiNhan.Email, null, null, "[FirstStep]-Xác nhận thêm thành viên", mailBody.ToString(), "smtp.gmail.com", true, "firststep.system.info@gmail.com", "!@#Hien4567");
        }
        public ActionResult _Team_DuAn(int IdDuAn)
        {
            ViewBag.IdDuAn = IdDuAn;
            var query = db.EntityNhomChienDiches.Where(g=>g.IdDuAn== IdDuAn).ToList();
            return PartialView(query);
        }
         public string _Team_ThemThanhVien_DuAn(int IdDuAn,string Email)
        {
            try
            {
                EntityUser user = db.EntityUsers.Where(g => g.Email == Email).FirstOrDefault();
                if (user == null)
                {
                    return "error";
                }
                if (db.EntityNhomChienDiches.Any(g => g.IdUser == user.Id) == true)
                    return "error";

                EntityNhomChienDich nhom = new EntityNhomChienDich();
                Security ser = new Security();
                string HasCode = ser.GetHashPassword(user.Email);
                nhom.IdDuAn = IdDuAn; nhom.IdUser = user.Id;
                nhom.TrangThai = false;
                nhom.IdUser = user.Id;
                user.HasCode = HasCode;
                db.SubmitChanges();
                db.EntityNhomChienDiches.InsertOnSubmit(nhom); db.SubmitChanges();
                GuiMailXacNhan_ThemThanhVien(user,nhom.EntityUser,nhom.EntityDuAn, HasCode);

                return nhom.Id.ToString();
            }
            catch { return "error"; }
        }
        public ActionResult _Team_VaiTro_DuAn(int IdNhomChienDich)
        {
            EntityNhomChienDich nhom = db.EntityNhomChienDiches.Where(g=>g.Id== IdNhomChienDich).FirstOrDefault();
            return PartialView(nhom);
        }
        public string _Team_ThemVaiTro_DuAn(int IdNhomChienDich,string VaiTro)
        {
            try
            {
                EntityNhomChienDich nhom = db.EntityNhomChienDiches.Where(g => g.Id == IdNhomChienDich).FirstOrDefault();
                nhom.VaiTro = VaiTro;
                db.SubmitChanges();
                return "complete";
            }
            catch { return "error"; }
        }

        public string _Team_XoaThanhVien_DuAn(int IdNhomChienDich)
        {
            try
            {
                EntityNhomChienDich nhom = db.EntityNhomChienDiches.Where(g => g.Id == IdNhomChienDich).FirstOrDefault();
                db.EntityNhomChienDiches.DeleteOnSubmit(nhom);
                db.SubmitChanges();
                return "complete";
            }
            catch { return "error"; }
        }

        #endregion

        // sửa thông tin cá nhân// link profile
        public string checklink(string link)
        {
            try
            {
                string[] mang = link.Split('/');
                string kt = mang[mang.Count() - 1].ToString().Replace("user-","");
                dbFirstStepDataContext db = new dbFirstStepDataContext();
               int  idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
                if (db.EntityUsers.Any(g=>g.VanityURL== kt)== false)
                {
                    return "t";
                }
                else return "f";
            }
            catch { return "f"; }
        }
    }
}
