using System.Linq;
using System.Web.Mvc;
using MvcLibrary.Repository;

namespace khainguyen_FirstStep.Areas.AdminCP.Controllers
{
    public class AdminUserController : Controller
    {
        //
        // GET: /AdminCP/AdminUser/

        public ActionResult Index()
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var ban = db.EntityUsers.ToList();
            return View(ban);
        }
        public ActionResult Ban(int id, int t)
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var item = db.EntityUsers.Where(p => p.Id == id).First();
            item.TrangThai = t;
            db.SubmitChanges();
            return RedirectToAction("Index", "AdminUser");
        }
        public ActionResult Sendmail_AllUser()
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var ban = db.EntityUsers.ToList();
            foreach (var user in ban)
            {
                MailHelper.SendMailtoUser(user.HoTen, user.Email, NoiDung_VideoMoi(user.HoTen));
            }
            return View(ban);
        }

        public string NoiDung_VideoMoi(string HoTen)
        {
            string noidung = "<div rev='full_text' contenteditable='false' class='cke_editable cke_editable_inline cke_contents_ltr' tabindex='0' spellcheck='true' role='textbox' aria-label='false' aria-describedby='cke_4390' title='' style='position: relative; border: none; cursor: move; padding: 0px;'><div mc:repeatable='full_text'><layout label='full_text'><table width='640' border='0' align='center' cellpadding='0' cellspacing='0' class='wrap' style='border-collapse: collapse;background-color: #fff;'><tbody><tr><td align='center' valign='top' class='module-td'><table align='center' width='580' border='0' cellpadding='0' cellspacing='0' class='row' style='border-collapse: collapse;'><tbody><tr><td align='left' class='content' mc:edit='content' style='font-family: &#39;Roboto&#39;,Tahoma,Arial;font-size: 14px;line-height: 22px;font-weight: 400;color: #495161;-webkit-font-smoothing: antialiased;'>";
            noidung += "<singleline label='content'>" + "Chào bạn <b>" + HoTen + "</b> ," + "<br/>";
            noidung += "Để minh họa cho việc gọi vốn cộng đồng – crowdfunding, tháng 9 này FirstStep hân hạnh giới thiệu cho mọi người một clip rất dễ thương về chú thỏ Muffin. <br/><br/>Câu chuyện bắt đầu từ một khu rừng xa xôi, có chú thỏ thông minh thích sáng chế tên là Muffin. Muffin nghĩ ra một chiếc máy biến 1 củ cà rốt thành 3 củ cà rốt. Tuy nhiên, chú ta lại không đủ tiền để thực hiện chiếc máy. <br/><br/>Muffin đã làm như thế nào? Hãy theo dõi clip để biết thêm chi tiết:<br/><br/>";
            noidung += "</singleline>";
            noidung += "</td></tr><tr><td height='15' class='small-img' style='font-size: 0px;line-height: 0px;border-collapse: collapse;'></td></tr><tr><td align='center'><a href='https://www.youtube.com/watch?v=g4KxBZVXGic' target='_blank'>Hành trình biến ý tưởng thành hiện thực của Muffin</a></td></tr>";
            noidung += "<tr><td align='center'><img alt='logo' class='logo' editable='true' label='logo' mc:edit='logo' data-cke-saved-src='http://firststep.vn/Content/Images/Upload/ratbit3-01.jpg' src='http://firststep.vn/Content/Images/Upload/ratbit3-01.jpg' style='border: 0px; display: block; max-height: 200px;'></td></tr></tbody></table></td></tr></tbody></table> </layout></div>";
            return noidung;
            //<tr><td height='15' class='small-img' style='font-size: 0px;line-height: 0px;border-collapse: collapse;'><img alt='logo' class='logo' editable='true' label='logo' mc:edit='logo' data-cke-saved-src='http://firststep.vn/Content/Images/Upload/ratbit3-01.jpg' src='http://firststep.vn/Content/Images/Upload/ratbit3-01.jpg' style='border: 0px; display: block; max-height: 40px;'></td></tr>
//                <div mc:repeatable="full_text"><layout label="full_text"><table width="640" border="0" align="center" cellpadding="0" cellspacing="0" class="wrap" style="border-collapse: collapse;background-color: #fff;"><tbody><tr><td align="center" valign="top" class="module-td"><table align="center" width="580" border="0" cellpadding="0" cellspacing="0" class="row" style="border-collapse: collapse;"><tbody><tr><td align="left" class="content" mc:edit="content" style="font-family: 'Roboto',Tahoma,Arial;font-size: 14px;line-height: 22px;font-weight: 400;color: #495161;-webkit-font-smoothing: antialiased;">
                //</td></tr><tr><td height="15" class="small-img" style="font-size: 0px;line-height: 0px;border-collapse: collapse;"><img data-cke-saved-src="http://chuyenbanle.com/Email-template-corporate/newsletter/html/blue/images/spacer.gif" src="./Basic_1_files/spacer.gif" width="1" height="1" style="border: 0;display: block;-ms-interpolation-mode: bicubic;"></td></tr><tr><td align="center"><iframe width="420" height="315" src="http://www.youtube.com/embed/g4KxBZVXGic?autoplay=1">
//</iframe></td></tr></tbody></table></td></tr></tbody></table> </layout></div>
        }

        //[ValidateInput(false)]
        //[HttpPost]
        //public ActionResult Sendmail_AllUser()
        //{
        //    dbFirstStepDataContext db = new dbFirstStepDataContext();
        //    var ban = db.EntityUsers.ToList();
        //    return View(ban);
        //    //<iframe width="420" height="315" src="http://www.youtube.com/embed/g4KxBZVXGic?autoplay=1"></iframe>
        //}
    }
}
