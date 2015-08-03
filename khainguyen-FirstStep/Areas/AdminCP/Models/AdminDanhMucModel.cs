using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace khainguyen_FirstStep.Areas.AdminCP.Models
{
    public class AdminDanhMucModel
    {
        public int? Id { get; set; }
        public int? IdRoot { get; set; }
        public string TieuDe { get; set; }
        public int? ViTri { get; set; }

        public static AdminDanhMucModel LayModel(int Id)
        {
            AdminDanhMucModel Ban = new AdminDanhMucModel();
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            EntityDanhMuc tnew = db.EntityDanhMucs.Where(p => p.Id == Id).First();
            Ban.Id = tnew.Id;
            Ban.IdRoot  = tnew.IdRoot;
            Ban.TieuDe = tnew.TenDM;
            return Ban;

        }
        public static void Edit(AdminDanhMucModel Banner)
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            EntityDanhMuc Faq = db.EntityDanhMucs.Where(p => p.Id == Banner.Id).First();
            Faq.TenDM = Banner.TieuDe;
            Faq.IdRoot = Banner.IdRoot;
            db.SubmitChanges();
        }
        public static void Delete(int Id)
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            EntityDanhMuc tnew = db.EntityDanhMucs.Where(p => p.Id == Id).Single();
            db.EntityDanhMucs.DeleteOnSubmit(tnew);
            db.SubmitChanges();
        }
    }
}