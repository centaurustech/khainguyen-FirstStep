using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace khainguyen_FirstStep.Areas.AdminCP.Models
{
    public class AdminDuAnModel
    {
        public int Id { get; set; }
        public int TrangThai { get; set; }
        public string LyDoBlock { get; set; }
       
        
    }
    public class AdminManagerIndexModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }// ==true : category
        public bool TrangThai { get; set; }

        public static List<AdminManagerIndexModel> get_ManagerIndex()
        {
            List<AdminManagerIndexModel> ds = new List<AdminManagerIndexModel>();
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            var Trend = db.EntityTrends.ToList();
            foreach (var item in Trend)
            {
                AdminManagerIndexModel md = new AdminManagerIndexModel();
                md.Id = item.Id;
                md.Name = item.IdCategory > 0 ? item.EntityDanhMuc.TenDM : item.TrendName;
                md.TrangThai = item.TrangThai.Value;
                md.Type = item.IdCategory > 0 ? "Danh mục" : "Trend";
                ds.Add(md);
            }
            return ds;

        }
    }
}