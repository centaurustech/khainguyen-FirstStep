using khainguyen_FirstStep.Areas.AdminCP.Models;
using MvcLibrary.Repository;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace khainguyen_FirstStep.Areas.AdminCP.Controllers
{
    public class AdminBannerController : Controller
    {
        //
        // GET: /AdminCP/AdminBanner/

        public ActionResult Index()
        {
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var ban = db.EntityBanners.ToList();
            return View(ban);
        }

        #region "insert"
        public ActionResult Insert()
        {
            AdminBannerModel tnew = new AdminBannerModel();
            return View(tnew);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Insert(AdminBannerModel DM)
        {
            try
            {
                //AdminBannerModel.Insert(DM);
                dbFirstStepDataContext db = new dbFirstStepDataContext();
                // IList<EntityAnhChiTietSP> list = new List<EntityAnhChiTietSP>();
                //var idsp = db.EntitySanPhams.Where(t => t.TenSanPham == DM.TenSanPham && t.Date == ban.Date && t.MaSanPham == DM.MaSanPham).First();

                if (DM.LoaiBanner == LoaiBanner.Image)
                {
                    int tt = 0;
                    for (int i = 0; i < Request.Files.Count; i++)
                    {

                        HttpPostedFileBase hpf = Request.Files[i];
                        if (hpf.FileName == "")
                        {
                            tt = -1; // k co hinh anh
                            break;
                        }
                        tt++;


                        ImageHelper imgHelper = new ImageHelper();
                        string encodestring = imgHelper.encodeImageFile(hpf);
                        //var anh = db.EntitySanPhams.Where(t => t.MaSanPham == DM.MaSanPham && t.Date == DM.Date).First();
                        DM.Anh = encodestring;
                        db.SubmitChanges();
                        if (encodestring == "!")
                            return RedirectToAction("Error", "Home", new { errorMsg = "Can't upload Images" });
                        var path = Path.Combine(Server.MapPath("~/Content/Images/Banner"), encodestring);
                        hpf.SaveAs(path);

                        EntityBanner ban = new EntityBanner();
                        ban.HinhAnh = encodestring;
                        ban.ViTri = DM.ViTri;
                        ban.TenNut = DM.TenNut;
                        ban.NoiDung = DM.NoiDung;
                        ban.LinkNut = DM.LinkNut;
                        ban.TieuDe = DM.TieuDe;
                        ban.LoaiBanner = DM.LoaiBanner;
                        db.EntityBanners.InsertOnSubmit(ban);
                        db.SubmitChanges();

                    }
                }
                else
                {
                    EntityBanner ban = new EntityBanner();
                    ban.LinkNut = DM.LinkNut;
                    ban.LoaiBanner = DM.LoaiBanner;
                    ban.ViTri = DM.ViTri;
                    db.EntityBanners.InsertOnSubmit(ban);
                    db.SubmitChanges();
                }
                
                return RedirectToAction("Index", "AdminBanner");
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
            return View(AdminBannerModel.LayModel(Id));
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(AdminBannerModel DM, HttpPostedFileBase Anh)
        {

            try
            {
                if (DM.LoaiBanner != LoaiBanner.Video)
                {


                    if (Anh != null && Anh.ContentLength > 0)
                    {
                        int kb = Anh.ContentLength / 1024; //file size kb
                        if (kb >= 2048) // file qua lon
                        {
                            return RedirectToAction("Index", "Error", new { errorMsg = "Hình ảnh phải bé hơn 2MB." });
                        }

                        ImageHelper imgHelper = new ImageHelper();
                        string encodestring = imgHelper.encodeImageFile(Anh);

                        if (encodestring == "!")
                            return RedirectToAction("Index", "Error", new { errorMsg = "Không thể Upload hình" });

                        var path = Path.Combine(Server.MapPath("~/Content/Images/Banner"), encodestring);
                        Anh.SaveAs(path);

                        DM.Anh = encodestring;
                    }

                    string image = AdminBannerModel.Edit(DM);
                    if (image != null)
                    {
                        string fileToDelete = Path.Combine(Server.MapPath("~/Content/Images/Banner"), image); // file hinh cu
                        System.IO.File.Delete(fileToDelete);
                    }
                }
                else
                {
                    AdminBannerModel.Edit(DM);
                }
                return RedirectToAction("Index", "AdminBanner");
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
                string image = AdminBannerModel.Delete(Id);
                if (image != null)
                {
                    string fileToDelete = Path.Combine(Server.MapPath("~/Content/Images/Banner"), image); // file hinh cu
                    System.IO.File.Delete(fileToDelete);
                }

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
