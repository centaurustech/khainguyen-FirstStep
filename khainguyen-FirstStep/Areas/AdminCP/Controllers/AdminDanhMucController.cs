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
    public class AdminDanhMucController : Controller
    {
        //
        // GET: /AdminCP/AdminDanhMuc/

        public ActionResult Index()
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var ban = db.EntityDanhMucs.ToList();
            return View(ban);
        }

        #region "insert"
        public ActionResult Insert()
        {
            AdminDanhMucModel tnew = new AdminDanhMucModel();
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            ViewBag.DanhMuc = db.EntityDanhMucs.Where(p => p.IdRoot == null).ToList();
            return View(tnew);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Insert(AdminDanhMucModel DM)
        {
            try
            {
                dbFirstStepDataContext db = new dbFirstStepDataContext();
                EntityDanhMuc ban = new EntityDanhMuc();
                ban.TenDM = DM.TieuDe;
                ban.IdRoot = DM.IdRoot;
                db.EntityDanhMucs.InsertOnSubmit(ban);
                db.SubmitChanges();

           
                return RedirectToAction("Index", "AdminDanhMuc");
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
            return View(AdminDanhMucModel.LayModel(Id));
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(AdminDanhMucModel DM, HttpPostedFileBase Anh)
        {

            try
            {

                AdminDanhMucModel.Edit(DM);
                return RedirectToAction("Index", "AdminDanhMuc");
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
               AdminDanhMucModel.Delete(Id);
               
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
