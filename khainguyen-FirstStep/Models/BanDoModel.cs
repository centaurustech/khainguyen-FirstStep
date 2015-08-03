using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Helpers;

namespace khainguyen_FirstStep.Models
{
    public class BanDoModel
    {
        public int value { get; set; }
        public string color { get; set; }
        public string highlight { get; set; }
        public string label { get; set; }
        public static string getSLDuAn_DaDauTu( List<EntityDuAn> duan)
        {
            string[] mang = new string[] { "#FF99CC", "#CC99CC", "#9999CC", "#6699CC", "#0099CC", "#FF66CC", "#CC99CC", "#6699CC", "#0099CC", "#3366CC", "#00EE00", "#008800", "#002200", "#000044", "#DDC488", "#ECAB53", "#008080", "#FFCC99", "#FFD700", "#9932CC", "#8A2BE2", "#C71585", "#800080", "#FFBF00", "#9966CC", "#7FFFD4", "#007FFF", "#3D2B1F", "	#0000FF", "#DE3163", "#7FFF00", "#50C878", "#DF73FF", "#4B0082", "#FFFF00", "#008080", "#660099", "#FFE5B4" };
            dbFirstStepDataContext db = new dbFirstStepDataContext();
            List<BanDoModel> list = new List<BanDoModel>  ();
            var danhmuc  =  db.EntityDanhMucs.Where(g => g.IdRoot ==1).ToList();
            Random rd = new Random();
            int vt = rd.Next(mang.Count()-danhmuc.Count());
            int i = -1;
            foreach(var item in danhmuc)
            {
                i++;
                BanDoModel bando = new BanDoModel ();
                int sl = duan == null ? 0 : duan.Where(g => g.IdDanhMuc == item.Id).ToList().Count();
                bando.value = 1;
                if (sl > 0)
                {
                    bando.color = mang[vt+i];
                    
                }
                else bando.color = "#f5f5f5";

             //   bando.color = sl > 0 ? "#14c3be" : "#f5f5f5";
                bando.highlight = "#FF5A5E";
                bando.label = item.TenDM.ToString()+"("+sl+")";
                list.Add(bando);
            }
           
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJSON = oSerializer.Serialize(list);
            return sJSON;
       
        }
    }
}