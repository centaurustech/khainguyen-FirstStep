using System;
using System.Collections.Generic;

namespace khainguyen_FirstStep.Models
{
    public class getSQL_Message
    {
        public int iduser = 0;
        public DateTime datesend = new DateTime ();
        public int IdDuAn = 0;
    }
    public class SendMessage
    {
        public int IdSendMessge = 0;
       public  EntityUser user = new EntityUser ();
       public int TrangThai = 1;
       public string TenDuAn = "";
       public int IdDuAn = 0;
       public int SoTinNhan = 0;
       public int SoTinNhanChuaDoc = 0;
       public DateTime ThoiGianNew= DateTime.Now;
       public string NoiDung = "";
       public string NoiDungRutGon = "";

    }
    public class LichSuModel
    {
        public EntityDauTu DauTuModel = new EntityDauTu();
        public IList<EntityTinNhan> TinNhanModel= new List<EntityTinNhan>();
        public IList<EntityCapNhatDuAn> CapNhatModel = new List<EntityCapNhatDuAn>();
    }
}