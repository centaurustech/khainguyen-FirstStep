using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace khainguyen_FirstStep.Models
{
    public class DetailHoatDongModel
    {
        public string Avatar = "";
        public string TenNguoiDung = "";
        public string TenDuAn = "";
        public string HinhDuAn = "";
        public int IdDuAn = 0;
        public string VanityURL = "";
        public string IdFacebook = "";

        public string Loai = "";
        public string Date = "";
    }

    public class HoatDongModel
    {
        public string IdUser = "";
        public string IdBanBe = "";
        public string Loai = "";
        public string IdDuAn = "";
        public string Date = "";

        public static List<HoatDongModel> get_HoatDongModel_Mode(string noidung,int IdUserRemove)
        {
            List<HoatDongModel> ketqua = new List<HoatDongModel>();
            if (noidung == "" || noidung == null)
                return ketqua;
            string[] mang = noidung.Split('#');
            for (int i = 0; i < mang.Length - 1; i++)
            {
                string[] item = mang[i].Split('*');
                if (item[1].ToString() != IdUserRemove.ToString() && item[0].ToString() != IdUserRemove.ToString())
                {
                    if (item.Length == 5)
                    {
                        HoatDongModel l = new HoatDongModel();
                        l.IdUser = item[0];
                        l.IdBanBe = item[1];
                        l.Loai = item[2];
                        l.IdDuAn = item[3];
                        l.Date = item[4];
                        ketqua.Add(l);
                    }
                }
            }
            return ketqua;

        }
        public static List<HoatDongModel> get_HoatDongModel(string noidung)
        {
            List<HoatDongModel> ketqua = new List<HoatDongModel>();
            if (noidung == "" || noidung == null)
                return ketqua;
            string[] mang = noidung.Split('#');
            for (int i = 0; i < mang.Length - 1; i++)
            {
                string[] item = mang[i].Split('*');
                if (item.Length == 5)
                {
                    HoatDongModel l = new HoatDongModel();
                    l.IdUser = item[0];
                    l.IdBanBe = item[1];
                    l.Loai = item[2];
                    l.IdDuAn = item[3];
                    l.Date = item[4];
                    ketqua.Add(l);
                }
            }
            return ketqua;

        }
        public static string set_HoatDongModel(string noidung, HoatDongModel itemnew)
        {
            string ketqua = "";
            ketqua = noidung + itemnew.IdUser + "*" + itemnew.IdBanBe + "*" + itemnew.Loai + "*" + itemnew.IdDuAn +"*"+itemnew.Date+ "#";
            return ketqua;

        }

        //lấy danh sách hoạt động
        public static List<HoatDongModel> getListHoatDongModel(EntityUser user)
        {
            string nhatkyhoatdong = user.NhatKyHoatDong!= null?user.NhatKyHoatDong.ToString():"";
            List<HoatDongModel> history = get_HoatDongModel(nhatkyhoatdong);
            history.Reverse();
            return history;
        }

        // add 1 hoạt động
        public static void AddHoatDong(int IdUser, int IdBanBe, int Loai, int IdDuAn)
        {
            var db = new dbFirstStepDataContext();

            EntityUser user = db.EntityUsers.Where(g => g.Id == IdBanBe).First();

            HoatDongModel hoatdong = new HoatDongModel();
            hoatdong.IdUser = IdUser.ToString();
            hoatdong.IdBanBe = IdBanBe.ToString();
            hoatdong.Loai = Loai.ToString();
            hoatdong.IdDuAn = IdDuAn.ToString();
            hoatdong.Date = DateTime.Now.ToString();
            string nhatkyhoatdong = user.NhatKyHoatDong!= null?user.NhatKyHoatDong.ToString():"";
            nhatkyhoatdong = set_HoatDongModel(nhatkyhoatdong, hoatdong);

            int soluonghoatdong = user.SoLuongHoatDong==null?0:user.SoLuongHoatDong.Value;
            soluonghoatdong++;
            user.SoLuongHoatDong = soluonghoatdong;
            user.NhatKyHoatDong = nhatkyhoatdong;

            db.SubmitChanges();
        }
       // add nhiều hoạt động
        public static void GetListFriend_Sent_HoatDong(int idlogin, int IdDuAn, int Loai)
        {
            var db = new dbFirstStepDataContext();
            var danhsachtheodoi = db.EntityTheoDois.Where(g => g.IdBanBe == idlogin).ToList();
            foreach (var item1 in danhsachtheodoi)
            {
                AddHoatDong(idlogin, item1.IdUser, Loai, IdDuAn);
            }
        }

        public static DetailHoatDongModel Convert(HoatDongModel model)
        {
            DetailHoatDongModel ketqua = new DetailHoatDongModel();
            try
            {
                ketqua.Loai = model.Loai;
                ketqua.IdDuAn =int.Parse(model.IdDuAn);
                ketqua.Date = model.Date;

                dbFirstStepDataContext db = new dbFirstStepDataContext();

                EntityUser user = db.EntityUsers.Where(g => g.Id.ToString() == model.IdUser).First();
               
                EntityDuAn duan = db.EntityDuAns.Where(g => g.Id.ToString() == model.IdDuAn).FirstOrDefault();
                if (duan != null)
                {
                    ketqua.HinhDuAn = duan.HinhAnh;
                    ketqua.TenDuAn = duan.TenDuAn;
                }

                ketqua.Avatar = user.Avatar;
                ketqua.TenNguoiDung = user.HoTen;
                ketqua.VanityURL = user.VanityURL;
                ketqua.IdFacebook = user.IdFacebook;
               
                
            }
            catch { }
            return ketqua;
        }

        // remove hoạt động của user => ra khỏi danh sách hoạt động
        public static void RemoveHoatDong(int IdUser, int IdUserRemove)//người gởi- người nhận - 
        {
            using (dbFirstStepDataContext db = new dbFirstStepDataContext())
            {
                EntityUser user = db.EntityUsers.Where(g => g.Id == IdUser).First();
                string nhatkyhoatdong = user.NhatKyHoatDong != null ? user.NhatKyHoatDong.ToString() : "";
                List<HoatDongModel> history = get_HoatDongModel_Mode(nhatkyhoatdong, IdUserRemove);
                string nhatky = "";
                foreach (var item in history)
                {
                    nhatky = set_HoatDongModel(nhatky, item);
                }
                user.NhatKyHoatDong = nhatky;
                db.SubmitChanges();
              //  history.Reverse();
            }
          //  return history;
        }
    }
}