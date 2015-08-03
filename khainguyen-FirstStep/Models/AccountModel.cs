using MvcLibrary.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace khainguyen_FirstStep.Models
{
    public class SettingAccountModel
    {
        
    }
    public class LoginHistoryModel
    {
       public string hoatdong = "";
       public string date = "";
       public string location = "";
       public string Ipaddress = "";
       
        public static List<LoginHistoryModel> get_HistoryModel(string noidung)
        {
            List<LoginHistoryModel> ketqua=new List<LoginHistoryModel> ();
            if (noidung == "" || noidung==null)
                return ketqua;
            string [] mang = noidung.Split('#');
            for (int i = 0; i < mang.Length - 1; i++)
            {
                string[] item = mang[i].Split('*');
                if (item.Length == 4)
                {
                    LoginHistoryModel l = new LoginHistoryModel();
                    l.hoatdong = item[0];
                    l.date = item[1];
                    l.location = item[2];
                    l.Ipaddress = item[3];
                    ketqua.Add(l);
                }
            }
                return ketqua;

        }
        public static string set_HistoryModel(string noidung, LoginHistoryModel itemnew)
        {
            string ketqua = "";
            ketqua = noidung + itemnew.hoatdong + "*" + itemnew.date + "*" + itemnew.location + "*" + itemnew.Ipaddress + "#";
            return ketqua;

        }
    }
    public class AccountModel
    {
        [Required(ErrorMessage = "Bạn vui lòng nhập họ tên !")]
        [StringLength(50, ErrorMessage = "Tên khoảng {2}-{1} kí tự", MinimumLength = 2)]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Bạn vui lòng nhập Email !")]
        [EmailAddress(ErrorMessage = "Định dạng Email không đúng !")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được rỗng !")]
        [RegularExpression(@"^((?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,20})$", ErrorMessage = "Mật khẩu phải vừa số, vừa chữ và có 1 kí tự viết hoa. (6 - 20 kí tự)")]
        public string Pass { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu nhập lại không được rỗng !")]
        [Compare("Pass", ErrorMessage = "Mật khẩu không khớp")]
        public string RePass { get; set; }

        public string GioiThieu { get; set; }
        public string DiaDiem { get; set; }
        public string Website { get; set; }
        public string Avatar { get; set; }
        public string Profile { get; set; }

        public static void DangKy(AccountModel dangky)
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            Security ser = new Security();
            string passHex = ser.GetHashPassword(dangky.Pass);
            string mailHex = ser.GetHashPassword(dangky.Email);
            EntityUser ban = new EntityUser();
            ban.TrangThai = 0;
            ban.HasCode = mailHex;
            ban.HoTen = dangky.HoTen;    
            ban.Email = dangky.Email;
            ban.Pass = passHex;
            string[] mang = HttpContext.Current.Request.Url.AbsoluteUri.ToString().Split('/');
            string url = mang[0] + "//" + mang[2];
            ban.Avatar = url+"/Content/Images/Avatar/ava0.jpg";
            ban.VanityURL = mailHex;//

            db.EntityUsers.InsertOnSubmit(ban);
            db.SubmitChanges();
        }

    }
}