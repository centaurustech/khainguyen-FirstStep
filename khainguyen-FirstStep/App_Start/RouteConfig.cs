using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace khainguyen_FirstStep
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Footer
            routes.MapRoute(
              name: "timhieufirststep",
              url: "tim-hieu-Firststep",
              defaults: new { controller = "Home", action = "_Footer_TimhieuFirststep" },
              namespaces: new[] { "khainguyen-FirstStep.Controllers" }
            );
            routes.MapRoute(
              name: "thongtinAlpe",
              url: "thong-tin-Alpe",
              defaults: new { controller = "Home", action = "_Footer_ThongtinAlpe" },
              namespaces: new[] { "khainguyen-FirstStep.Controllers" }
            );
            routes.MapRoute(
              name: "filedownload",
              url: "File-Download",
              defaults: new { controller = "Home", action = "_Footer_FileDownload" },
              namespaces: new[] { "khainguyen-FirstStep.Controllers" }
            );
            #endregion            

            #region Dự Án
            routes.MapRoute(
              name: "taoduan",
              url: "tao-du-an",
              defaults: new { controller = "DuAn", action = "TaoDuAn" },
              namespaces: new[] { "khainguyen-FirstStep.Controllers" }
          );
            routes.MapRoute(
              name: "dieuchinhduan",
              url: "dieu-chinh-thong-tin-du-an/{Id}",
              defaults: new { controller = "DuAn", action = "TaoDuAn_Buoc2", Id = UrlParameter.Optional },
              namespaces: new[] { "khainguyen-FirstStep.Controllers" }
          );
            routes.MapRoute(
              name: "dieuchinhduan1",
              url: "dieu-chinh-thong-tin-du-an-1/{Id}",
              defaults: new { controller = "DuAn", action = "ChinhSuaDuAn", Id = UrlParameter.Optional },
              namespaces: new[] { "khainguyen-FirstStep.Controllers" }
          );

            routes.MapRoute(
              name: "duanindex",
              url: "du-an",
              defaults: new { controller = "DuAn", action = "Index" },
              namespaces: new[] { "khainguyen-FirstStep.Controllers" }
          );
            routes.MapRoute(
             name: "chitietduan",
             url: "du-an/{Title}/{*InfoAdd}",
             defaults: new { controller = "DuAn", action = "ChiTietDuAn", Title = UrlParameter.Optional, InfoAdd = UrlParameter.Optional },
             namespaces: new[] { "khainguyen-FirstStep.Controllers" }
         );
            routes.MapRoute(
            name: "quanlyduan",
            url: "quan-ly-du-an/{Title}",
            defaults: new { controller = "DuAn", action = "QuanLy_DuAn", Title = UrlParameter.Optional },
            namespaces: new[] { "khainguyen-FirstStep.Controllers" }
        );
            routes.MapRoute(
           name: "UpdateDuAn",
           url: "cap-nhat-du-an/{IdDuAn}",
           defaults: new { controller = "DuAn", action = "UpdateDuAn", IdDuAn = UrlParameter.Optional },
           namespaces: new[] { "khainguyen-FirstStep.Controllers" }
       );
            //UpdateDuAn
            #endregion

            #region  "HoatDongController"
            routes.MapRoute(
                name: "tinnhan",
                url: "thong-tin-ca-nhan/tin-nhan/{type}",
                defaults: new { controller = "HoatDong", action = "TinNhan", Type = UrlParameter.Optional },
                namespaces: new[] { "khainguyen-FirstStep.Controllers" }
             );

            routes.MapRoute(
               name: "nhatkyhoatdong",
               url: "thong-tin-ca-nhan/nhat-ky-hoat-dong",
               defaults: new { controller = "HoatDong", action = "HoatDong", Type = UrlParameter.Optional },
               namespaces: new[] { "khainguyen-FirstStep.Controllers" }
            );
            routes.MapRoute(
             name: "lichsudautu",
             url: "thong-tin-ca-nhan/luoc-su-dau-tu-du-an",
             defaults: new { controller = "HoatDong", action = "LichSu", Type = UrlParameter.Optional },
             namespaces: new[] { "khainguyen-FirstStep.Controllers" }
          );
            #endregion

            #region"USER - link"
            routes.MapRoute(
           name: "nguoi-dung",
           url: "thong-tin-ca-nhan/{Link}",
           defaults: new { controller = "user", action = "Index", Link = UrlParameter.Optional },
           namespaces: new[] { "khainguyen-FirstStep.Controllers" }
            );
            #endregion

            #region"AccountController"
            routes.MapRoute(
           name: "tuy-chinh-tai-khoan",
           url: "tuy-chinh-khac/tai-khoan",
           defaults: new { controller = "Account", action = "DoiMatKhau"},
           namespaces: new[] { "khainguyen-FirstStep.Controllers" }
            );
            routes.MapRoute(
              name: "thongtincanhan-taikhoan",
              url: "tuy-chinh-khac/thong-tin-ca-nhan",
              defaults: new { controller = "Account", action = "SuaThongTin"},
              namespaces: new[] { "khainguyen-FirstStep.Controllers" }
               );
            routes.MapRoute(
                name: "Dang nhap",
              url: "tai-khoan/dang-nhap",
              defaults: new { controller = "Account", action = "Login"},
              namespaces: new[] { "khainguyen-FirstStep.Controllers" }
               );
            routes.MapRoute(
                name: "Dang xuat",
              url: "tai-khoan/dang-xuat",
              defaults: new { controller = "Account", action = "Logout"},
              namespaces: new[] { "khainguyen-FirstStep.Controllers" }
               );
            routes.MapRoute(
                name: "Nhap lai mat khau",
              url: "tai-khoan/nhap-lai-mat-khau",
              defaults: new { controller = "Account", action = "LoginLife" },
              namespaces: new[] { "khainguyen-FirstStep.Controllers" }
               );
            routes.MapRoute(
           name: "dang-nhap-bang-fb",
           url: "tai-khoan/dang-nhap-bang-fb",
           defaults: new { controller = "Account", action = "Facebookcallback" },
           namespaces: new[] { "khainguyen-FirstStep.Controllers" }
            );
            #endregion

            #region"FAQController"
            routes.MapRoute(
           name: "faq1",
           url: "noi-dung-ho-tro",
           defaults: new { controller = "FAQ", action = "IndexFAQ1", Link = UrlParameter.Optional },
           namespaces: new[] { "khainguyen-FirstStep.Controllers" }
            );
            routes.MapRoute(
           name: "faq2",
           url: "cau-hoi-thuong-gap-2",
           defaults: new { controller = "FAQ", action = "IndexFAQ2", Link = UrlParameter.Optional },
           namespaces: new[] { "khainguyen-FirstStep.Controllers" }
            );
            routes.MapRoute(
          name: "faq3",
          url: "cau-hoi-thuong-gap/{name}",
          defaults: new { controller = "FAQ", action = "IndexFAQ3", name = UrlParameter.Optional },
          namespaces: new[] { "khainguyen-FirstStep.Controllers" }
           );
            routes.MapRoute(
          name: "faq4",
          url: "so-tay-firststep/{name}",
          defaults: new { controller = "FAQ", action = "IndexFAQ4", name = UrlParameter.Optional},
          namespaces: new[] { "khainguyen-FirstStep.Controllers" }
           );
            #endregion

            #region"ThanhToanController"

            routes.MapRoute(
           name: "thanhtoan_lan1",
           url: "thanh-toan-buoc-1-chon-muc-gia-{Id}-{pt}",
           defaults: new { controller = "ThanhToan", action = "ThanhToan_Lan1", Id = UrlParameter.Optional, pt = UrlParameter.Optional },
           namespaces: new[] { "khainguyen-FirstStep.Controllers" }
            );
           
            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
           name: "CatchAll",
           url: "{*catchall}",
           defaults: new { controller = "Error", action = "Index", statusCode = 404, error = new HttpException(404, "Route not found via RouteConfig.") });
        }
    }
}