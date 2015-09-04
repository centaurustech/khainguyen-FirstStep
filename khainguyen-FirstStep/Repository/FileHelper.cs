using System;
using System.IO;
using System.Web;

namespace MvcLibrary.Repository
{
    public class FileHelper
    {
        public string encodeFile(HttpPostedFileBase file)
        {
            // extract only the fielname
            var fileName = Path.GetFileName(file.FileName);
            string ext = Path.GetExtension(file.FileName);
            ext = ext.ToLower();
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif" && ext != ".rar" && ext != ".pdf" && ext != ".txt" && ext != ".csv") // check file hinh
                return "!";

            // store the file inside folder
            //string milisecond = DateTime.Now.Millisecond.ToString();
            //fileName = fileName + DateTime.Now.ToLongDateString() + milisecond;

            //MvcLibrary.Repository.Security ser = new MvcLibrary.Repository.Security();
            //string encodeFileName = ser.GetHashPassword(fileName); // ma hoa file hinh

            //store the file inside forder
            string[] name;
            name = fileName.Split('.');
            fileName = name[0];
            string encodeFileName = fileName + "_" + DateTime.Now.ToString("yyyy_MM_dd");


            encodeFileName += ext;
            return encodeFileName;
        }
    }
}