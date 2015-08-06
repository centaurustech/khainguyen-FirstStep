using khainguyen_FirstStep.Models;
using MvcLibrary.Repository;
using Payoo.Lib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace khainguyen_FirstStep.Controllers
{
    public class ThanhToanController : Controller
    {
        //
        // GET: /ThanhToan/
        private dbFirstStepDataContext db = new dbFirstStepDataContext();
        public ActionResult ThanhToan_Lan1(int Id, int pt, int SoTien)
        {
            var item = db.EntityPhanThuongs.Where(p => p.IdDuAn == Id).ToList();
            if (item.Count() > 0)
            {
                EntityPhanThuong phanthuong = item.Where(g=>g.Id==pt).FirstOrDefault();
                ViewBag.phanthuong = phanthuong;
               // phanthuong.TienHoTro;
            }
            ViewBag.DuAn = db.EntityDuAns.Where(p => p.Id == Id).First();
            ViewBag.pt = "#"+pt;
            ViewBag.SoTien = SoTien;
            return View(item);
        }
        
        [HttpPost]
        public ActionResult ThanhToan_Lan2(int IdDuAn, int PhanThuong, int SoTien)
        {
            var item = db.EntityPhanThuongs.Where(p => p.IdDuAn == IdDuAn).ToList();
            if (item.Count() > 0)
            {
                EntityPhanThuong phanthuong = item.Where(g => g.Id == PhanThuong).FirstOrDefault();
                if (phanthuong != null)
                    ViewBag.phanthuong = phanthuong;
                else
                {
                    phanthuong = new EntityPhanThuong();
                    phanthuong.Id = 0;
                    ViewBag.phanthuong = phanthuong;
                }
                // phanthuong.TienHoTro;
            }
            ViewBag.DuAn = db.EntityDuAns.Where(p => p.Id == IdDuAn).First();
            ViewBag.SoTien = SoTien;
            ViewBag.pt = "#" + PhanThuong;
            return View(item);
            //try
            //{
            //    int ketqua = 0;
            //    int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
            //    if (PhanThuong > 0)
            //    {
            //        if (db.EntityDauTus.Any(g => g.IdDuAn == IdDuAn && g.IdUser == idlogin) == true)
            //            return View();

            //        EntityDauTu item = new EntityDauTu();
            //        item.IdDuAn = IdDuAn;
            //        item.IdPhanthuong = PhanThuong;
            //        item.SoTienDauTu = SoTien;
            //        item.IdUser = idlogin;
            //        item.ThoiGian = DateTime.Now;
            //        item.TrangThai = 0;
            //        db.EntityDauTus.InsertOnSubmit(item);

            //        db.SubmitChanges();
            //        ketqua = item.Id;

            //        EntityDuAn duan = db.EntityDuAns.Where(g => g.Id == IdDuAn).First();
            //        int songuoidautu = duan.SoNguoiDaDauTu == null ? 0 : duan.SoNguoiDaDauTu.Value;
            //        songuoidautu++;
            //        duan.SoNguoiDaDauTu = songuoidautu;
            //        int tienhientai = duan.TienDauTuHienTai == null ? 0 : duan.TienDauTuHienTai.Value;
            //        duan.TienDauTuHienTai = tienhientai + SoTien;

            //        db.SubmitChanges();

            //        // them hoat động
            //        HoatDongModel.GetListFriend_Sent_HoatDong(idlogin, IdDuAn, 4);// đầu tư
            //        if (duan.TienDauTuHienTai >= duan.TienDauTuMucTieu)
            //        {
            //            duan.TrangThaiFund = 1;
            //            db.SubmitChanges();
            //            HoatDongModel.GetListFriend_Sent_HoatDong(idlogin, IdDuAn, 4);// dự án thành công
            //        }
            //    }


            //}
            //catch
            //{
               
            //}

            //return View();
            //var item = db.EntityDauTus.Where(p => p.Id == Id).FirstOrDefault();
            //return View(item);

        }

        //public string KetThuc_ThanhToan(int IdDuAn, int PhanThuong, int SoTien, int loaihinh, string SDT)
        public string KetThuc_ThanhToan(int IdDuAn, int PhanThuong, int SoTien, string SDT)
        {
            try
            {
                Random r = new Random();
                string orderId = r.Next().ToString();
                int NgayChuyenTien = 0;
                int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
                if (PhanThuong >= 0)
                {
                    #region Thêm "Đầu Tư" hoặc Cập Nhật "Đầu Tư"
                    EntityDauTu item;
                    EntityDuAn duan;

                    //Cập Nhật
                    //if (db.EntityDauTus.Any(g => g.IdDuAn == IdDuAn && g.IdUser == idlogin) == true)
                    //{
                    //    item = db.EntityDauTus.Where(g => g.IdDuAn == IdDuAn && g.IdUser == idlogin).FirstOrDefault();
                    //    if (PhanThuong != 0)
                    //        item.IdPhanthuong = PhanThuong;
                    //    item.SoTienDauTu = SoTien;
                    //    item.ThoiGian = DateTime.Now;
                    //    item.TrangThai = 0;
                    //    item.NoteOwner = "SDT: " + SDT + " - Đã thanh toán bằng tài khoản Payoo";
                    //    item.OrderID_Payoo = orderId;

                    //    #region Cập nhật thông tin dự án
                    //    duan = db.EntityDuAns.Where(g => g.Id == IdDuAn).First();
                    //    NgayChuyenTien = (int)(duan.ThoiGianKetThuc.Value - DateTime.Now).Days;

                    //    //int songuoidautu = duan.SoNguoiDaDauTu == null ? 0 : duan.SoNguoiDaDauTu.Value;
                    //    //songuoidautu++;
                    //    //duan.SoNguoiDaDauTu = songuoidautu;
                    //    //int tienhientai = duan.TienDauTuHienTai == null ? 0 : duan.TienDauTuHienTai.Value;
                    //    //duan.TienDauTuHienTai = tienhientai + SoTien;                        
                    //    #endregion

                    //    //#region Cập nhật số lượng phần thưởng
                    //    //if (PhanThuong != 0)
                    //    //{
                    //    //    EntityPhanThuong pt = db.EntityPhanThuongs.Where(z => z.Id == PhanThuong).First();
                    //    //    int soluong = pt.SoNguoiMua == null ? 0 : pt.SoNguoiMua.Value;
                    //    //    soluong++;
                    //    //    pt.SoNguoiMua = soluong;
                    //    //}
                    //    //#endregion
                    //    db.SubmitChanges();
                    //}
                    //Thêm                     
                    //else
                    //{
                        item = new EntityDauTu();
                        item.IdDuAn = IdDuAn;
                        if (PhanThuong != 0)
                            item.IdPhanthuong = PhanThuong;
                        item.SoTienDauTu = SoTien;
                        item.IdUser = idlogin;
                        item.ThoiGian = DateTime.Now;
                        item.TrangThai = 0;
                        item.NoteOwner = "SDT: " + SDT + " - Đã thanh toán bằng tài khoản Payoo";
                        item.OrderID_Payoo = orderId;

                        #region Cập nhật thông tin dự án
                        duan = db.EntityDuAns.Where(g => g.Id == IdDuAn).First();
                        //int songuoidautu = duan.SoNguoiDaDauTu == null ? 0 : duan.SoNguoiDaDauTu.Value;
                        //songuoidautu++;
                        //duan.SoNguoiDaDauTu = songuoidautu;
                        //int tienhientai = duan.TienDauTuHienTai == null ? 0 : duan.TienDauTuHienTai.Value;
                        //duan.TienDauTuHienTai = tienhientai + SoTien;
                        NgayChuyenTien = (int)(duan.ThoiGianKetThuc.Value - DateTime.Now).Days;
                        #endregion

                        //#region Cập nhật số lượng phần thưởng
                        //if (PhanThuong != 0)
                        //{
                        //    EntityPhanThuong pt = db.EntityPhanThuongs.Where(z => z.Id == PhanThuong).First();
                        //    int soluong = pt.SoNguoiMua == null ? 0 : pt.SoNguoiMua.Value;
                        //    soluong++;
                        //    pt.SoNguoiMua = soluong;
                        //}
                        //#endregion
                        db.EntityDauTus.InsertOnSubmit(item);                        
                    //}
                    
                    
                    
                    //if (loaihinh == 1)
                    //{
                    //    item.NoteOwner = "SDT: " + SDT + " - Sẽ chuyển khoản ngân hàng";
                    //}
                    //else
                    //{
                    //    item.NoteOwner = "SDT: " + SDT + " - Sẽ thu tiền trực tiếp";
                    //}
                    //db.EntityDauTus.InsertOnSubmit(item);
                    #endregion                  

                    

                    db.SubmitChanges();

                    //#region Thêm hoạt động (để bạn bè biết mình đã đầu tư)
                    //HoatDongModel.GetListFriend_Sent_HoatDong(idlogin, IdDuAn, 4);// đầu tư
                    //if (duan.TienDauTuHienTai >= duan.TienDauTuMucTieu)
                    //{
                    //    duan.TrangThaiFund = 1;
                    //    db.SubmitChanges();
                    //    HoatDongModel.GetListFriend_Sent_HoatDong(idlogin, IdDuAn, 4);// dự án thành công
                    //}
                    //#endregion

                    //#region Gửi mail
                    //EntityUser ndtu = db.EntityUsers.Where(g => g.Id == idlogin).First();
                    //EntityUser cdan = db.EntityUsers.Where(g => g.Id == duan.IdUser).First();
                    //GuiMailXacNhanThanhToan_NguoiTaiTro(ndtu.HoTen, ndtu.Email, duan.TenDuAn, SoTien.ToString(), "/du-an/mv-hen-uoc-tay-ho-ky-duyen_77");
                    //GuiMailXacNhanThanhToan_ChuDauTu(cdan.HoTen, ndtu.HoTen, cdan.Email, duan.TenDuAn, SoTien.ToString(), "/du-an/mv-hen-uoc-tay-ho-ky-duyen_77");
                    //#endregion 
                    #region Thanh toán Payoo
                    return btnPaynow_Click(orderId, duan.TenDuAn, SoTien, NgayChuyenTien, item);
                    #endregion
                }
                return "0";
            }
            catch
            {
                return "0";
            }
        }

        public void GuiMailXacNhanThanhToan_NguoiTaiTro(string HoTen, string Email, string TenDuAn, string sotien, string linkduan)
        {
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("<h1>[FirstStep]-Xác nhận đã tài trợ</h1>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat("<p> Chào bạn: " + " " + HoTen + "   !  " + "</p>");
            mailBody.AppendFormat("<p>Cảm ơn bạn đã ủng hộ dự án :" + " " + TenDuAn + " với gói tài trợ là " + sotien + " đồng. Hệ thống sẽ gửi mail xác nhận cho bạn khi nhận được tiền tài trợ từ bạn." + "</p>");
            mailBody.AppendFormat("<p>Để theo dõi những thông tin mới nhất về dự án, bạn vui lòng theo dõi mục cập nhật trong phần thông tin về dự án bằng cách nhấn vào link dưới đây." + "</p>");
            string[] mang = Request.Url.AbsoluteUri.ToString().Split('/');
            string url = mang[0] + "//" + mang[2];
            mailBody.AppendFormat("<p>" + url + linkduan + "</p>");
            mailBody.AppendFormat("<br></br>");
            mailBody.AppendFormat("<p>Đây là email hệ thống, vui lòng không gửi hồi đáp mail này!</p>");
            MailHelper.SendMailMessage("firststep.system.info@gmail.com", Email, null, null, "Xác nhận đã tài trợ - dự án " + TenDuAn, mailBody.ToString(), "smtp.gmail.com", true, "firststep.system.info@gmail.com", "!@#Hien4567");
        }
        public void GuiMailXacNhanThanhToan_ChuDauTu(string HoTen, string HoTen1, string Email, string TenDuAn, string sotien, string linkduan)
        {
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("<h1>[FirstStep]-Thông báo có người đầu tư mới</h1>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat("<p> Chào bạn: " + " " + HoTen + "   !  " + "</p>");            
            mailBody.AppendFormat("<p> " + HoTen1 + " đã tài trợ " + sotien + " đồng cho dự án <b>" + TenDuAn + "</b> của bạn.</p>");
            mailBody.AppendFormat("<p>Hãy đăng nhập để xem thông tin người tài trợ cũng như các thông tin cần thiết khác để chuẩn bị tốt hơn cho việc gửi quà tặng nếu dự án thành công." + "</p>");
            string[] mang = Request.Url.AbsoluteUri.ToString().Split('/');
            string url = mang[0] + "//" + mang[2];
            mailBody.AppendFormat("<p>" + url + linkduan + "</p>");
            mailBody.AppendFormat("<br></br>");
            mailBody.AppendFormat("<p>Đây là email hệ thống, vui lòng không gửi hồi đáp mail này!</p>");
            MailHelper.SendMailMessage("firststep.system.info@gmail.com", Email, null, null, "Thông báo có người đầu tư mới - dự án " + TenDuAn, mailBody.ToString(), "smtp.gmail.com", true, "firststep.system.info@gmail.com", "!@#Hien4567");
        }

        //Thanh toán với Payoo
        protected string btnPaynow_Click(string orderId, string TenDuAn, int SoTien, int NgayChuyenTien, EntityDauTu item)
        {
            EntityPhanThuong phanthuong;
            if (item.IdPhanthuong.GetValueOrDefault() == 0)
            {
                phanthuong = new EntityPhanThuong();
                phanthuong.NoiDung = "Bạn không nhận phần thưởng";
            }
            else
            {
                phanthuong = db.EntityPhanThuongs.Where(p => p.Id == item.IdPhanthuong).First();

            }
            PayooOrder order = new PayooOrder();
            order.Session = orderId;
            order.BusinessUsername = ConfigurationManager.AppSettings["BusinessUsername"];
            order.OrderCashAmount = SoTien;
            order.OrderNo = orderId;
            order.ShippingDays = short.Parse(NgayChuyenTien.ToString());
            order.ShopBackUrl = ConfigurationManager.AppSettings["ShopBackUrl"];
            order.ShopDomain = ConfigurationManager.AppSettings["ShopDomain"];
            order.ShopID = long.Parse(ConfigurationManager.AppSettings["ShopID"]);
            order.ShopTitle = ConfigurationManager.AppSettings["ShopTitle"];
            order.StartShippingDate = DateTime.Now.ToString("dd/MM/yyyy");
            order.NotifyUrl = ConfigurationManager.AppSettings["NotifyUrl"];
            //You can do

            string OrderHtml = "<table width='100%' border='1' cellspacing='0'><thead><tr><td width='30%' align='center'><b>Dự án bạn đã đầu tư</b></td><td width='45%' align='center'><b>Phần thưởng</b></td><td width='25%' align='center'><b>Số tiền</b></td></tr></thead><tbody>";
            OrderHtml += "<tr><td align='center'>" + TenDuAn + "</td><td align='center'>" + phanthuong.NoiDung + "</td><td align='right'>" + String.Format("{0:0,0 VNĐ}", SoTien) + "</td></tr>";
            //OrderHtml += "<tr><td align='left'>Chi phí thuế</td><td align='right'>50,000</td><td align='right'>50,000</td></tr>";
            OrderHtml += "<tr><td align='right' colspan='2'><b>Tổng tiền:</b></td><td align='right'>" + String.Format("{0:0,0 VNĐ}", SoTien) + "</tr>";
            OrderHtml += "<tr><td align='left' colspan='3'>Một vài lưu ý khác: </td></tr></tbody></table>";
            order.OrderDescription = HttpUtility.UrlEncode(OrderHtml);

            //order.OrderDescription = HttpUtility.UrlEncode("<table width='100%' border='1' cellspacing='0'><thead><tr><td width='45%' align='center'><b>Tên dự án</b></td><td width='30%' align='center'><b>Số tiền đầu tư</b></td><td width='25%' align='center'><b>Thành tiền</b></td></tr></thead><tbody><tr><td align='left'>Dự án chuồn chuồn giấy</td><td align='right'>500,000</td><td align='right'>500,000</td></tr><tr><td align='left'>Chi phí thuế</td><td align='right'>50,000</td><td align='right'>50,000</td></tr><tr><td align='right' colspan='2'><b>Tổng tiền:</b></td><td align='right'>550,000</td></tr><tr><td align='left' colspan='3'>Một vài lưu ý khác</td></tr></tbody></table>");

            string Checksum = string.Empty;

            //Su dung checksum ko ma hoa du lieu
            //string ChecksumKey = ConfigurationManager.AppSettings["ChecksumKey"];
            //string XML = PaymentXMLFactory.GetPaymentXML(order);
            //Checksum = SHA1encode.hash(ChecksumKey + XML);

            //khong su dung checksum, co ma hoa du lieu
            string XML = PaymentXMLFactory.GetPaymentXML(order, Server.MapPath(@"..\App_Data\Certificates\biz_pkcs12.p12"), "alpe", Server.MapPath(@"..\App_Data\Certificates\payoo_public_cert.pem"));            

            return RedirectToProvider(ConfigurationManager.AppSettings["PayooCheckout"], XML, Checksum);
        }

        private string RedirectToProvider(string ProviderUrl, string XMLCheckout, string checksum)
        {
            //string redirect = "<html><head><title></title></head>";
            string redirect = "<input type='hidden' name='OrdersForPayoo' value='" + XMLCheckout + "'/>";

            if (!string.IsNullOrEmpty(checksum))
            {
                redirect += "<input type='hidden' name='CheckSum' value='" + checksum + "'/>";
            }            
            return redirect;
        }

        public ActionResult HoanTatThanhToan(int session, int order_no, int status)
        {
            EntityDauTu dautu = db.EntityDauTus.Where(dt => dt.OrderID_Payoo == order_no.ToString()).First();
            if (dautu.TrangThai == 1)
            {
                Response.Redirect(Url.Action("Index", "Home"));
            }
            else
            {   
                if (status == 1)
                {
                    dautu.TrangThai = 1;
                    #region Cập nhật thông tin dự án
                    EntityDuAn duan = db.EntityDuAns.Where(g => g.Id == dautu.IdDuAn).First();
                    int songuoidautu = duan.SoNguoiDaDauTu == null ? 0 : duan.SoNguoiDaDauTu.Value;
                    songuoidautu++;
                    duan.SoNguoiDaDauTu = songuoidautu;
                    int tienhientai = duan.TienDauTuHienTai == null ? 0 : duan.TienDauTuHienTai.Value;
                    duan.TienDauTuHienTai = tienhientai + dautu.SoTienDauTu;
                    #endregion

                    #region Cập nhật số lượng phần thưởng
                    if (dautu.IdPhanthuong != 0)
                    {
                        EntityPhanThuong pt = db.EntityPhanThuongs.Where(z => z.Id == dautu.IdPhanthuong).First();
                        int soluong = pt.SoNguoiMua == null ? 0 : pt.SoNguoiMua.Value;
                        soluong++;
                        pt.SoNguoiMua = soluong;
                    }
                    #endregion
                }
                else
                {
                    dautu.TrangThai = -1;
                }
                db.SubmitChanges();
                return View(dautu);
            }
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public void KetQuaThanhToan()
        {
            string NotifyMessage = Request.Form.Get("NotifyData");

            //NotifyMessage = "<?xml version='1.0'?><PayooConnectionPackage xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><Data>PFBheW1lbnROb3RpZmljYXRpb24+PHNob3BzPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxzaG9wPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8c2Vzc2lvbj4xMDAwODY8L3Nlc3Npb24+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx1c2VybmFtZT50cnV5ZW5oaW5oYXZnPC91c2VybmFtZT4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHNob3BfaWQ+NDE1PC9zaG9wX2lkPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8c2hvcF90aXRsZT5UcnV5ZW4gaGluaCBBbiBWaWVuPC9zaG9wX3RpdGxlPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8c2hvcF9kb21haW4+aHR0cDovL3BheWdhdGUudHJ1eWVuaGluaGFudmllbi52bi88L3Nob3BfZG9tYWluPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8c2hvcF9iYWNrX3VybD5odHRwOi8vcGF5Z2F0ZS50cnV5ZW5oaW5oYW52aWVuLnZuL1BheW9vL3Jlc3VsdC5hc3B4PC9zaG9wX2JhY2tfdXJsPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8b3JkZXJfbm8+MTAwMDg2PC9vcmRlcl9ubz4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPG9yZGVyX2Nhc2hfYW1vdW50PjE8L29yZGVyX2Nhc2hfYW1vdW50PgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8b3JkZXJfc2hpcF9kYXRlPjIzLzEwLzIwMTM8L29yZGVyX3NoaXBfZGF0ZT4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPG9yZGVyX3NoaXBfZGF5cz4yPC9vcmRlcl9zaGlwX2RheXM+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxvcmRlcl9kZXNjcmlwdGlvbj5UaGFuaCB0b8OhbiBjxrDhu5tjIHRydXnhu4FuIGjDrG5oIEFuIFZpw6puLiBNw6MgZ2lhbyBk4buLY2ggMTAwMDg2IHPhu5EgdGnhu4FuIDEgbmfDoHkgZ2lhbyBk4buLY2ggMjMvMTAvMjAxMzwvb3JkZXJfZGVzY3JpcHRpb24+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxub3RpZnlfdXJsPmh0dHA6Ly9wYXlnYXRlLnRydXllbmhpbmhhbnZpZW4udm4vUGF5b28vTm90aWZ5TGlzdGVuZXIuYXNweDwvbm90aWZ5X3VybD4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3Nob3A+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3Nob3BzPjxTdGF0ZT5QQVlNRU5UX1JFQ0VJVkVEPC9TdGF0ZT48L1BheW1lbnROb3RpZmljYXRpb24+</Data><Signature>MIIBbQYJKoZIhvcNAQcCoIIBXjCCAVoCAQExCzAJBgUrDgMCGgUAMAsGCSqGSIb3DQEHATGCATkwggE1AgEBMIGSMIGEMQswCQYDVQQGEwJWVTEMMAoGA1UECBMDSENNMQwwCgYDVQQHEwNIQ00xEjAQBgNVBAoTCVZpZXRVbmlvbjEOMAwGA1UECxMFUGF5b28xDjAMBgNVBAMTBVBheW9vMSUwIwYJKoZIhvcNAQkBFhZwYXlvb0B2aWV0dW5pb24uY29tLnZuAgkA673T+Q8894cwCQYFKw4DAhoFADANBgkqhkiG9w0BAQEFAASBgBdgEhkRcz/oAeGhxmAfYoE6s/4JlcQcKB44Rq0N/cVnhJwbMCvi++HPTaknUuLyfSk0To7mXfkroU1gUsD5qlEmU/oBGP9Fm6E8GJbwgWaHKUcG0UvTq8Jj+22F5FKNJmkSuDXNOT3XID5+efk+MR5QIZMXlwGGleET5EloiZlI</Signature><PayooSessionID>Jjm+AAobumDw5OFaR3NttRWZPgORM+a9noPLT7fE3wEL+Cf4MeqmPq11zA2h0MHp7AdxhkiefekH6+QOQjLNAg==</PayooSessionID></PayooConnectionPackage>";

            if (NotifyMessage == null || "".Equals(NotifyMessage))
            {
                return;
            }
            Credential credential = new Credential();
            credential.APIUsername = ConfigurationManager.AppSettings["APIUsername"];
            credential.APIPassword = ConfigurationManager.AppSettings["APIPassword"];
            credential.APISignature = ConfigurationManager.AppSettings["APISignature"];


            PayooNotify listener = new PayooNotify(NotifyMessage,ConfigurationManager.AppSettings["PayooBusinessAPI"],credential, Server.MapPath(@"..\App_Data\Certificates\biz_pkcs12.p12"), "alpe",Server.MapPath(@"..\App_Data\Certificates\payoo_public_cert.pem"));
            if (listener.CheckNotifyMessage())
            {
                PaymentNotification invoice = listener.GetPaymentNotify();
                EntityDauTu dautu = db.EntityDauTus.Where(dt => dt.OrderID_Payoo == invoice.OrderNo.ToString()).First();
                if (listener.ConfirmToPayoo())
                {
                    dautu.TrangThai = 1;
                    #region Cập nhật thông tin dự án
                    EntityDuAn duan = db.EntityDuAns.Where(g => g.Id == dautu.IdDuAn).First();
                    int songuoidautu = duan.SoNguoiDaDauTu == null ? 0 : duan.SoNguoiDaDauTu.Value;
                    songuoidautu++;
                    duan.SoNguoiDaDauTu = songuoidautu;
                    int tienhientai = duan.TienDauTuHienTai == null ? 0 : duan.TienDauTuHienTai.Value;
                    duan.TienDauTuHienTai = tienhientai + dautu.SoTienDauTu;
                    #endregion

                    #region Cập nhật số lượng phần thưởng
                    if (dautu.IdPhanthuong != 0)
                    {
                        EntityPhanThuong pt = db.EntityPhanThuongs.Where(z => z.Id == dautu.IdPhanthuong).First();
                        int soluong = pt.SoNguoiMua == null ? 0 : pt.SoNguoiMua.Value;
                        soluong++;
                        pt.SoNguoiMua = soluong;
                    }
                    #endregion
                }
                else
                {
                    dautu.TrangThai = -2;
                }
            }
            else
            {
                db.EntityDauTus.Where(dt => dt.OrderID_Payoo == listener.GetPaymentNotify().OrderNo.ToString()).First().TrangThai = -3;                
            }
            db.SubmitChanges();
        }

    }
}
