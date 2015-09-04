using System.Linq;

namespace khainguyen_FirstStep.Areas.AdminCP.Models
{
    public class AdminFAQModel
    {
        public int? Id { get; set; }
        public int? IdLoaiFAQ { get; set; }
        public int? IdGroupFAQ { get; set; }
        public string TenGroup { get; set; }
        public string CauHoi { get; set; }
        public string CauTraLoi { get; set; }
        public int? ViTri { get; set; }
        //Type = 1 => Group
        //Type = 2 => FAQ
        public int Type { get; set; }

        public static AdminFAQModel LayModelGroup(int Id)
        {
            AdminFAQModel Ban = new AdminFAQModel();
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            EntityGroupFAQ tnew = db.EntityGroupFAQs.Where(p => p.Id == Id).First();
            Ban.Id = tnew.Id;
            Ban.TenGroup = tnew.TenGroup;
            Ban.IdGroupFAQ = tnew.IdGroupFAQ;
            Ban.IdLoaiFAQ = tnew.IdLoaiFAQ;
            Ban.ViTri = tnew.ViTri;
            Ban.Type = 1;
            return Ban;

        }
        public static AdminFAQModel LayModelFAQ(int Id)
        {
            AdminFAQModel Ban = new AdminFAQModel();
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            EntityFAQ1 tnew = db.EntityFAQ1s.Where(p => p.Id == Id).First();
            Ban.Id = tnew.Id;
            Ban.CauHoi = tnew.CauHoi;
            Ban.CauTraLoi = tnew.CauTraLoi;
            Ban.IdGroupFAQ = tnew.IdGroupFAQ;
            Ban.ViTri = tnew.ViTri;
            Ban.Type = 2;
            return Ban;
        }
        public static void Edit(AdminFAQModel Banner)
        {
            //dbFirstStepDataContext db = new dbFirstStepDataContext();
            //EntityFAQ Faq = db.EntityFAQs.Where(p => p.Id == Banner.Id).First();
            //Faq.TieuDe = Banner.TieuDe;
            //Faq.Loai = Banner.Loai;
            //Faq.NoiDung = Banner.NoiDung;
            //Faq.ViTri = Banner.ViTri;
            //Faq.IdRoot = Banner.IdRoot;
            //db.SubmitChanges();
        }
        public static void Delete(int Id)
        {
            //dbFirstStepDataContext db = new dbFirstStepDataContext();
            //EntityFAQ tnew = db.EntityFAQs.Where(p => p.Id == Id).Single();
            //db.EntityFAQs.DeleteOnSubmit(tnew);
            //db.SubmitChanges();
        }
    }
}