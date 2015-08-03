using MvcLibrary.Repository;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace khainguyen_FirstStep.Models
{
    public class AllSearchModel
    {
        public IList<EntityDanhMuc> danhmuc = new List<EntityDanhMuc>();
        public IList<EntityTrend> Trend = new List<EntityTrend>();
        public IList<EntityDuAn> DuAn = new List<EntityDuAn>();
       
    }
    public class SearchModel
    {
        public int op1 =0;
        public int op2 = 0;
        public int op3 = 0;
    }
    public class DuAnModel
    {
        public string flag { get; set; }

        public int? Id { get; set; }

        public int? IdDanhMuc { get; set; }

        public int? Loaihinhkeugoivon { get; set; }

        [Required(ErrorMessage = "Tên dự án không được rỗng")]
        public string TenDuAn { get; set; }


        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Chỉ được nhập số > 0 !")]
        [Range(0, 500000000, ErrorMessage = "Giới hạn dự án dưới 500 triệu !")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = )]
        public int? TienDauTuMucTieu { get; set; }

        public string ThongTinNgan { get; set; }
        public int? SoNgay { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime ThoiGianKetThuc { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime ThoiGianBatDau { get; set; }

        public string HinhAnh { get; set; }
        public string PhanThuong { get; set; }
        public string LinkVideo { get; set; }
        public string NoiDung { get; set; }
        public string RuiRo { get; set; }
        public string HoTen { get; set; }
        public string GioiThieu { get; set; }
        public string ChoO { get; set; }
        public string WebCaNhan { get; set; }

        public int TrangThai { get; set; }
        public int TrangThaiFund { get; set; }

        public string whichpreson { get; set; }
        public int vt_pt_edit { get; set; }//vi tri phan thuong edit
        public string TKPayoo { get; set; }

        #region tài khoản

        //public string col1 = "";
        //public string col2 = "";
        //public string col3 = "";
        //public string col4 = "";
        //public string col5 = "";
        //public string col6 = "";
        //public string col7 = "";

        //tổ chức từ thiện
        public string tcTenToChuc { get; set; }
        public string tcDiaChi { get; set; }
        public string tcCoQuanChuQuan { get; set; }
        public string tcSDTBan { get; set; }
        public string tcTenNguoiDaiDien { get; set; }
        public string tcEmail { get; set; }
        public string Website { get; set; }
        public string tcHinhAnh { get; set; }

        //công ty
        public string ctWebsite { get; set; }
        public string ctEmail { get; set; }
        public string ctTenCongTy { get; set; } //col1
        public string ctDiaChi { get; set; } //col2
        public string ctMST { get; set; } //col3
        public string ctSDTBan { get; set; } //col4
        public string ctTenNguoiDaiDien { get; set; } //col5
        public string ctChucVu { get; set; } //col6
        public string ctHinhAnh { get; set; } //col7
        //cá nhân

        public string cnName { get; set; }
        public string cnYear { get; set; }
        public string cnMonth { get; set; }
        public string cnDay { get; set; }
        public string cnHomeaddress1 { get; set; }
        public string cnHomeaddress2 { get; set; }
        public string cnHinhAnh { get; set; }

        #endregion

        public Boolean CheckThongTin { get; set; }
        public Boolean CheckPhanThuong { get; set; }
        public Boolean CheckMoTa { get; set; }
        public Boolean CheckGioiThieu { get; set; }
        public Boolean CheckTaiKhoan { get; set; }


        // phần tạo tạo tài khoản

        public static DuAnModel LayModel(int Id)
        {
            DuAnModel Ban = new DuAnModel();
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            EntityDuAn tnew = db.EntityDuAns.Where(p => p.Id == Id).First();

            var thongtin = db.EntityUsers.Where(p => p.Id == Convert.ToInt16(HttpContext.Current.Request.Cookies["ftid"].Value)).First();
            
            Ban.flag = tnew.Flag == null ? "ThongTin": tnew.Flag;       

            Ban.TrangThai = tnew.TrangThai == null ? 0 : tnew.TrangThai.Value;
            Ban.Id = tnew.Id == null ? 0 : tnew.Id;

            #region Thông Tin
            Ban.IdDanhMuc = tnew.IdDanhMuc == null ? 0 : tnew.IdDanhMuc;
            Ban.Loaihinhkeugoivon = tnew.LoaiHinhGoiVon == null ? 0 : tnew.LoaiHinhGoiVon;
            Ban.TenDuAn = tnew.TenDuAn == null ? string.Empty : tnew.TenDuAn;
            Ban.HinhAnh = tnew.HinhAnh == null ? string.Empty : tnew.HinhAnh;
            Ban.ThongTinNgan = tnew.ThongTinNgan == null ? string.Empty : tnew.ThongTinNgan;
            Ban.TienDauTuMucTieu = tnew.TienDauTuMucTieu == null ? 0 : tnew.TienDauTuMucTieu;            
            Ban.ThoiGianKetThuc = tnew.ThoiGianKetThuc == null ? DateTime.Now : tnew.ThoiGianKetThuc.Value;
            Ban.SoNgay = tnew.SoNgay == null ? 0 : tnew.SoNgay;
            Ban.CheckGioiThieu = false;

            if (Ban.IdDanhMuc != 0 && Ban.TenDuAn != string.Empty && Ban.HinhAnh != string.Empty && Ban.ThongTinNgan != string.Empty && Ban.TienDauTuMucTieu != 0 && (Ban.SoNgay != 0 || (Ban.ThoiGianKetThuc > DateTime.Now)))
            {
                Ban.CheckThongTin = true;
            } 
            #endregion

            #region Phần thưởng
            var phanthuong = db.EntityPhanThuongs.Where(g=>g.IdDuAn== tnew.Id).ToList();
            Ban.vt_pt_edit = phanthuong.Count();
            Ban.CheckPhanThuong = false;

            if (Ban.vt_pt_edit > 0)
            {
                Ban.CheckPhanThuong = true;
            }
            #endregion

            #region Mô Tả
            Ban.LinkVideo = tnew.LinkVideo == null ? string.Empty : tnew.LinkVideo;
            Ban.NoiDung = tnew.NoiDung == null ? string.Empty : tnew.NoiDung;
            Ban.RuiRo = tnew.RuiRo == null ? string.Empty : tnew.RuiRo;
            Ban.CheckMoTa = false;

            if (Ban.NoiDung != string.Empty && Ban.RuiRo != string.Empty)
            {
                Ban.CheckMoTa = true;
            }
            #endregion

            #region Giới Thiệu Bản Thân
            Ban.ChoO = thongtin.DiaDiem == null ? string.Empty : thongtin.DiaDiem;
            Ban.WebCaNhan = thongtin.Website == null ? string.Empty : thongtin.Website;
            Ban.GioiThieu = thongtin.GioiThieu == null ? string.Empty : thongtin.GioiThieu;
            Ban.CheckGioiThieu = false;

            if (Ban.ChoO != string.Empty && Ban.WebCaNhan != string.Empty && Ban.GioiThieu != string.Empty)
            {
                Ban.CheckGioiThieu = true;
            }
            #endregion

            #region Tài Khoản
            Ban.TKPayoo = thongtin.TKPayoo == null ? string.Empty : thongtin.TKPayoo;
            Ban.whichpreson = tnew.ThongTinWhichPerson == null ? string.Empty : tnew.ThongTinWhichPerson;
            if (tnew.IdThongTinTaiKhoan > 0)
            {
                EntityThongTinTaiKhoan thongtintaikhoan = db.EntityThongTinTaiKhoans.Where(g=>g.Id== tnew.IdThongTinTaiKhoan).FirstOrDefault();
                if (thongtintaikhoan != null && Ban.whichpreson == "Cá nhân")
                {
                    Ban.cnName = thongtintaikhoan.Col1;
                    Ban.cnYear = thongtintaikhoan.Col2;
                    Ban.cnMonth = thongtintaikhoan.Col3;
                    Ban.cnDay = thongtintaikhoan.Col4;
                    Ban.cnHomeaddress1 = thongtintaikhoan.Col5;
                    Ban.cnHomeaddress2 = thongtintaikhoan.Col6;
                    Ban.cnHinhAnh = thongtintaikhoan.Col7;
                }
                if (thongtintaikhoan != null && Ban.whichpreson == "Doanh nghiệp")
                {
                    Ban.ctTenCongTy = thongtintaikhoan.Col1;
                    Ban.ctDiaChi = thongtintaikhoan.Col2;
                    Ban.ctMST = thongtintaikhoan.Col3;
                    Ban.ctSDTBan = thongtintaikhoan.Col4;
                    Ban.ctTenNguoiDaiDien = thongtintaikhoan.Col5;
                    Ban.ctChucVu = thongtintaikhoan.Col6;
                    Ban.ctHinhAnh = thongtintaikhoan.Col7;
                }
            }
            if (Ban.TKPayoo != String.Empty)
            {
                Ban.CheckTaiKhoan = true;
            }
            #endregion

            return Ban;

        }

        public static void GetDuAn_Trend(ref  List<EntityDuAn> ketqua, int loai)
        
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var danhsachduan = ketqua;
            List<EntityDuAn>  query = new List<EntityDuAn>();
            switch (loai)
            {
                case 21:// trend - mới nhất
                    query = danhsachduan.OrderByDescending(g => g.ThoiGianBatDau).ToList();
                    break;
                case 22:// trend - dự án sắp thành công
                    query = danhsachduan.Where(g => g.TienDauTuHienTai >= ((g.TienDauTuMucTieu / 4)*3)).ToList();
                    break;
                case 23:// trend - phổ biến - thêm cột view
                    query = danhsachduan.Where(g => g.SoLuotView>1000).ToList();
                    break;
                case 24:// trend - dự án lớn
                    query = danhsachduan.Where(g => g.TienDauTuMucTieu>50000000).ToList();
                    break;
                case 25:// trend - dự án sắp kết thúc
                    query = danhsachduan.Where(g => (g.ThoiGianKetThuc.Value - DateTime.Now).Days <5).ToList();
                    break;
            }

            ketqua.Clear();
            ketqua = query;
        }

        public static void GetDuAn_Other(ref  List<EntityDuAn> ketqua, int loai, int model)
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var danhsachduan = ketqua;
            List<EntityDuAn> query = ketqua;
            int idlogin=0;
            int min = 0;
            int max = 0;
            switch (model)
            {
                case 1: min = 0; max = 5 * 1000000; break;
                case 2: min = 5 * 1000000; ; max = 10 * 1000000; break;
                case 3: min = 10 * 1000000; ; max = 20 * 1000000; break;
                case 4: min = 20 * 1000000; ; max = 100 * 1000000; break;
                case 5: min = 100 * 1000000; ; max = 500 * 1000000; break;
                case 6: min = 500 * 1000000; ; max = int.MaxValue; break;
            }
            switch (loai)
            {
                case 1:// dự án đã đầu tư
                      if (System.Web.HttpContext.Current.Request.Cookies["ftid"] == null)
                      {
                          return;
                      }
                      idlogin = Convert.ToInt16(System.Web.HttpContext.Current.Request.Cookies["ftid"].Value);
                      query = (from op in danhsachduan
                                 join pg in db.EntityDauTus on op.Id equals pg.IdDuAn
                                 where pg.IdUser == idlogin
                                 select op)
                                 .Distinct().ToList();
                    break;
                case 2:// dự án bạn bè đã đầu tư
                    if (System.Web.HttpContext.Current.Request.Cookies["ftid"] == null)
                      {
                          return;
                      }
                     idlogin = Convert.ToInt16(System.Web.HttpContext.Current.Request.Cookies["ftid"].Value);
                     query = (from op in danhsachduan
                              join pg in db.EntityDauTus on op.Id equals pg.IdDuAn
                              where db.EntityTheoDois.Any(es => (es.IdBanBe == pg.IdUser && es.IdUser== idlogin))
                              select op)
                              .Distinct().ToList();
                    break;
                case 3:// đã được tài trợ <5t / 5-10t / 20->100t / 100-500t / >500t
                   
                    query = danhsachduan.Where(g => g.TienDauTuHienTai>= min&& g.TienDauTuHienTai<= max).ToList();
                    break;
                case 4:// trạng thái đã thành công / chưa thành công
                    int trangthai = model == 2 ? 0 : 1;
                    query = danhsachduan.Where(g => g.TrangThaiFund == trangthai).ToList();
                    break;
                case 5:// mục tiêu  <5t / 5-10t / 20->100t / 100-500t / >500t
                    query = danhsachduan.Where(g => g.TienDauTuMucTieu >= min && g.TienDauTuHienTai <= max).ToList();
                    break;
                case 6:// hoàn thành <75% / 75% ->100% / >100%
                    switch (model)
                    {
                        case 1: query = danhsachduan.Where(g => g.TienDauTuHienTai < ((g.TienDauTuMucTieu / 4) * 3)).ToList();
                            break;
                        case 2: query = danhsachduan.Where(g => g.TienDauTuHienTai > ((g.TienDauTuMucTieu / 4) * 3) && (g.TienDauTuHienTai <= g.TienDauTuMucTieu)).ToList();
                            break;
                        case 3: query = danhsachduan.Where(g =>  (g.TienDauTuHienTai > g.TienDauTuMucTieu)).ToList();
                            break;
                    }
                   
                    break;
            }

            ketqua.Clear();
            ketqua = query;
        }

        public static List<EntityDuAn> GetDuAn_TrangChu(int loai, int model)// loai: category hay trend // model: trend bao nhieu hoặc category gì
        {
             dbFirstStepDataContext db = new dbFirstStepDataContext();
             List<EntityDuAn> ketqua = db.EntityDuAns.Where(g=>g.TrangThai== 2).ToList();
             if (loai == 1)// category
             {
                 return ketqua.Where(g => g.IdDanhMuc == model).Take(20).ToList();
             }
             else // trend
             {
                 GetDuAn_Trend(ref ketqua, model);
                 return ketqua.Take(20).ToList() ;
             }
        }
        public static List<EntityDuAn> GetDuAn_Search(List<SearchModel> danhsach,ref List<EntityDuAn> ketqua)// loai: category hay trend // model: trend bao nhieu hoặc category gì
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
          //  List<EntityDuAn> ketqua = db.EntityDuAns.Where(g => g.TrangThai == 1).ToList();
            List<EntityDuAn> ketquacuoi = ketqua;
            foreach (var item in danhsach)
            {
                switch (item.op1)
                {
                    case 1:
                        if (item.op2 > 0)
                        {
                            ketquacuoi = ketqua.Where(g => g.IdDanhMuc == item.op2).ToList();
                            ketqua = ketquacuoi;
                        }
                        break;
                    case 2:
                        if (item.op2 > 0)
                        {
                            GetDuAn_Trend(ref ketqua, item.op2);
                           // ketqua = ketquacuoi;
                        }
                        break;
                    case 3:
                            GetDuAn_Other(ref ketqua, item.op2, item.op3);
                        break;
                }
            }
            return ketqua;
        }
    }
}