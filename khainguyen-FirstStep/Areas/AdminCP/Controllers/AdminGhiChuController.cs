using khainguyen_FirstStep.Areas.AdminCP.Models;
using MvcLibrary.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace khainguyen_FirstStep.Areas.AdminCP.Controllers
{
    public class AdminGhiChuController : Controller
    {
        //
        // GET: /AdminCP/AdminGhiChu/
        public ActionResult Index()
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var ban = db.EntityQuotes.ToList();
            return View(ban);
        }

        #region "insert"
        public ActionResult Insert()
        {
            AdminGhiChuModel tnew = new AdminGhiChuModel();
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            ViewBag.DanhMuc = db.EntityUsers.ToList();
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Insert(AdminGhiChuModel DM, HttpPostedFileBase Image)
        {
            try
            {
                dbFirstStepDataContext db = new dbFirstStepDataContext();
                EntityQuote ban = new EntityQuote();
                if (Image != null)
                {
                    ImageHelper imgHelper = new ImageHelper();
                    string encodestring = imgHelper.encodeImageFile(Image);
                    db.SubmitChanges();
                    if (encodestring == "!")
                        return RedirectToAction("Index", "Error", new { errorMsg = "Can't upload Images" });
                    var path = Path.Combine(Server.MapPath("~/Content/Images/GhiChu"), encodestring);
                    Image.SaveAs(path);
                    ban.Image = encodestring;
                    ban.IdUser = null;
                }
                else
                {
                    ban.IdUser = DM.IdUser>0?DM.IdUser:null;
                }
                ban.NoiDung = DM.NoiDung;
                db.EntityQuotes.InsertOnSubmit(ban);
                db.SubmitChanges();


                return RedirectToAction("Index", "AdminGhiChu");
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }
        #endregion //insert

        #region "Edit"
        public ActionResult Edit(int Id)
        {
            AdminGhiChuModel tnew = new AdminGhiChuModel();
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            ViewBag.DanhMuc = db.EntityUsers.ToList();
            return View(AdminGhiChuModel.LayModel(Id));
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(AdminGhiChuModel DM, HttpPostedFileBase Image)
        {

            try
            {
                if (Image != null)
                {
                    ImageHelper imgHelper = new ImageHelper();
                    string encodestring = imgHelper.encodeImageFile(Image);
                    if (encodestring == "!")
                        return RedirectToAction("Index", "Error", new { errorMsg = "Can't upload Images" });
                    var path = Path.Combine(Server.MapPath("~/Content/Images/GhiChu"), encodestring);
                    Image.SaveAs(path);
                    DM.HinhAnh = encodestring;
                    DM.IdUser = null;
                }
                else
                {
                    DM.IdUser = DM.IdUser > 0 ? DM.IdUser : null;
                }
                AdminGhiChuModel.Edit(DM);
                return RedirectToAction("Index", "AdminGhiChu");
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }
        #endregion //Edit

        #region "Delete"

        public string Delete(int Id)
        {
            try
            {
                AdminGhiChuModel.Delete(Id);

                return "ok";
            }
            catch
            {
                return "error";
            }
        }
        #endregion //Delete

    }
}
