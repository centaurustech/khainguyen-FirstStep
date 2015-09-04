using System.Linq;

namespace khainguyen_FirstStep.Areas.AdminCP.Models
{
    public class AdminGhiChuModel
    {
        public int? Id { get; set; }
        public string NoiDung { get; set; }
        public string HinhAnh { get; set; }
        public int? IdUser { get; set; }

        public static AdminGhiChuModel LayModel(int Id)
        {
            AdminGhiChuModel Ban = new AdminGhiChuModel();
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            EntityQuote tnew = db.EntityQuotes.Where(p => p.Id == Id).First();
            Ban.Id = tnew.Id;
            Ban.IdUser = tnew.IdUser;
            Ban.HinhAnh = tnew.Image;
            Ban.NoiDung = tnew.NoiDung;
            return Ban;

        }
        public static void Edit(AdminGhiChuModel Banner)
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            EntityQuote Faq = db.EntityQuotes.Where(p => p.Id == Banner.Id).First();
            Faq.IdUser = Banner.IdUser;
            Faq.Image = Banner.HinhAnh;
            Faq.NoiDung = Banner.NoiDung;
            db.SubmitChanges();
        }
        public static void Delete(int Id)
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            EntityQuote tnew = db.EntityQuotes.Where(p => p.Id == Id).Single();
            db.EntityQuotes.DeleteOnSubmit(tnew);
            db.SubmitChanges();
        }
    }
}