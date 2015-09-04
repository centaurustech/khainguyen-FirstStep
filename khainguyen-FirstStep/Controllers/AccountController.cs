using Facebook;
using khainguyen_FirstStep.Models;
using MvcLibrary.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace khainguyen_FirstStep.Controllers
{
    public class AccountController : Controller
    {
        dbFirstStepDataContext db = new dbFirstStepDataContext();
        //
        // GET: /Account/
        #region đăng nhập thường , đăng xuất
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult LoginLife(string link)
        {
            ViewBag.link = link;
            return View();
        }
        [HttpPost]
        public ActionResult Checkpass(string link)
        {
            ViewBag.link = link;
            return View();
        }
        public ActionResult Login(string trolai)
        {
            if (Request.Cookies["ftid"] != null)
            {
                return RedirectToAction("Index", "Home");
                //return View();
            }
            else
            {
                if (trolai != null)
                {
                    string[] mang = Request.Url.AbsoluteUri.ToString().Split('/');
                    ViewBag.UrlReferrer = mang[0] + "//" + mang[2] + "/" + trolai;
                }
                else
                {
                    if (Request.UrlReferrer != null)
                        ViewBag.UrlReferrer = Request.UrlReferrer.ToString();
                    else
                        ViewBag.UrlReferrer = "http://firststep.vn";
                }
                return View();
            }
        }
        public ActionResult FormLogin()
        {
            return PartialView();
        }
        public string CheckPassLife(string password)
        {
            using (dbFirstStepDataContext db1 = new dbFirstStepDataContext())
            {
                int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
                EntityUser user = db1.EntityUsers.Where(g => g.Id == idlogin).FirstOrDefault();
                if (user == null)
                {
                    return "f";
                }
                else
                {
                    Security ser = new Security();
                    string passHex = ser.GetHashPassword(password);
                    if (user.Pass == null || user.Pass == "")
                    {
                        user.Pass = passHex;
                        db.SubmitChanges();
                        Session["fsduytrihoatdong"] = "1";
                        return "t";
                    }
                    if (user.Pass == passHex)
                    {
                        Session["fsduytrihoatdong"] = "1";
                        return "t";
                    }
                    else
                        return "f";
                }
            }
        }
        public bool CheckPassDonGian(string password)
        {
            if (password == "2903")
            {
                Session["fstaoduan"] = "1";
                return true;
            }
            else
            {
                return false;
            }
        }
        public JsonResult CheckId(string username, string password, string remember)
        {
            EntityUser user = new EntityUser();
            user.Email = " ";
            user.TrangThai = 0;
            try
            {
                dbFirstStepDataContext db = new dbFirstStepDataContext();

                Security ser = new Security();
                string passHex = ser.GetHashPassword(password);

                var admin = db.EntityUsers.Where(t => t.Email == username);
                if (admin.Count() == 0)
                {
                    user.Email = "Khong ton tai";
                    return Json(user);
                }
                else if (admin.First().Pass != passHex)
                {
                    user.Email = "Khong dung pass";
                    return Json(user);
                }
                else if (admin.First().TrangThai != 1)
                {
                    user.Email = "Chua kich hoat";
                    return Json(user);
                }
                else if (admin.First().TrangThai == 1)
                {
                    createnew_LoginHistory("Đã đăng nhập ", admin.First().Id);
                    Response.Cookies["ftid"].Value = admin.First().Id.ToString();
                    Response.Cookies["ftusername"].Value = admin.First().Email;
                    Response.Cookies["fthoten"].Value = HttpUtility.UrlEncode(admin.First().HoTen); //admin.HoTen;
                    Response.Cookies["ftavatar"].Value = admin.First().Avatar;
                    Response.Cookies["ftidFacebook"].Value = admin.First().IdFacebook;
                    if (remember == "false")
                    {
                        Response.Cookies["ftid"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["ftavatar"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["ftusername"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["fthoten"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["ftidFacebook"].Expires = DateTime.Now.AddDays(1);
                    }
                    else
                    {
                        Response.Cookies["ftid"].Expires = DateTime.Now.AddDays(30);
                        Response.Cookies["ftavatar"].Expires = DateTime.Now.AddDays(30);
                        Response.Cookies["ftusername"].Expires = DateTime.Now.AddDays(30);
                        Response.Cookies["fthoten"].Expires = DateTime.Now.AddDays(30);
                        Response.Cookies["ftidFacebook"].Expires = DateTime.Now.AddDays(1);
                    }

                    user.Email = "ok";
                    Session["fsduytrihoatdong"] = "1";
                    return Json(user);
                }
                else
                {
                    return Json(user);
                }

            }
            catch
            {
                return Json(user);
            }
        }
        public ActionResult Logout()
        {

            var x = new HttpCookie("ftid");
            createnew_LoginHistory("Đã đăng xuất ", int.Parse(System.Web.HttpContext.Current.Request.Cookies["ftid"].Value));
            x.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(x);

            var c = new HttpCookie("ftusername");
            c.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(c);

            var a = new HttpCookie("fthoten");
            a.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(a);

            var b = new HttpCookie("ftavatar");
            b.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(b);

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region "login Facebook"
        [AllowAnonymous]
        public ActionResult Facebook()
        {
            //if (trolai != null)
            //{
            //    string[] mang = Request.Url.AbsoluteUri.ToString().Split('/');
            //    ViewBag.UrlReferrer = mang[0] + "//" + mang[2] + "/" + trolai;
            //}
            //else
            //{
            //    if (Request.UrlReferrer != null)
            //        ViewBag.UrlReferrer = Request.UrlReferrer.ToString();
            //    else
            //        ViewBag.UrlReferrer = "http://firststep.vn";
            //}
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "570964363013096",
                client_secret = "d6edfe3df4e121a296e8e542a8932154",
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);

                uriBuilder.Query = null;

                uriBuilder.Fragment = null;

                uriBuilder.Path = Url.Action("FacebookCallback");

                return uriBuilder.Uri;

            }
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new

            {

                client_id = "570964363013096",

                client_secret = "d6edfe3df4e121a296e8e542a8932154",

                redirect_uri = RedirectUri.AbsoluteUri,

                code = code

            });
            var accessToken = result.access_token;

            //Luu access token ma fb tra ve vao session
            Session["AccessToken"] = accessToken;

            fb.AccessToken = accessToken;

            var urlreturn = result.AbsoluteUri;



            //var friendListData = fb.Get("/me/friends?fields=first_name,id");
            //JObject friendListJson = JObject.Parse(friendListData.ToString());

            ////List<FbUser> fbUsers = new List<FbUser>();
            //foreach (var friend in friendListJson["data"].Children())
            //{
            //    string kkk = friend["id"].ToString().Replace("\"", "");

            //}
            dynamic info = fb.Get("me?fields=first_name,last_name,id,email,picture");
            string email = info.email;
            string HoTen = info.first_name + " " + info.last_name;
            string IdFacebook = info.id;
            //{"data":{"url":"https://fbcdn-profile-a.akamaihd.net/hprofile-ak-ash2/1119295_100000887034532_1079608060_q.jpg","is_silhouette":false}}
            //"{\"data\":{\"is_silhouette\":false,\"url\":\"https://fbcdn-profile-a.akamaihd.net/hprofile-ak-xaf1/t1.0-1/c34.34.422.422/s50x50/425336_196174787203821_1477323958_n.jpg\"}}"
            string picture = info.picture.ToString();
            string[] arr1 = picture.Split('"');
            foreach (var item in arr1)
            {
                if (item.ToString().IndexOf("http") != -1)
                {
                    picture = item.ToString();
                    break;
                }
            }
         //   string[] arr2 = arr1[3].Split('"');
           // picture = "https:" + arr2[0];
            if (email != null)
            {
                FormsAuthentication.SetAuthCookie(email, false);
                // luu vao CSDL 111
                dbFirstStepDataContext db = new dbFirstStepDataContext();
                Security ser = new Security();
                EntityUser user = new EntityUser();
                var query = from p in db.EntityUsers
                            where p.Email == email
                            select p;
                if (query.Count() > 0)
                {
                    if (query.First().TrangThai == 0)
                    {
                    
                    }
                    if (query.First().TrangThai == 1)
                    {
                       // query.First().HoTen = HoTen;
                       // user.Email = email;
                       // query.First().Avatar = picture;
                        // user.HasCode = ser.GetHashPassword(email);
                        // user.Loai = 1;
                        //user.TrangThai = 1;
                        // user.Date = DateTime.Now;
                        query.First().IdFacebook = IdFacebook;
                        db.SubmitChanges();
                        DangNhapFB(email);
                       // return RedirectToAction("Index", "Home");
                        if (ViewBag.UrlReferrer != null)
                        {
                            string url = ViewBag.UrlReferrer;
                            if(url.IndexOf("Dang-Nhap") != -1)
                                return RedirectToAction("Index", "Home");
                            else return Redirect(url);
                        }
                        else
                        {
                            if (Request.UrlReferrer != null && Request.UrlReferrer.ToString().Contains("facebook") == false)
                            {
                                if (Request.UrlReferrer.ToString().IndexOf("Dang-Nhap") != -1)
                                    return RedirectToAction("Index", "Home");
                                else return Redirect(Request.UrlReferrer.ToString());
                                //else return Redirect(result.AbsoluteUri);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                    else
                    {
                        return RedirectToAction("BanFacebook", "LoginAccount");
                    }
                }
                else
                {
                    user.HoTen = HoTen;
                    user.Email = email;
                    user.Avatar = picture;
                    user.IdFacebook = IdFacebook;
                    user.TrangThai = 1;

                    string HasCode = ser.GetHashPassword(user.Email);
                    user.VanityURL = HasCode;

                    db.EntityUsers.InsertOnSubmit(user);
                    user.Date = DateTime.Now;
                    db.SubmitChanges();
                    DangNhapFB(email);
                    if (ViewBag.UrlReferrer != null)
                    {
                        return Redirect(ViewBag.UrlReferrer);
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Message = "Không thể lấy được email từ facebook của bạn";
                return Content("<script language='javascript' type='text/javascript'>alert('Không thể lấy được email từ facebook của bạn.'); window.location = '/Tai-Khoan/Dang-Nhap';</script>");
            }
        }
        public void DangNhapFB(string email)
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var query = from p in db.EntityUsers
                        where p.Email == email
                        select p;
            if (query.Count() > 0)
            {
                createnew_LoginHistory("Đã đăng nhập ", query.First().Id);
                Response.Cookies["ftid"].Value = query.First().Id.ToString();
                Response.Cookies["ftusername"].Value = query.First().Email;
                Response.Cookies["fthoten"].Value = HttpUtility.UrlEncode(query.First().HoTen);
                //Session["HoTen"] = query.First().HoTen;
                Response.Cookies["ftavatar"].Value = query.First().Avatar;
                Response.Cookies["ftidfacebook"].Value = query.First().IdFacebook;
                //Response.Cookies["TTBUserType"].Value = "1";

                Response.Cookies["ftid"].Expires = DateTime.Now.AddDays(30);
                Response.Cookies["ftusername"].Expires = DateTime.Now.AddDays(30);
                Response.Cookies["fthoten"].Expires = DateTime.Now.AddDays(30);
                Response.Cookies["ftavatar"].Expires = DateTime.Now.AddDays(30);
                //Response.Cookies["TTBUserType"].Expires = DateTime.Now.AddDays(30);
                Session["fsduytrihoatdong"] = "1";

            }
        }
        #endregion 

        #region "đăng ký"
        public ActionResult DangKy()
        {
            AccountModel tnew = new AccountModel();
            return View(tnew);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult DangKy(AccountModel Mtnew)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbFirstStepDataContext db = new dbFirstStepDataContext();
                    var query = from p in db.EntityUsers
                                where p.Email == Mtnew.Email
                                select p;
                    if (query.Count() > 0)
                    {
                        return RedirectToAction("TrungEmail", "Account");
                    }
                    else
                    {
                        #region "Add new user"
                        Security ser = new Security();
                        string passHex = ser.GetHashPassword(Mtnew.Pass);
                        string mailHex = ser.GetHashPassword(Mtnew.Email);
                        EntityUser ban = new EntityUser();
                        ban.TrangThai = 0;
                        ban.HasCode = mailHex;
                        ban.HoTen = Mtnew.HoTen;
                        ban.Email = Mtnew.Email;
                        ban.Pass = passHex;
                        string[] mang = Request.Url.AbsoluteUri.ToString().Split('/');
                        string url = mang[0] + "//" + mang[2];
                        ban.Avatar = url + "/Content/Images/Avatar/ava0.jpg";
                        ban.VanityURL = mailHex;//
                        #endregion

                        string HoTen = Mtnew.HoTen;
                        string Email = Mtnew.Email;                        
                        //GuiMailDangKy(HoTen,Email,HasCode);
                        MailHelper.SendMail_DangKy(HoTen, Email, url + "/account/kichhoat?HasCode=" + ban.HasCode);                        
                        db.EntityUsers.InsertOnSubmit(ban);
                        db.SubmitChanges();
                        return RedirectToAction("DangKyThanhCong", "Account");
                    }
                }
            }
            catch
            {                
            }
            return View(Mtnew);
        }

        public ActionResult TrungEmail()
        {
            return View();
        }
        public ActionResult DangKyThanhCong()
        {
            return View();
        }        
        public void GuiMailDangKy(string HoTen, string Email, string HasCode)
        {
                //StringBuilder mailBody = new StringBuilder();
                //mailBody.AppendFormat("<h1>[FirstStep]-Kích hoạt tài khoản</h1>");
                //mailBody.AppendFormat("<br />");
                //mailBody.AppendFormat("<p> Chào : " + " " + HoTen + "   !  " + "</p>");
                //mailBody.AppendFormat("<p>Email đăng ký tài khoản :  " + " " + Email + " " + "</p>");
                //string[] mang = Request.Url.AbsoluteUri.ToString().Split('/');
                //string url = mang[0] + "//" + mang[2];
                //mailBody.AppendFormat("<p>Link kích hoạt tài khoản:  " +url+ "/account/kichhoat?HasCode="+HasCode+ " " + "</p>");
                //mailBody.AppendFormat("<br></br>");
                //mailBody.AppendFormat("<p>Không trả lời thư vào email này ! Xin cảm ơn.</p>");
                //MailHelper.SendMailMessage("firststep.system.info@gmail.com", Email, null, null, "[FirstStep]-Kích hoạt tài khoản", mailBody.ToString(), "smtp.gmail.com", true, "firststep.system.info@gmail.com", "!@#Hien4567");                           
        }
        public ActionResult kichhoat(string HasCode)
        {
            try
            { 
                if (HasCode != null)
                {
                    dbFirstStepDataContext db = new dbFirstStepDataContext();
                    var item = db.EntityUsers.Where(p => p.HasCode == HasCode).First();
                    if (item.TrangThai == 0)
                    {
                        item.TrangThai = 1;
                        db.SubmitChanges();
                        createnew_LoginHistory("Đã đăng nhập ", item.Id);
                        Response.Cookies["ftid"].Value = item.Id.ToString();
                        Response.Cookies["ftusername"].Value = item.Email;
                        Response.Cookies["fthoten"].Value = HttpUtility.UrlEncode(item.HoTen); //admin.HoTen;
                        Response.Cookies["ftavatar"].Value = item.Avatar;
                        Response.Cookies["ftidFacebook"].Value = item.IdFacebook;
                        Session["fsduytrihoatdong"] = "1";
                        return RedirectToAction("SuaThongTin", "Account");
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult KichHoatThanhCong()
        {
            if (Request.Cookies["ftid"] != null)
            {
                RedirectToAction("Index", "Home");
            }
            return View();
        }
        public ActionResult kichhoat_team(string HasCode)
        {
            try
            {
                dbFirstStepDataContext db = new dbFirstStepDataContext();
                var nhom = db.EntityNhomChienDiches.Where(g => g.EntityUser.HasCode == HasCode).First(); ;
                // string linkduan = Request.Url.AbsoluteUri.Split('/')[2] + Url.Action("ChiTietDuAn", "DuAn", new { Title = Utilities.Paste_Int64(Utilities.Encode(duan.TenDuAn.ToString()), duan.Id) });

                if (nhom.TrangThai == false)
                {
                    nhom.TrangThai = true;
                    db.SubmitChanges();
                }
                return RedirectToAction("ChiTietDuAn", "DuAn", new { Title = Utilities.Paste_Int64(Utilities.Encode(nhom.EntityDuAn.TenDuAn.ToString()), nhom.EntityDuAn.Id) });
            }
            catch {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        #region quên mật khẩu
         public ActionResult QuenMatKhau()
        {
            AccountModel tnew = new AccountModel();
            return View(tnew);
        }
         [ValidateInput(false)]
         [HttpPost]
         public ActionResult QuenMatKhau(AccountModel Mtnew)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     dbFirstStepDataContext db = new dbFirstStepDataContext();
                     var query = from p in db.EntityUsers
                                 where p.Email == Mtnew.Email
                                 select p;
                     if (query.Count() == 0)
                     {
                         return RedirectToAction("DaGuiEmailQuenMK", "Account");
                     }
                     else
                     {
                         //string Pass = GenerateRandomText();
                         string Pass = "Pas1hahahaafs";
                         Security ser = new Security();
                         string Passhex = ser.GetHashPassword(Pass);
                         query.First().Pass = Passhex;
                         db.SubmitChanges();
                         string HoTen = query.First().HoTen;
                         string Email = Mtnew.Email;
                         string HasCode = Pass;
                         GuiMailQuenMK(HoTen, Email, HasCode);
                         return RedirectToAction("DaGuiEmailQuenMK", "Account");
                     }
                 }
             }
             catch
             {

                 return RedirectToAction("Index", "Error", new { errorMsg = "Lỗi SQL hosting / server" });
             }
             return View(Mtnew);
         }
        public static string GenerateRandomText()
        {
            int textLength = 10;
            const string Chars = "ABCDEFGHIJKLMNPOQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(Chars, textLength)
                    .Select(s => s[random.Next(s.Length)])
                    .ToArray());
            return result;
        }
        public void GuiMailQuenMK(string HoTen, string Email, string HasCode)
        {
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("<h1>[FirstStep]-Lấy lại mật khẩu tài khoản</h1>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat("<p> Chào : " + " " + HoTen + "   !  " + "</p>");
            mailBody.AppendFormat("<p>Email đăng ký tài khoản :  " + " " + Email + " " + "</p>");
            mailBody.AppendFormat("<p>Password đã được cung cấp mới:  " +  HasCode + " " + "</p>");
            mailBody.AppendFormat("<br></br>");
            mailBody.AppendFormat("<p>Không trả lời thư vào email này ! Xin cảm ơn.</p>");
            MailHelper.SendMailMessage("firststep.system.info@gmail.com", Email, null, null, "[FirstStep]-Lấy lại mật khẩu tài khoản", mailBody.ToString(), "smtp.gmail.com", true, "firststep.system.info@gmail.com", "!@#Hien4567");
        }
        public ActionResult DaGuiEmailQuenMK()
        {
            return View();
        }
        #endregion

        #region Quản lý tài khoản 
        public ActionResult _MenuTaiKhoan()
        {
            return PartialView();
        }

      

        #region "lấy địa chỉ ip - convert to city nam - create new login history"
        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        public string GetLocation(string ip)
        {
            string data = "";
            using (WebClient client = new WebClient())
            {
                try
                {
                  //  ip = "1.53.229.165";
                    byte[] ar = client.DownloadData("http://freegeoip.net/json/"+ip);
                    string str = System.Text.Encoding.UTF8.GetString(ar);
                    string[] mang = str.Split(',');
                    string tennuoc = mang[1].Split(':')[1].Replace("\"","");
                    string tentp = mang[5].Split(':')[1].Replace("\"", "");
                    data = tentp + ", " + tennuoc;
                }
                catch
                {
                    
                }
            }
            return data;
        }
        public void createnew_LoginHistory(string ten, int iduser)
        {
            LoginHistoryModel model1 = new LoginHistoryModel ();
            model1.hoatdong = ten;
            model1.date = Utilities.GetDate_Timezone() + " " + Utilities.GetClientTimeOffset()+":00";
            
            model1.Ipaddress=GetIPAddress();
            model1.location = GetLocation(model1.Ipaddress);
            EntityUser user = db.EntityUsers.Where(g=>g.Id== iduser).FirstOrDefault();
            string lichsu = user.Login_History;
            string ketqua = LoginHistoryModel.set_HistoryModel(lichsu, model1);
            user.Login_History = ketqua;
            db.SubmitChanges();
        }
        public List<LoginHistoryModel> getAccount(EntityUser user)
        {
            List<LoginHistoryModel> history = LoginHistoryModel.get_HistoryModel(user.Login_History);
            history.Reverse();
            return history;
        }
        #endregion 

        public ActionResult DoiMatKhau()
        {           
           int idlogin = 0;
           if (Request.Cookies["ftid"] != null)
           {
               idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
               EntityUser user = db.EntityUsers.Where(g => g.Id == idlogin).FirstOrDefault();
               ViewBag.history = getAccount(user);
               ViewBag.User = user;
               AccountModel tnew = new AccountModel();
               return View(tnew);
           }
           else
           {
               return RedirectToAction("Login", "Account", new { trolai = "tuy-chinh-khac/tai-khoan"});
           }
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult DoiMatKhau(AccountModel Mtnew)
        {
            try
            {
                dbFirstStepDataContext db = new dbFirstStepDataContext();
              
                var query = from p in db.EntityUsers
                                where p.Email == Mtnew.Email
                                select p;
              
                if (query.Count() == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.history = getAccount(query.First());
                    ViewBag.User = query.First();
                    if (ModelState.IsValid)
                    {
                        Security ser = new Security();
                        string Passhex = ser.GetHashPassword(Mtnew.Pass);
                        query.First().Pass = Passhex;
                        db.SubmitChanges();
                        createnew_LoginHistory("Đã đổi mật khẩu", query.First().Id);
                        //Logout();
                        //return RedirectToAction("Login", "Account");
                    }
                }

                //if (ModelState.IsValid)
                //{
                //    var query = from p in db.EntityUsers
                //                where p.Email == Mtnew.Email
                //                select p;
                //    if (query.Count() == 0)
                //    {
                //        return RedirectToAction("Index", "Home");
                //    }
                //    else
                //    {
                //        Security ser = new Security();
                //        string Passhex = ser.GetHashPassword(Mtnew.Pass);
                //        query.First().Pass = Passhex;
                //        db.SubmitChanges();
                //        createnew_LoginHistory("Đã đổi mật khẩu", query.First().Id);
                //        Logout();
                //        return RedirectToAction("Login", "Account");
                //    }
                //}
            }
            catch
            {

                return RedirectToAction("Index", "Error", new { errorMsg = "Lỗi SQL hosting / server" });
            }
            return View(Mtnew);
        }

        // sua thong tin ca nhan
        public ActionResult SuaThongTin()
        {
            if(Request.Cookies["ftusername"] != null)
            {
                AccountModel sua = new AccountModel();
                dbFirstStepDataContext db = new dbFirstStepDataContext();
                var item = db.EntityUsers.Where(p => p.Email == Request.Cookies["ftusername"].Value).First();
                sua.HoTen = item.HoTen;
                sua.GioiThieu = item.GioiThieu;
                sua.Avatar = item.Avatar;
                sua.DiaDiem = item.DiaDiem;
                sua.Website = item.Website;
                string[] mang = Request.Url.AbsoluteUri.ToString().Split('/');
                string url = mang[0] + "//" + mang[2];
                ViewBag.Link = url + "/user/";
                sua.Profile = url + "/user/"+item.VanityURL;
                return View(sua);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { trolai = "tuy-chinh-khac/thong-tin-ca-nhan" });
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SuaThongTin(AccountModel Mtnew)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                dbFirstStepDataContext db = new dbFirstStepDataContext();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase hpf = Request.Files[i];
                    if (hpf.FileName != "")
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

                        //ImageHelper imgHelper = new ImageHelper();
                        //string encodestring = imgHelper.encodeImageFile(hpf);
                        //ava.Avatar = "/Content/Images/Avatar/" + encodestring;
                        //db.SubmitChanges();
                        //if (encodestring == "!")
                        //    return RedirectToAction("Error", "Home", new { errorMsg = "Can't upload Images" });
                        //var path = Path.Combine(Server.MapPath("~/Content/Images/Avatar"), encodestring);
                        //hpf.SaveAs(path);
                        //Response.Cookies["ftavatar"].Value = ava.Avatar;
                    }
                }

                var query = from p in db.EntityUsers
                            where p.Email == Request.Cookies["ftusername"].Value
                            select p;
                if (query.Count() == 0)
                {
                    return RedirectToAction("Logout", "Account");
                }
                else
                {
                    query.First().HoTen = Mtnew.HoTen;
                    query.First().GioiThieu = Mtnew.GioiThieu;
                    query.First().DiaDiem = Mtnew.DiaDiem;
                    query.First().Website = Mtnew.Website;
                    string[] mang = Mtnew.Profile.Split('/');
                    string kt = mang[mang.Count() - 1].ToString().Replace("user/", "");
                    kt = Utilities.Encode(kt);
                    if (db.EntityUsers.Any(g => g.VanityURL == kt) == false)
                        query.First().VanityURL = kt;

                    db.SubmitChanges();
                    return RedirectToAction("SuaThongTin", "Account");
                    //  View(Mtnew);
                }
                //}
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { errorMsg = "Lỗi SQL hosting / server" });
            }

            //string[] mang1 = Request.Url.AbsoluteUri.ToString().Split('/');
            //string url1 = mang1[0] + "//" + mang1[2];
            //ViewBag.Link = url1 + "/user/";

            //return View(Mtnew);
        }

      
        #endregion

        public String SaveAccountPayoo(String username)
        {
            String result = "";
            try
            {
                dbFirstStepDataContext db = new dbFirstStepDataContext();
                int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
                EntityUser user = db.EntityUsers.Where(g => g.Id == idlogin).FirstOrDefault();
                user.TKPayoo = username;
                db.SubmitChanges();
                result = "True";
            }
            catch (Exception ex)
            {
                result = "False";
                throw ex;
            }
            return result;
        }


    }
}
