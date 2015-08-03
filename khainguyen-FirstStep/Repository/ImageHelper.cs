using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace MvcLibrary.Repository
{
    public class ImageHelper
    {
        public string encodeImageFile(HttpPostedFileBase file)
        {
            // extract only the fielname
            var fileName = Path.GetFileName(file.FileName);
            string ext = Path.GetExtension(file.FileName);
            ext = ext.ToLower();
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif") // check file hinh
                return "!";

            // store the file inside folder
            string milisecond = DateTime.Now.Millisecond.ToString();
            fileName = fileName + DateTime.Now.ToLongDateString() + milisecond;

            MvcLibrary.Repository.Security ser = new MvcLibrary.Repository.Security();
            string encodeFileName = ser.GetHashPassword(fileName); // ma hoa file hinh

            encodeFileName += ext;
            return encodeFileName;
        }
        public string encodeShortImageFile(HttpPostedFileBase file)
        {
            // extract only the fielname
            var fileName = Path.GetFileName(file.FileName);
            string[]arr = fileName.Split('.');
            string ext = arr[arr.Length-1];
            ext ="."+ ext.ToLower();
          
            // store the file inside folder
            string milisecond = DateTime.Now.Millisecond.ToString();
            fileName = arr[0] + DateTime.Now.ToLongDateString() + milisecond;

            //MvcLibrary.Repository.Security ser = new MvcLibrary.Repository.Security();
            //string encodeFileName = ser.GetHashPassword(fileName); // ma hoa file hinh

            fileName += ext;
            return fileName;
        }
        public void ResizeStream(int imageSize, Stream filePath, string outputPath)
        {
            var image = Image.FromStream(filePath);

            int thumbnailSize = imageSize;
            int newWidth, newHeight;

            if (image.Width > image.Height)
            {
                newWidth = thumbnailSize;
                newHeight = image.Height * thumbnailSize / image.Width;
            }
            else
            {
                newWidth = image.Width * thumbnailSize / image.Height;
                newHeight = thumbnailSize;
            }

            var thumbnailBitmap = new Bitmap(newWidth, newHeight);

            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbnailGraph.DrawImage(image, imageRectangle);

            thumbnailBitmap.Save(outputPath, image.RawFormat);
            thumbnailGraph.Dispose();
            thumbnailBitmap.Dispose();
            image.Dispose();
        }
    }
}