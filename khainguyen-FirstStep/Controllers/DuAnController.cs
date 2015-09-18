using khainguyen_FirstStep.Models;
using MvcLibrary.Repository;
using Payoo.Lib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace khainguyen_FirstStep.Controllers
{
    public class DuAnController : Controller
    {
        static IList<EntityDuAn> ListDuAn = new List<EntityDuAn>();
        private dbFirstStepDataContext db = new dbFirstStepDataContext();
        // int pagesize = 5;
        int pagesize1 = 3;

        #region khám phá

        public ActionResult Test()
        {
            return View();
        }

        public String CheckPayooAccount(String username)
        {
            String result = "";
            try
            {
                Credential credential = new Credential();
                credential.APIUsername = ConfigurationManager.AppSettings["APIUsername"];
                credential.APIPassword = ConfigurationManager.AppSettings["APIPassword"];
                credential.APISignature = ConfigurationManager.AppSettings["APISignature"];
                Caller caller = new Caller();
                caller.InitCall(ConfigurationManager.AppSettings["PayooBusinessAPI"], credential,
                    Server.MapPath(@"..\App_Data\Certificates\biz_pkcs12.p12"), "alpe", Server.MapPath(@"..\App_Data\Certificates\payoo_public_cert.pem"));

                CheckPayooAccountRequestType req = new CheckPayooAccountRequestType();
                req.AccountID = username; // agentb2 ; vimp71 | anhtuan150787@gmail.com 

                CheckPayooAccountResponseType res = (CheckPayooAccountResponseType)caller.Call("CheckPayooAccount", req);
                if (res.Ack == AckCodeType.Success)
                {
                    result = "Valid";
                }
                else
                {
                    result = "InValid Account";
                }

            }
            catch (Exception ex)
            {
                result = "False";
                throw ex;
            }
            return result;
        }

        public ActionResult Index(string option)
        {
            var danhmuc = db.EntityDanhMucs.Where(p => p.IdRoot == 1).ToList();
            var trend = db.EntityDanhMucs.Where(g => g.IdRoot == 2).ToList();
            ViewBag.page = 0;
            ViewBag.trend = trend;
            ViewBag.option = option;
            if (option != null)
            {
                string s = option.Split('-')[0];
                ViewBag.seach = s;

            }
            else
                ViewBag.seach = "";
            return View(danhmuc);
        }

        // du an trang kham pha 
        public List<SearchModel> CreateSearchModel(string option)
        {
            List<SearchModel> ketqua = new List<SearchModel>();

            string[] mang = option.Split('-');
            for (int i = 1; i < mang.Count(); i++)
            {
                string[] mang1 = mang[i].Split('=');
                if (mang1.Count() == 2 && mang1[1] != "")
                {
                    SearchModel s = new SearchModel();
                    switch (mang1[0])
                    {
                        case "category":
                            s.op1 = 1;
                            s.op2 = int.Parse(mang1[1]);
                            break;
                        case "blend":
                            s.op1 = 2;
                            s.op2 = int.Parse(mang1[1]);
                            break;
                        case "social":// đã được tài trợ
                            s.op1 = 3;
                            s.op2 = 3;
                            s.op3 = int.Parse(mang1[1]);
                            break;
                        case "starred":// trạng thái
                            s.op1 = 3;
                            s.op2 = 4;
                            s.op3 = int.Parse(mang1[1]);
                            break;
                        case "tag_id": // đã hoàn thành
                            s.op1 = 3;
                            s.op2 = 6;
                            s.op3 = int.Parse(mang1[1]);
                            break;
                        case "backed":// mục tiêu
                            s.op1 = 3;
                            s.op2 = 5;
                            s.op3 = int.Parse(mang1[1]);
                            break;
                        case "syou":// dự án đã đầu tư
                            s.op1 = 3;
                            s.op2 = 1;
                            s.op3 = 1;
                            break;
                        case "sfriend":// dự án đã đầu tư
                            s.op1 = 3;
                            s.op2 = 2;
                            s.op3 = 1;
                            break;
                    }
                    ketqua.Add(s);
                }
            }
            return ketqua;
        }
        public ActionResult _Index_DuAnHienThi(int page, string option)
        {

            int currentPageIndex = page;

            if (page != 0)
                currentPageIndex = currentPageIndex * pagesize1;

            List<EntityDuAn> ketqua = db.EntityDuAns.Where(g => g.TrangThai == 2).ToList();
            if (option != null)
            {
                string s = option.Split('-')[0].ToLower();
                s = s.Trim();
                if (s != "")
                    ketqua = ketqua.ToList().Where(g => Utilities.Encode(g.TenDuAn.ToLower()).IndexOf(Utilities.Encode(s)) != -1
                        || Utilities.Encode(g.NoiDung.ToLower()).IndexOf(Utilities.Encode(s)) != -1
                        || Utilities.Encode(g.EntityUser.HoTen.ToLower()).IndexOf(Utilities.Encode(s)) != -1
                        ).ToList();
            }
            if (option == null)
            {

                return PartialView(ketqua.Skip(currentPageIndex).Take(pagesize1).ToList());
            }
            List<SearchModel> optionsearch = CreateSearchModel(option);

            DuAnModel.GetDuAn_Search(optionsearch, ref ketqua);
            return PartialView(ketqua.Skip(currentPageIndex).Take(pagesize1).ToList());
        }
        #endregion

        #region tạo dự án
        public ActionResult TaoDuAn()
        {
            if (Request.Cookies["ftid"] != null)
            {
                ViewBag.DanhMuc = db.EntityDanhMucs.Where(g => g.IdRoot == 1).ToList();
                DuAnModel daModel = new DuAnModel();
                return View(daModel);
            }
            else
            {
                return RedirectToAction("Login","Account", new { trolai = "tao-du-an"});
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoDuAn(DuAnModel daModel)
        {
            try
            {
                ViewBag.DanhMuc = db.EntityDanhMucs.Where(g => g.IdRoot == 1).ToList();
                if (ModelState.IsValid)
                {
                    int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);

                    EntityThongTinTaiKhoan thongtin = new EntityThongTinTaiKhoan();
                    db.EntityThongTinTaiKhoans.InsertOnSubmit(thongtin);
                    db.SubmitChanges();

                    EntityDuAn duan = new EntityDuAn();
                    duan.LoaiHinhGoiVon = daModel.Loaihinhkeugoivon;
                    duan.TenDuAn = daModel.TenDuAn;
                    duan.IdDanhMuc = daModel.IdDanhMuc;
                    duan.TienDauTuMucTieu = daModel.TienDauTuMucTieu;
                    duan.IdUser = idlogin;
                    duan.TrangThai = -1;
                    duan.ThongTinWhichPerson = daModel.whichpreson;
                    duan.IdThongTinTaiKhoan = thongtin.Id;
                    db.EntityDuAns.InsertOnSubmit(duan);
                    db.SubmitChanges();
                    HoatDongModel.GetListFriend_Sent_HoatDong(idlogin, duan.Id, 3);// tạo dự án
                    return RedirectToAction("TaoDuAn_Buoc2", "DuAn", new { Id = duan.Id });
                }
            }
            catch
            {

                //return RedirectToAction("Index", "Error", new { id = 500 });
            }
            return View(daModel);
        }


        public static String Decode(string Path)
        {
            String text;
            using (StreamReader sr = new StreamReader(Path))
            {
                text = sr.ReadToEnd();
                byte[] bytes = Convert.FromBase64String(text);
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder decoder = encoder.GetDecoder();
                int count = decoder.GetCharCount(bytes, 0, bytes.Length);
                char[] arr = new char[count];
                decoder.GetChars(bytes, 0, bytes.Length, arr, 0);
                text = new string(arr);

                return text;
            }
        }

        public ActionResult _LoadTabThongTin(int IdDuAn)
        {
            ViewBag.DanhMuc = db.EntityDanhMucs.Where(p => p.IdRoot == 1).ToList();
            return PartialView(DuAnModel.LayModel(IdDuAn));
        }

        public ActionResult _LoadTabPhanThuong(int IdDuAn)
        {
            return PartialView(DuAnModel.LayModel(IdDuAn));
        }

        public ActionResult _LoadTabMoTa(int IdDuAn)
        {
            return PartialView(DuAnModel.LayModel(IdDuAn));
        }

        public ActionResult _LoadTabGioiThieu(int IdDuAn)
        {
            return PartialView(DuAnModel.LayModel(IdDuAn));
        }

        public ActionResult _LoadTabTaiKhoan(int IdDuAn)
        {
            return PartialView(DuAnModel.LayModel(IdDuAn));
        }

        public ActionResult _LoadTabCapNhat(int IdDuAn)
        {
            return PartialView(DuAnModel.LayModel(IdDuAn));
        }

        public ActionResult _LoadTabXemTruoc(int IdDuAn)
        {
            var item = db.EntityDuAns.Where(p => p.Id == IdDuAn).FirstOrDefault();
            
            // xu ly video  xu ly phan tram tien
            if (!string.IsNullOrEmpty(item.LinkVideo))
            {
                string[] arr = item.LinkVideo.Split('=');
                if (arr.Length == 2)
                    ViewBag.video = "http://www.youtube.com/embed/" + arr[1];
                else
                    ViewBag.video = "";
            }
            else
            {
                ViewBag.video = "";
            }
            // xu ly phan tram tien 
            if (item.TienDauTuHienTai == 0 || item.TienDauTuHienTai == null)
            {
                item.TienDauTuHienTai = 0;
                ViewBag.phantram = "width:1%;";
            }
            else
            {
                ViewBag.phantram = "width:" + (item.TienDauTuHienTai * 100) / item.TienDauTuMucTieu + "%;";
            }
            if (item.TienDauTuMucTieu == null)
            {
                item.TienDauTuMucTieu = 0;
            }
            return PartialView(item);
        }

        public ActionResult UpdateDuAn(int IdDuAn)
        {
            EntityDuAn duan = db.EntityDuAns.Where(g => g.Id == IdDuAn).FirstOrDefault();
            ViewBag.duan = duan;
            EntityCapNhatDuAn capnhat = new EntityCapNhatDuAn();
            capnhat.IdDuAn = IdDuAn;
            return View(capnhat);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult UpdateDuAn(EntityCapNhatDuAn duan)
        {
            EntityDuAn DA = db.EntityDuAns.Where(g => g.Id == duan.IdDuAn).FirstOrDefault();
            try
            {

                EntityCapNhatDuAn capnhat = new EntityCapNhatDuAn();
                capnhat.NoiDung = duan.NoiDung;
                capnhat.IdDuAn = duan.IdDuAn;
                capnhat.TieuDe = duan.TieuDe;
                capnhat.TrangThai = 0;
                capnhat.ThoiGian = DateTime.Now;
                db.EntityCapNhatDuAns.InsertOnSubmit(capnhat);
                db.SubmitChanges();
            }
            catch
            { }
            return RedirectToAction("ChiTietDuAn", "DuAn", new { Title = Utilities.Paste_Int64(Utilities.Encode(DA.TenDuAn), DA.Id) });
            //  return View(duan);
        }

        public ActionResult ChinhSuaDuAn(int Id)
        {
            var item = db.EntityDuAns.Where(p => p.Id == Id).First();
            ViewBag.DanhMuc = db.EntityDanhMucs.Where(p => p.IdRoot == 1).ToList();
            if (Request.Cookies["ftid"] == null)
            {
                Response.Redirect(Url.Action("Index", "Home"));
            }
            //if (item.TrangThai == 1)
            //{
            //    return RedirectToAction("UpdateDuAn", "DuAn", new { IdDuAn = item.Id });
            //}
            if (item.TrangThai == -2)
            {
                //int i = 0;// xử lý link tới trang Block
            }
            return View(DuAnModel.LayModel(Id));
        }

        public ActionResult TaoDuAn_Buoc2(int Id)
        {
            var item = db.EntityDuAns.Where(p => p.Id == Id).First();

            if (Request.Cookies["ftid"].Value != item.IdUser.ToString())
            {
                Response.Redirect(Url.Action("Index", "Home"));
            }
            //if (item.TrangThai == 1)
            //{
            //    return RedirectToAction("UpdateDuAn", "DuAn", new { IdDuAn = item.Id });
            //}
            if (item.TrangThai == 2)
            {
                //int i = 0;// xử lý link tới trang Block
            }
            return View(DuAnModel.LayModel(Id));

        }

        [ValidateInput(false)]
        [HttpPost]
        //  public ActionResult TaoDuAn_Buoc2(int IdDuAn, string DanhMuc, string TenDuAn, string TienDauTuMucTieu, string DiaDiem, string ThongTinNgan, string SoNgay, string ThoiGianKetThuc, string PhanThuong, string LinkVideo, string MoTa, string RuiRo, string HoTen, string GioiThieu, string ChoO, string WebCaNhan)
        public ActionResult TaoDuAn_Buoc2(DuAnModel DA)
        {
            try
            {
                var duan = db.EntityDuAns.Where(p => p.Id == DA.Id).First();
                ViewBag.DanhMuc = db.EntityDanhMucs.Where(p => p.IdRoot == 1).ToList();

                #region Chỉnh sửa hình ảnh (chung cho toàn bộ)
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase hpf = Request.Files[i];

                    #region phan sua khi chay du an  // hình ảnh - phần thưởng - tiếu sử

                    if (hpf.FileName != "" && i == 0)
                    {
                        ImageHelper imgHelper = new ImageHelper();
                        string encodestring = imgHelper.encodeImageFile(hpf);
                        duan.HinhAnh = encodestring;
                        db.SubmitChanges();
                        var path = Path.Combine(Server.MapPath("~/Content/Images/DuAn"), encodestring);
                        hpf.SaveAs(path);
                    }

                    #endregion

                    if (duan.TrangThai < 1)
                    {
                        if (hpf.FileName != "" && i == 1)
                        {
                            var ava = db.EntityUsers.Where(p => p.Email == Request.Cookies["ftusername"].Value).First();

                            if (ava.Avatar != null && ava.Avatar.IndexOf("ava0.jpg") == -1)
                            {
                                string[] link = ava.Avatar.Split('/');
                                string fileToDelete = Path.Combine(Server.MapPath("~/Content/Images/Avatar"), link[link.Count() - 1]); // file hinh cu
                                System.IO.File.Delete(fileToDelete);
                            }
                            ImageHelper imgHelper = new ImageHelper();
                            string encodestring = imgHelper.encodeImageFile(hpf);
                            string[] mang = Request.Url.AbsoluteUri.ToString().Split('/');
                            string url = mang[0] + "//" + mang[2];
                            ava.Avatar = url + "/Content/Images/Avatar/" + encodestring;
                            imgHelper.ResizeStream(180, hpf.InputStream, Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Content\\Images\\Avatar\\", encodestring));
                            //  ava.Avatar = "http://localhost:41372/Content/Images/Avatar/" + encodestring;
                            db.SubmitChanges();
                            //var path = Path.Combine(Server.MapPath("~/Content/Images/Avatar"), encodestring);
                            //hpf.SaveAs(path);
                            Response.Cookies["ftavatar"].Value = ava.Avatar;
                        }
                        if (hpf.FileName != "" && i == 2)
                        {
                            ImageHelper imgHelper = new ImageHelper();
                            string encodestring = imgHelper.encodeImageFile(hpf);
                            imgHelper.ResizeStream(180, hpf.InputStream, Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Content\\Images\\TaiKhoan\\", encodestring));
                            //var path = Path.Combine(Server.MapPath("~/Content/Images/TaiKhoan"), encodestring);
                            //hpf.SaveAs(path);
                            DA.cnHinhAnh = encodestring;
                        }
                        if (hpf.FileName != "" && i == 3)
                        {
                            ImageHelper imgHelper = new ImageHelper();
                            string encodestring = imgHelper.encodeImageFile(hpf);
                            imgHelper.ResizeStream(180, hpf.InputStream, Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Content\\Images\\TaiKhoan\\", encodestring));
                            //var path = Path.Combine(Server.MapPath("~/Content/Images/TaiKhoan"), encodestring);
                            //hpf.SaveAs(path);
                            DA.ctHinhAnh = encodestring;
                        }
                    }
                }
                #endregion
                
                try
                {
                    if(DA.flag != "XemTruoc")
                        duan.Flag = DA.flag;
                    #region Cập nhật dữ liệu TAB MÔ TẢ
                    if (DA.flag == "MoTa")
                    {
                        duan.NoiDung = DA.NoiDung;
                        duan.LinkVideo = DA.LinkVideo;
                        duan.RuiRo = DA.RuiRo;
                    }

                    #endregion

                    #region xử lý trạng thái XEM TRƯỚC
                    if (DA.flag == "XemTruoc")
                    {

                    }
                    #endregion

                    //if (duan.TrangThai < 1)
                    //{
                    #region Cập nhật dữ liệu TAB THÔNG TIN
                    if (DA.flag == "ThongTin")
                    {
                        duan.LoaiHinhGoiVon = DA.Loaihinhkeugoivon;
                        duan.TenDuAn = DA.TenDuAn;
                        duan.TienDauTuMucTieu = DA.TienDauTuMucTieu;
                        duan.ThongTinNgan = DA.ThongTinNgan;


                        duan.IdDanhMuc = DA.IdDanhMuc;
                        duan.TienDauTuHienTai = 0;

                        duan.ThongTinWhichPerson = DA.whichpreson;
                        if (DA.SoNgay != null && DA.SoNgay > 0)
                        {
                            duan.SoNgay = DA.SoNgay;
                            int i = Convert.ToInt16(DA.SoNgay);
                            duan.ThoiGianKetThuc = DateTime.Now.AddDays(i);
                        }
                        else
                        {
                            if (DA.ThoiGianKetThuc != null)
                            {
                                duan.ThoiGianKetThuc = Convert.ToDateTime(DA.ThoiGianKetThuc);
                                DateTime i = Convert.ToDateTime(DA.ThoiGianKetThuc);
                                // duan.SoNgay = Math.Abs(i.Subtract(DateTime.Now).Days);
                                duan.SoNgay = null;
                            }
                        }

                        //#region  // Phần thêm mới danh mục, hiện tại chưa dùng
                        //try
                        //{
                        //    var dm = db.EntityChiTietDanhMucs.Where(p => p.IdDuAn == DA.Id).ToList();
                        //    if (dm.Count() > 0)
                        //    {
                        //        dm.First().IdDanhMuc = Convert.ToInt16(DA.IdDanhMuc);
                        //        db.SubmitChanges();
                        //    }
                        //    else
                        //    {
                        //        EntityChiTietDanhMuc ct = new EntityChiTietDanhMuc();
                        //        ct.IdDanhMuc = Convert.ToInt16(DA.IdDanhMuc);
                        //        ct.IdDuAn = DA.Id;
                        //        db.EntityChiTietDanhMucs.InsertOnSubmit(ct);
                        //        db.SubmitChanges();
                        //    }
                        //}
                        //catch { }
                        //#endregion
                     }
                    //}
                    db.SubmitChanges();
                    #endregion

                    #region Cập nhật dữ liệu TAB PHẦN THƯỞNG
                    try
                    {
                        if (DA.flag == "PhanThuong")
                        {
                            if (DA.PhanThuong != null)
                            {
                                string[] arr1 = DA.PhanThuong.Split('#');
                                var phanthuong = db.EntityPhanThuongs.Where(g => g.IdDuAn == DA.Id).ToList();
                                for (int i = 0; i < DA.vt_pt_edit; i++)
                                {
                                    string[] arr2 = arr1[i].Split('^');
                                    for (int j = 0; j < arr2.Count() / 4; j = j + 4)
                                    {
                                        EntityPhanThuong pt = phanthuong.ElementAt(i);
                                        if (db.EntityDauTus.Any(g => g.IdPhanthuong == pt.Id) == true)
                                            continue;
                                        pt.IdDuAn = DA.Id;
                                        int tienhotro = 0;
                                        int.TryParse(arr2[j], out tienhotro);
                                        pt.TienHoTro = tienhotro;
                                        pt.HinhThucGiao = arr2[2];
                                        pt.NgayGiao = arr2[1];
                                        if (arr2[3] != "")
                                        {
                                            int SoLuong = 0;
                                            int.TryParse(arr2[3], out SoLuong);
                                            pt.SoLuong = SoLuong;
                                        }
                                        pt.NoiDung = arr2[4];

                                        //db.EntityPhanThuongs.InsertOnSubmit(pt);
                                        db.SubmitChanges();

                                    }
                                }
                                for (int i = DA.vt_pt_edit; i < arr1.Count() - 1; i++)
                                {
                                    string[] arr2 = arr1[i].Split('^');
                                    for (int j = 0; j < arr2.Count() / 4; j = j + 4)
                                    {
                                        EntityPhanThuong pt = new EntityPhanThuong();
                                        pt.IdDuAn = DA.Id;
                                        int tienhotro = 0;
                                        int.TryParse(arr2[j], out tienhotro);
                                        pt.TienHoTro = tienhotro;
                                        pt.HinhThucGiao = arr2[2];
                                        pt.NgayGiao = arr2[1];
                                        if (arr2[3] != "")
                                        {
                                            int SoLuong = 0;
                                            int.TryParse(arr2[3], out SoLuong);
                                            pt.SoLuong = SoLuong;
                                        }
                                        pt.NoiDung = arr2[4];

                                        db.EntityPhanThuongs.InsertOnSubmit(pt);
                                    }
                                }
                            }
                            db.SubmitChanges();
                        }
                    }
                    catch { }
                    #endregion

                    #region Cập nhật dữ liệu TAB GIỚI THIỆU
                    try
                    {
                        if (DA.flag == "GioiThieu")
                        {
                            var user = db.EntityUsers.Where(p => p.Id == Convert.ToInt16(Request.Cookies["ftid"].Value)).First();
                            user.HoTen = DA.HoTen;
                            user.GioiThieu = DA.GioiThieu;
                            user.DiaDiem = DA.ChoO;
                            user.Website = DA.WebCaNhan;
                            db.SubmitChanges();
                        }
                    }
                    catch { }
                    #endregion

                    #region Cập nhật dữ liệu TAB TÀI KHOẢN
                    try
                    {
                        if (DA.flag == "TaiKhoan")
                        {
                            //if (duan.TrangThai < 1 )
                            //{
                            EntityThongTinTaiKhoan thongtintaikhoan = db.EntityThongTinTaiKhoans.Where(g => g.Id == duan.IdThongTinTaiKhoan).FirstOrDefault();
                            if (thongtintaikhoan != null)
                            {
                                if (DA.whichpreson == "Cá nhân")
                                {
                                    thongtintaikhoan.Col1 = DA.cnName;
                                    thongtintaikhoan.Col2 = DA.cnYear;
                                    thongtintaikhoan.Col3 = DA.cnMonth;
                                    thongtintaikhoan.Col4 = DA.cnDay;
                                    thongtintaikhoan.Col5 = DA.cnHomeaddress1;
                                    thongtintaikhoan.Col6 = DA.cnHomeaddress2;
                                    thongtintaikhoan.Col7 = DA.cnHinhAnh != null && DA.cnHinhAnh != "" ? DA.cnHinhAnh : thongtintaikhoan.Col7;
                                }
                                else if (DA.whichpreson == "Doanh nghiệp")
                                {
                                    thongtintaikhoan.Col1 = DA.ctTenCongTy;
                                    thongtintaikhoan.Col2 = DA.ctDiaChi;
                                    thongtintaikhoan.Col3 = DA.ctMST;
                                    thongtintaikhoan.Col4 = DA.ctSDTBan;
                                    thongtintaikhoan.Col5 = DA.ctTenNguoiDaiDien;
                                    thongtintaikhoan.Col6 = DA.ctChucVu;
                                    thongtintaikhoan.Col7 = DA.ctHinhAnh != null && DA.ctHinhAnh != "" ? DA.ctHinhAnh : thongtintaikhoan.Col7;
                                }
                                db.SubmitChanges();
                            }
                            //}
                        }
                    }
                    catch { }
                    #endregion
                }
                catch { }

                #region Xuất Bản
                try
                {
                    if (DA.TrangThai == 1 && DA.CheckGioiThieu && DA.CheckMoTa && DA.CheckPhanThuong && DA.CheckThongTin && DA.CheckTaiKhoan)
                    {
                        duan.Flag = "XemTruoc";
                        //duan.ThoiGianBatDau = 
                        duan.TrangThai = DA.TrangThai;
                        duan.ThoigianSummit = DateTime.Now;
                        string[] mang = Request.Url.AbsoluteUri.ToString().Split('/');
                        string url = mang[0] + "//" + mang[2];
                        string linkdanhmucduan = url + "/du-an?option=-category=" + duan.IdDanhMuc + "-blend=0-";
                        MailHelper.SendMail_DangKyDuAn(duan.EntityUser.HoTen, duan.EntityUser.Email, linkdanhmucduan);
                        db.SubmitChanges();
                    }
                }
                catch { }
                #endregion

                // return View(DuAnModel.LayModel(duan.Id));
                return RedirectToAction("ChiTietDuAn", "DuAn", new { Title = Utilities.Paste_Int64(Utilities.Encode(DA.TenDuAn), DA.Id.Value) });
            }
            catch
            { return View(DA); }
        }

        

        public ActionResult _ChiTiet_TaoPhanThuong(int IdDuAn)
        {
            var query = db.EntityPhanThuongs.Where(g => g.IdDuAn == IdDuAn).ToList();
            return PartialView(query);
        }
        public ActionResult _ChiTiet_TaoPhanThuong_item(int vt)
        {
            ViewBag.vt = vt;
            return PartialView();
        }
        public string _XoaPhanThuong(int IdPhanThuong)
        {
            try
            {
                EntityPhanThuong pt = db.EntityPhanThuongs.Where(g => g.Id == IdPhanThuong).First();
                if (db.EntityDauTus.Any(g => g.IdPhanthuong == pt.Id) == true)
                {
                    return "error";
                }
                db.EntityPhanThuongs.DeleteOnSubmit(pt);
                db.SubmitChanges();
                return "complete";
            }
            catch { return "error"; }
        }
        public ActionResult _PopupLuuDuAn()
        {
            return PartialView();
        }
        #endregion

        #region Chi tiết dự án


        public ActionResult ChiTietDuAn(string Title,string InfoAdd)
        {
            try
            {
                ViewBag.InfoAdd = InfoAdd;
                int Id = Utilities.SplitName_Int64(Title);

                var item = db.EntityDuAns.Where(p => p.Id == Id).FirstOrDefault();
                if (item != null && item.TrangThai == 2)
                {
                    int luotview = item.SoLuotView == null ? 0 : item.SoLuotView.Value;
                    item.SoLuotView = luotview++;
                    db.SubmitChanges();

                }

                #region "xử lý thông tin dự án"
                // Các cập nhật của dự án
                var cp = db.EntityCapNhatDuAns.Where(p => p.IdDuAn == Id).ToList();
                ViewBag.capnhat = cp.Count();

                // Số người đã đầu tư dự án
                var sn = db.EntityDauTus.Where(p => p.IdDuAn == Id && p.TrangThai > 0).ToList();
                ViewBag.songuoithamgia = sn.Count();

                // Các bình luận của dự án
                var bl = db.EntityBinhLuans.Where(p => p.IdDuAn == Id).ToList();
                ViewBag.binhluan = bl.Count();

                // Có theo dõi dự án hay chưa
                if (Request.Cookies["ftid"] != null)
                {
                    var td = db.EntityTheoDoiDuAns.Where(p => p.IdDuAn == Id && p.IdUser == Convert.ToInt16(Request.Cookies["ftid"].Value)).ToList();
                    if (td.Count() > 0)
                        ViewBag.theodoi = td.FirstOrDefault().Watch;
                    else
                        ViewBag.theodoi = 0;
                }
                else
                    ViewBag.theodoi = 0;
                #endregion

                if (item.TrangThai == 2)
                {
                    #region // xu ly video  xu ly phan tram tien

                    if (item.LinkVideo != null && item.LinkVideo != "")
                    {
                        string[] arr = item.LinkVideo.Split('=');
                        if (arr.Length == 2)
                            ViewBag.video = "http://www.youtube.com/embed/" + arr[1];
                        else
                            ViewBag.video = "";
                    }
                    else
                    {
                        ViewBag.video = "";
                    }
                    // xu ly phan tram tien 
                    if (item.TienDauTuHienTai == 0 || item.TienDauTuHienTai == null)
                    {
                        ViewBag.phantram = "width:1%;";
                    }
                    else
                    {
                        ViewBag.phantram = "width:" + (item.TienDauTuHienTai * 100) / item.TienDauTuMucTieu + "%;";
                    }
                    #endregion

                    #region "xử lý khác"
                    // so ngay ket thuc 

                    DateTime date = Convert.ToDateTime(item.ThoiGianKetThuc);

                    if (date <= DateTime.Now)
                    {
                        //ViewBag.thoigianconlai = 0;
                        ViewBag.thoigianconlai = (int)(item.ThoiGianKetThuc.Value - DateTime.Now).TotalMinutes;
                        ViewBag.songaydaqua = (int)(item.ThoiGianKetThuc.Value - item.ThoiGianBatDau.Value).TotalDays;
                    }
                    else
                    {
                        ViewBag.thoigianconlai = (int)(item.ThoiGianKetThuc.Value - DateTime.Now).TotalMinutes;
                        ViewBag.songaydaqua = (int)(DateTime.Now - item.ThoiGianBatDau.Value).TotalDays;
                    }
                    #endregion

                    #region "xác định quyền"
                    string dathamgia = "false";
                    string idlogin = "";
                    if (Request.Cookies["ftid"] != null)
                    {
                        idlogin = Request.Cookies["ftid"].Value.ToString();
                        // xác định có là chủ của dự án
                        if (item.IdUser.ToString() == idlogin)
                        {
                            if (item.TrangThai <= 1) 
                                ViewBag.cothethamgia = "edit"; 
                            else 
                                ViewBag.cothethamgia = "manager";
                        }
                        else if (item.TrangThai == 2)
                        {
                            if (sn.Any(g => g.EntityUser.Id.ToString() == idlogin && g.TrangThai > 0) == true)
                                dathamgia = "true";

                            if (dathamgia == "false" && date >= DateTime.Now && item.IdUser.ToString() != idlogin)
                                ViewBag.cothethamgia = "true";
                            else
                                ViewBag.cothethamgia = "false";
                        }
                    }
                    else
                    {
                        ViewBag.cothethamgia = "false";
                    }
                    #endregion
                }
                else if (item.TrangThai == -1 || item.TrangThai == 0 || item.TrangThai == 1)
                {
                    return RedirectToAction("TaoDuAn_Buoc2", "DuAn", new { Id = item.Id });
                }
                return View(item);
            }
            catch
            {
                throw (new Exception());
            }
        }

        public ActionResult _ChiTietDuAn_SoNguoiThamGia(int Id)
        {

            var item = db.EntityDauTus.Where(p => p.IdDuAn == Id && p.TrangThai > 0).ToList();
            ViewBag.tongso = item.Count();
            return PartialView(item);
        }

        public ActionResult _ChiTietDuAn_PhanThuong(int Id, string Status)
        {

            var item = db.EntityPhanThuongs.Where(p => p.IdDuAn == Id).ToList();
            ViewBag.tongso = item.Count();
            ViewBag.cothethamgia = Status;
            return PartialView(item);
        }

        public ActionResult _ChiTietDuAn_CapNhat(int Id)
        {

            var item = db.EntityCapNhatDuAns.Where(p => p.IdDuAn == Id).OrderByDescending(p => p.ThoiGian).ToList();
            return PartialView(item);
        }
        public ActionResult _ChiTietDuAn_NguoiThamGia(int Id)
        {

            var item = db.EntityDauTus.Where(p => p.IdDuAn == Id).ToList();
            return PartialView(item);
        }
        public ActionResult _ChiTietDuAn_BinhLuan(int Id)
        {

            var item = db.EntityBinhLuans.Where(p => p.IdDuAn == Id).ToList();
            return PartialView(item);
        }
        public ActionResult _ChiTietDuAn_NhomDuAn(int Id)
        {

            var item = db.EntityNhomChienDiches.Where(p => p.IdDuAn == Id && p.TrangThai == true).ToList();
            return PartialView(item);
        }
        public ActionResult _ChiTietDuAn_TagsDuAn(int Id)
        {

            var item = db.EntityDuAnTags.Where(p => p.IdDuAn == Id).ToList();
            return PartialView(item.Take(4));
        }
        public ActionResult _ChiTietDuAn_SoDuAnThamGia(int Id)
        {
            var item = db.EntityDauTus.Where(p => p.IdUser == Id).ToList();
            ViewBag.SoDuAnThamGia = item.Count();
            return PartialView();
        }
        public ActionResult _ChiTietDuAn_SlideDuAnFooter(int Id, int IdDuAn)
        {
            var item = db.EntityDuAns.OrderByDescending(p => p.Id).Where(p => p.Id != IdDuAn && p.TrangThai == 2).ToList();
            if (item.Count() > 4)
                return PartialView(item.Take(4));
            else
                return PartialView(item);
        }

        // gui binh luan 
        public string _ChiTietDuAn_GuiBinhLuan(int IdDuAn, string NoiDung, string Publish)
        {
            try
            {
                int pl;
                if (Publish == "true")
                { pl = 1; }
                else
                { pl = 0; }
                int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
                EntityBinhLuan bl = new EntityBinhLuan();
                bl.IdDuAn = IdDuAn;
                bl.NoiDung = NoiDung;
                bl.Public = pl;
                bl.IdUser = idlogin;
                db.EntityBinhLuans.InsertOnSubmit(bl);
                db.SubmitChanges();

                HoatDongModel.GetListFriend_Sent_HoatDong(idlogin, IdDuAn, 5);

                return "conplete";
            }
            catch
            {
                return "error";
            }
        }
        public void GuiMail_CoBinhLuanMoi(string HoTen, string Email, string TenDuAn, string noidung, string linkduan)
        {
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("<h1>[FirstStep]-Thông báo có bình luận mới</h1>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat("<p> Chào bạn: " + " " + HoTen + "   !  " + "</p>");
            mailBody.AppendFormat("<p>Bạn có bình luận mới trong dự án :" + " " + TenDuAn + ". Vui lòng đăng nhập để xem toàn bộ nội dung bình luận." + "</p>");
            mailBody.AppendFormat("<p>" + " " + noidung + "</p>");
            string[] mang = Request.Url.AbsoluteUri.ToString().Split('/');
            string url = mang[0] + "//" + mang[2];
            mailBody.AppendFormat("<p>" + url + linkduan + "</p>");
            mailBody.AppendFormat("<br></br>");
            mailBody.AppendFormat("<p>Đây là email hệ thống, vui lòng không gửi hồi đáp mail này.</p>");
            MailHelper.SendMailMessage("firststep.system.info@gmail.com", Email, null, null, "Xác Nhận Tài Trợ Dự Án - " + TenDuAn, mailBody.ToString(), "smtp.gmail.com", true, "firststep.system.info@gmail.com", "!@#Hien4567");
        }

        //Submit button Theo Dõi Dự Án 
        public string _ChiTietDuAn_TheoDoi(int IdDuAn, int IdUser)
        {
            try
            {
                EntityTheoDoiDuAn td;
                var tdDuAn = db.EntityTheoDoiDuAns.Where(p => p.IdDuAn == IdDuAn && p.IdUser == IdUser);
                if (tdDuAn.Count() > 0)
                {
                    td = tdDuAn.FirstOrDefault();
                    if (td.Watch == 0)
                        td.Watch = 1;
                    else
                        td.Watch = 0;
                }
                else
                {
                    td = new EntityTheoDoiDuAn();
                    td.Watch = 1;
                    td.IdDuAn = IdDuAn;
                    td.IdUser = IdUser;
                    db.EntityTheoDoiDuAns.InsertOnSubmit(td);
                }                                
                db.SubmitChanges();
                return td.Watch.ToString();
            }
            catch
            {
                return "error";
            }
        }

        #endregion

        #region "Quản lý đầu tư dự án"

        public ActionResult QuanLy_DuAn(string Title)
        {
            string[] mang = Title.Split('_');
            int Id = int.Parse(mang[1]);
            ViewBag.TenDuAn = mang[0];
            ViewBag.IdDuAn = Id;
            var dautu = db.EntityDauTus.Where(g => g.IdDuAn == Id).ToList();
            return View(dautu);
        }

        public string RefundAll(int IdDuan)
        {
            string result = "false";            

            var dautu = db.EntityDauTus.Where(g => g.IdDuAn == IdDuan).ToList();
            foreach (var item in dautu)
            {
                Credential credential = new Credential();
                credential.APIUsername = ConfigurationManager.AppSettings["APIUsername"];
                credential.APIPassword = ConfigurationManager.AppSettings["APIPassword"];
                credential.APISignature = ConfigurationManager.AppSettings["APISignature"];
                Caller caller = new Caller();
                caller.InitCall(ConfigurationManager.AppSettings["PayooBusinessAPI"], credential,
                    Server.MapPath(@"..\App_Data\Certificates\biz_pkcs12.p12"), "alpe", Server.MapPath(@"..\App_Data\Certificates\payoo_public_cert.pem"));

                UpdateOrderStatusRequestType request = new UpdateOrderStatusRequestType();
                request.NewStatus = OrderStatus.Cancelled;
                request.ShopID = long.Parse(ConfigurationManager.AppSettings["ShopID"]);
                request.UpdateLog = "Dự án kết thúc thất bại.";
                request.OrderID = item.OrderID_Payoo;
                UpdateOrderStatusResponseType response = (UpdateOrderStatusResponseType)caller.Call("UpdateOrderStatus", request);
                if (response.Ack == AckCodeType.Success || response.Ack == AckCodeType.SuccessWithWarning)
                {
                    result = "Refund success";
                }
                else
                {
                    return "Fail";
                }
            }
            return result;
        }

        public void Quanlyduan_ExportExcel(int IdDuAn)
        {
            var grid = new GridView();

            grid.DataSource = from d in db.EntityDauTus
                              where d.IdDuAn == IdDuAn
                              select new
                              {
                                  Name = d.EntityDuAn.TenDuAn,
                                  Money = d.SoTienDauTu,
                                  Content = d.EntityPhanThuong.NoiDung,
                                  State = d.TrangThai  
                              };

            grid.DataBind();

            Response.ClearContent();

            Response.AddHeader("content-disposition", "attachment; filename=Danhsach_nguoitaitro.xls");

            Response.ContentType = "application/excel";

            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Write(sw.ToString());

            Response.End();
        }
        
        #endregion

        #region "Quản lý đầu tư dự án"

        public void GuiEmail(string Title)
        {
            string[] mang = Title.Split('_');
            int Id = int.Parse(mang[1]);
            ViewBag.TenDuAn = mang[0];
            var dautu = db.EntityDauTus.Where(g => g.IdDuAn == Id).ToList();
            foreach (var item in dautu)
            {
                GuiMail_BaoCon2ngay(item.EntityUser.HoTen, item.EntityUser.Email, item.EntityDuAn.TenDuAn, item.EntityDuAn.EntityUser.HoTen, "/du-an/mv-hen-uoc-tay-ho-ky-duyen_77");
            }
            //GuiMail_BaoCon2ngay("Duc Hoang", "hmduc87@gmail.com", "MV - Hẹn Ước", "nhóm Chuồn Chuồn Giấy", "/du-an/mv-hen-uoc-tay-ho-ky-duyen_77");            
            
        }

        public void GuiMail_BaoCon2ngay(string HoTen, string Email, string TenDuAn, string TenChuDuAn, string linkduan)
        {
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("<h1>[FirstStep]-Thông báo dự án sắp kết thúc</h1>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat("<p> Chào bạn " + " " + HoTen + " - thành viên tiên phong của FirstStep,</p>");
            mailBody.AppendFormat("<p>Dự án <b>" + TenDuAn + "</b> của " + "<b>" + TenChuDuAn + "</b> chỉ còn 02 ngày nữa kết thúc việc kêu gọi vốn. Mặc dù đây là dự án theo mô hình Crowdfunding đầu tiên của Firststep và nhóm Chuồn Chuồn Giấy, nhưng cũng đã thu hút được 50% mục tiêu kêu gọi.</p>");
            mailBody.AppendFormat("<p>Chúng ta đã đi được nửa đường và cần rất nhiều nỗ lực nữa để dự án có thể kêu gọi vốn thành công. Firststep tin rằng thành công của dự án MV Hẹn Ước sẽ là nguồn động lực khích lệ không chỉ cho nhóm Chuồn Chuồn Giấy mà còn cho cả những người có có ý tưởng đột phá, cộng đồng năng động khát khao sự đổi mới và cho cả những dự án Crowdfunding về sau của Firststep.</p>");
            mailBody.AppendFormat("<p>Vậy, xin đừng ngần ngại share link dưới đây lên trang Facebook của bạn hoặc những trang mạng xã hội khác để kêu gọi mọi người chung tay đóng góp cho dự án thành công.</p>");
            string[] mang = Request.Url.AbsoluteUri.ToString().Split('/');
            string url = mang[0] + "//" + mang[2];
            mailBody.AppendFormat("<p>" + url + linkduan + "</p>");
            mailBody.AppendFormat("<p>Nếu có người bạn của bạn thắc mắc về mô hình này, vui lòng gửi họ đường link dưới đây để họ được biết thêm về mô hình Crowdfunding:</p>");
            mailBody.AppendFormat("<p>" + "https://www.facebook.com/notes/firststep/crowdfunding-l%C3%A0-g%C3%AC/767410173348460" + "</p>");
            mailBody.AppendFormat("<br></br>");
            mailBody.AppendFormat("<p>Cám ơn các bạn, những con người tiên phong ủng hộ nhóm Chuồn Chuồn Giấy và mô hình huy động vốn cộng đồng cho các ý tưởng tuyệt vời đang thịnh hành trên thế giới!</p>");
            MailHelper.SendMailMessage("firststep.system.info@gmail.com", Email, null, null, "Thông báo dự án \" " + TenDuAn + "\" sắp kết thúc", mailBody.ToString(), "smtp.gmail.com", true, "firststep.system.info@gmail.com", "!@#Hien4567");
        }

        #endregion
    }
}
