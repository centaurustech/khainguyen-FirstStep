using System.Linq;

namespace khainguyen_FirstStep.Areas.AdminCP.Models
{
    public class AdminBannerModel
    {

        public int? Id { get; set; }
        public string Anh { get; set; }
        public int? ViTri { get; set; }
        public string TenNut { get; set; }
        public string NoiDung { get; set;}
        public string LinkNut { get; set; }
        public string TieuDe { get; set; }

        public static AdminBannerModel LayModel(int Id)
        {
            AdminBannerModel Ban = new AdminBannerModel();
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            EntityBanner tnew = db.EntityBanners.Where(p => p.Id == Id).First();
            Ban.Id = tnew.Id;
            Ban.Anh = tnew.HinhAnh;
            Ban.ViTri = tnew.ViTri;
            Ban.TenNut = tnew.TenNut;
            Ban.NoiDung = tnew.NoiDung;
            Ban.LinkNut = tnew.LinkNut;
            Ban.TieuDe = tnew.TieuDe;
            return Ban;

        }
        public static string Edit(AdminBannerModel Banner)
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            EntityBanner Faq = db.EntityBanners.Where(p => p.Id == Banner.Id).Single();
            string image = null;
            if (Banner.Anh != null)
            {
                image = Faq.HinhAnh;
                Faq.HinhAnh = Banner.Anh;
            }
            Faq.ViTri = Banner.ViTri;
            Faq.TenNut = Banner.TenNut;
            Faq.NoiDung = Banner.NoiDung;
            Faq.LinkNut = Banner.LinkNut;
            Faq.TieuDe = Banner.TieuDe;
            db.SubmitChanges();
            return image;

        }
        public static string Delete(int Id)
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();

            EntityBanner tnew = db.EntityBanners.Where(p => p.Id == Id).Single();
            db.EntityBanners.DeleteOnSubmit(tnew);
            db.SubmitChanges();
            return tnew.HinhAnh;
        }
    }
}