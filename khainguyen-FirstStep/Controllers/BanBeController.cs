using Facebook;
using khainguyen_FirstStep.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace khainguyen_FirstStep.Controllers
{
    public class BanBeController : Controller
    {
        //
        // GET: /BanBe/

        public ActionResult _Index()
        {
            return PartialView ();
        }
        public ActionResult TimBan()
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var item = db.EntityTheoDois.Where(p => p.IdUser == Convert.ToInt16(Request.Cookies["ftid"].Value) && p.Loaifb == 1).ToList();
            return View(item);
        }
        public ActionResult DuAn()
        {
            return View();
        }
        public ActionResult DangTheoDoi()// mình theo dõi người ta
        {
            int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var item = db.EntityTheoDois.Where(p => p.IdUser == idlogin && p.TrangThai == 1).ToList();
            return View(item);
        }
        public ActionResult BiTheoDoi() // người ta theo dõi mình
        {
            int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var item = db.EntityTheoDois.Where(p => p.IdBanBe == idlogin && p.TrangThai==1).ToList();

            return View(item);
        }
        public ActionResult KhoaBan() // những người theo dõi bị khóa
        {
            int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var item = db.EntityTheoDois.Where(p => p.IdBanBe == idlogin && p.TrangThai == 0).ToList();
            return View(item);
        }



        #region Tìm bạn trên facebook 
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);

                uriBuilder.Query = null;

                uriBuilder.Fragment = null;

                uriBuilder.Path = Url.Action("Index");
                return uriBuilder.Uri;

            }
        }
        public ActionResult Index1()
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var loai = db.EntityUsers.Where(p => p.Id == Convert.ToInt16(Request.Cookies["ftid"].Value)).First();
            if (loai.IdFacebook != null)
            {

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
            else
            {
                return RedirectToAction("TimBan", "BanBe");
            }
          
        }
        public ActionResult Index(string code)
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

            FacebookFriendsModel friends = new FacebookFriendsModel();

            var client = new FacebookClient(Session["accessToken"].ToString());
            dynamic fbresult = client.Get("me/friends");
            var data = fbresult["data"].ToString();

            friends.friendsListing = JsonConvert.DeserializeObject<List<FacebookFriend>>(data);

            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var item = db.EntityUsers.ToList();
            IList<EntityUser> banchung = new List<EntityUser>();

            foreach (var ds in friends.friendsListing)
            {
                var ban = item.Where(g => g.IdFacebook == ds.id).ToList();
                foreach (var item2 in ban)
                {
                    var daco = db.EntityTheoDois.Where(t => t.IdUser == Convert.ToInt16(Request.Cookies["ftid"].Value) && t.IdBanBe == item2.Id).ToList();
                    if (daco.Count() == 0)
                    {
                        banchung.Add(item2);
                        EntityTheoDoi dulieu = new EntityTheoDoi();
                        dulieu.IdUser = Convert.ToInt16(Request.Cookies["ftid"].Value);
                        dulieu.IdBanBe = item2.Id;
                        dulieu.TrangThai = 0;
                        dulieu.Loaifb = 1;
                        db.EntityTheoDois.InsertOnSubmit(dulieu);
                        db.SubmitChanges();
                    }
                }
            }
            return RedirectToAction("TimBan", "BanBe");
        }
        #endregion

        #region theo dõi 

        #region "bc làm"
        // follow ban be fb
        public string Follow(int IdBanBe)// một là thêm bạn mới
        {
            try
            {
                int idlogin= Convert.ToInt16(Request.Cookies["ftid"].Value);

                dbFirstStepDataContext db = new dbFirstStepDataContext();

                var user = db.EntityTheoDois.Where(g=>g.IdUser== idlogin&& g.IdBanBe== IdBanBe).FirstOrDefault();
                if (user != null)
                {
                    user.TrangThai = 1;
                    db.SubmitChanges();
                }
                else
                {
                    EntityTheoDoi theodoi = new EntityTheoDoi();
                    theodoi.IdUser = idlogin;
                    theodoi.IdBanBe = IdBanBe;
                    theodoi.TrangThai = 1;
                    theodoi.Loaifb = 1;
                    theodoi.Date = DateTime.Now;
                    db.EntityTheoDois.InsertOnSubmit(theodoi);
                    db.SubmitChanges();
                }
                HoatDongModel.AddHoatDong(idlogin, IdBanBe, 1, 0);
                return "complete";
            }
            catch { return "error"; }

        }
        //
        public string UnBlockFollow(int IdBanBe)//là mở khóa bạn
        {
            try
            {
                int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);

                dbFirstStepDataContext db = new dbFirstStepDataContext();

                var user = db.EntityTheoDois.Where(g => g.IdUser == IdBanBe && g.IdBanBe == idlogin).FirstOrDefault();
                if (user != null)
                {
                    user.TrangThai = 1;
                    db.SubmitChanges();
                }

                
               HoatDongModel.AddHoatDong(idlogin, IdBanBe, 1, 0);
                return "complete";
            }
            catch { return "error"; }

        }
        public string UnFollow(int IdBanBe)
        {
            try
            {
                int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);

                dbFirstStepDataContext db = new dbFirstStepDataContext();

                var user = db.EntityTheoDois.Where(g => g.IdUser == idlogin && g.IdBanBe == IdBanBe).FirstOrDefault();
                if (user != null)
                {
                    user.TrangThai = 0;
                    db.SubmitChanges();
                }
                HoatDongModel.RemoveHoatDong(idlogin,IdBanBe);
              //  HoatDongModel.AddHoatDong(idlogin, IdBanBe, 2, 0); // người gởi - người nhận
                return "complete";
            }
            catch { return "error"; }

        }
        public string BlockFollow(int IdBanBe)
        {
            try
            {
                int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);

                dbFirstStepDataContext db = new dbFirstStepDataContext();

                var user = db.EntityTheoDois.Where(g => g.IdUser == IdBanBe && g.IdBanBe == idlogin).FirstOrDefault();
                if (user != null)
                {
                    user.TrangThai = 0;
                    db.SubmitChanges();
                }
                HoatDongModel.AddHoatDong(idlogin, IdBanBe, 2, 0);
                return "complete";
            }
            catch { return "error"; }

        }

        #endregion

        public ActionResult _NutTheoDoi(int IdBanBe)
        {
            ViewBag.IdBanBe = IdBanBe;
            ViewBag.TrangThai = -1;// k hiện nút theo dõi
            if (Request.Cookies["ftid"] != null)
            {
                int idlogin = Convert.ToInt16(Request.Cookies["ftid"].Value);
                if (IdBanBe != idlogin)
                {
                    using (dbFirstStepDataContext db = new dbFirstStepDataContext())
                    {
                        var theodoi =db.EntityTheoDois.Where(g => g.IdUser == idlogin && g.IdBanBe == IdBanBe).FirstOrDefault();
                        if (theodoi!= null)
                        {
                            if (theodoi.TrangThai == 0)
                                ViewBag.TrangThai = 2;//hiện lên theo dõi
                            else
                                ViewBag.TrangThai = 1;//hiện lên nút hủy theo dõi
                        }
                        else
                        {
                            ViewBag.TrangThai = 2;//hiện lên theo dõi
                        }
                    }
                }
                
                
            }
            return PartialView();
        }

        #region ----------------------------sau nay bo phan nay
       
        //// follow ko phải fb 
        //public string FollowBiTheoDoi(int IdBanBe)
        //{
        //    try
        //    {
        //        dbFirstStepDataContext db = new dbFirstStepDataContext();
        //        var item = db.EntityTheoDois.Where(p => p.IdUser == Convert.ToInt16(Request.Cookies["ftid"].Value) && p.IdBanBe == IdBanBe).First();
        //        item.TrangThai = 1;
        //        item.Date = DateTime.Now;
        //        db.SubmitChanges();
        //        bantintheodoi(IdBanBe);
        //        return "complete";
        //    }
        //    catch { return "error"; }

        //}
        //public string FollowPro(int IdBanBe)
        //{
        //    try
        //    {
        //        dbFirstStepDataContext db = new dbFirstStepDataContext();
        //        EntityTheoDoi theo = new EntityTheoDoi();
        //        theo.IdBanBe = IdBanBe;
        //        theo.IdUser = Convert.ToInt16(Request.Cookies["ftid"].Value);
        //        theo.TrangThai = 1;
        //        theo.Date = DateTime.Now;
        //        db.EntityTheoDois.InsertOnSubmit(theo);
        //        db.SubmitChanges();
        //        bantintheodoi(IdBanBe);
        //        return "complete";
        //    }
        //    catch { return "error"; }

        //}
        //public string UnFollowPro(int IdBanBe)
        //{
        //    try
        //    {
        //        dbFirstStepDataContext db = new dbFirstStepDataContext();
        //        var item = db.EntityTheoDois.Where(p => p.IdUser == Convert.ToInt16(Request.Cookies["ftid"].Value) && p.IdBanBe == IdBanBe && p.TrangThai == 1).First();
        //        db.EntityTheoDois.DeleteOnSubmit(item);
        //        db.SubmitChanges();
        //        return "complete";
        //    }
        //    catch { return "error"; }

        //}

        #endregion

        #endregion

        #region bản tin hiển thị khi theo dõi , cơ mà nó liên quan tới theo dõi nha :3
        public void bantintheodoi(int IdBanBe)
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var tim = db.EntityHoatDongs.Where(p => p.IdBanBe == IdBanBe && p.IdUser == Convert.ToInt16(Request.Cookies["ftid"].Value) && p.Loai == 1).ToList();
            if (tim.Count() == 0)
            {
                EntityHoatDong taomoi = new EntityHoatDong();
                taomoi.IdUser = Convert.ToInt16(Request.Cookies["ftid"].Value);
                taomoi.IdBanBe = IdBanBe;
                taomoi.Date = DateTime.Now;
                taomoi.Loai = 1;
                //taomoi.IdDuAn = 1;
                db.EntityHoatDongs.InsertOnSubmit(taomoi);
                db.SubmitChanges();
            }
        }
        #endregion
    }
}
