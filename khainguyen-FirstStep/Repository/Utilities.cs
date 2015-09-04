using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
public class Utilities
{

    public static string CheckLife()
    {
        if (System.Web.HttpContext.Current.Request.Cookies["ftid"] != null)
        {
            if (System.Web.HttpContext.Current.Session["fsduytrihoatdong"] == null)
            {
                return "login";
            }
        }
        return "notlogin";
        //  return PartialView();
    }
    public static string URLCategory(int id)
    {
        string ketqua = "/du-an?option=-category=" + id + "-blend=0-"; 
        return ketqua;
    }

    public static int  GetClientTimeOffset()
    {
        var timeOffset = 0;
        var timeOffsetCookie = System.Web.HttpContext.Current.Request.Cookies["fsTimeOffset"];
        if (timeOffsetCookie != null)
        {
            timeOffset += Convert.ToInt32(timeOffsetCookie.Value);
        }
        //DateTime date = DateTime.UtcNow;

        // date = date.AddHours((timeOffset * (-1)));
        return (timeOffset/(-60));
    }
    public static DateTime GetDate_Timezone()
    {
        int off = Utilities.GetClientTimeOffset();
        DateTime date = DateTime.UtcNow;
        date = date.AddHours(off);
        return date;
    }
   
    public static string FormatCurrent(string tien)
    {
        if (int.Parse(tien) <= 0)
        { return "0"; }
        else
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string a = double.Parse(tien).ToString("#,###", cul.NumberFormat);
            return a;
        }
    }
    public static string FormatDay(DateTime date)
    {
        string format = "dd:MMM:yyyy";
        string kq = date.ToString(format);
        //string kq = date.Day + " Tháng" + date.Month + " " + date.Year;
        return kq;
    }

    public static int SplitName_Int64(string Name)
    {
        try
        {
            string[] arr = Name.Split('_');
            if (arr.Count() == 2)
                return int.Parse(arr[1]);
            return -1;
        }
        catch
        {
            return -1;
        }
    }
    // Name_Id // nối
    public static string Paste_Int64(string Name, int Id)
    {
        return Utilities.Encode(Name) + "_" + Id.ToString();
    }

    public static string textInputFilter(string strInput)
    {
        string str = Regex.Replace(strInput, "<.*?>", string.Empty);
        str = strInput.Trim();
        str = Regex.Replace(str, @"\s+", " ");

        str = str.Replace("'", "''");
        str = str.Replace("`", "''");
        str = str.Replace("\\", "");
        return str;
    }

    public static string ToProperCase(string s)
    {
        if (s == null) return s;

        String[] words = s.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length == 0) continue;

            Char firstChar = Char.ToUpper(words[i][0]);
            String rest = "";
            if (words[i].Length > 1)
            {
                rest = words[i].Substring(1).ToLower();
            }
            words[i] = firstChar + rest;
        }
        return String.Join(" ", words);
    }

    public static string FilterHTMLTags(string source)
    {
        string[] allowedTagWithNoProperties = new string[] { "b", "strong", "i", "em", "ul", "li", "br", "hr", "tr" };
        string[] allowedTagWithProperties = new string[] { "img", "a", "table", "td", "th", "p", "font" };
        string[] allowedProperties = new string[] { "height", "width", "src", "href", "colspan", "rowspan", "cellspacing", "cellpadding", "border", "align", "valign", "style", "color", "size" };
        string strBuffer = source;

        foreach (string s in allowedTagWithProperties)
        {
            strBuffer = strBuffer.Replace("<" + s + " ", "[" + s + " ");
            strBuffer = strBuffer.Replace("<" + s + ">", "[" + s + "]");
            strBuffer = strBuffer.Replace("</" + s + ">", "[/" + s + "]");
        }

        foreach (string s in allowedTagWithNoProperties)
        {
            string pattern = "<" + s + @"\s*((\s[\w]+='[_=@" + "\"" + @"();\?\[\]\/\&\+\-\:\.\,\w\d\s]*')?(\s[\w]+=" + "\"" + @"[_=@'();\?\[\]\/\&\+\-\:\.\,\w\d\s]*" + "\"" + @")?(\s[\w]+=[_@();\?\/\&\+\-\:\.\,\w\d]+)?)*\s*/?\s*>";
            strBuffer = Regex.Replace(strBuffer, pattern, "[" + s + "]");
            strBuffer = strBuffer.Replace("</" + s + ">", "[/" + s + "]");
        }

        strBuffer = strBuffer.Replace("<div>", "[br]");

        strBuffer = Regex.Replace(strBuffer, "<.*?>", string.Empty);

        foreach (string s in allowedTagWithNoProperties)
        {
            strBuffer = strBuffer.Replace("[" + s + "]", "<" + s + ">");
            strBuffer = strBuffer.Replace("[/" + s + "]", "</" + s + ">");
            strBuffer = Regex.Replace(strBuffer, "<" + s + @">\s*</" + s + ">", string.Empty);
        }

        foreach (string s in allowedTagWithProperties)
        {
            strBuffer = strBuffer.Replace("[" + s + " ", "<" + s + " ");
            strBuffer = strBuffer.Replace("[" + s + "]", "<" + s + ">");
            strBuffer = strBuffer.Replace("[/" + s + "]", "</" + s + ">");
        }

        foreach (string s in allowedProperties)
            strBuffer = strBuffer.Replace(" " + s + "=", " " + s + "::==");

        string pattern1 = @"(\s[\w]+='[_=!%@" + "\"" + @"();\?\[\]\/\&\+\-\:\.\,\w\d\s]*')?(\s[\w]+=" + "\"" + @"[_=!%@'();\?\[\]\/\&\+\-\:\.\,\w\d\s]*" + "\"" + @")?(\s[\w]+=[_@();\?\/\&\+\-\:\.\,\w\d]+)?";
        strBuffer = Regex.Replace(strBuffer, pattern1, string.Empty);

        foreach (string s in allowedProperties)
            strBuffer = strBuffer.Replace(" " + s + "::==", " " + s + "=");

        strBuffer = strBuffer.Trim();
        strBuffer = Regex.Replace(strBuffer, @"\n+", string.Empty);
        strBuffer = Regex.Replace(strBuffer, @">\s+<", "><");
        strBuffer = Regex.Replace(strBuffer, @">\s+", ">");
        strBuffer = Regex.Replace(strBuffer, @"\s+<", "<");

        strBuffer = strBuffer.Replace("'", "''");
        strBuffer = strBuffer.Replace("`", "''");

        return strBuffer;
    }

    public static bool isNumberic(string str)
    {
        Regex rgx = new Regex("^[0-9]+$");

        if (rgx.IsMatch(str))
            return true;
        else return false;
    }

    public static string HMAC_MD5(string key, string value)
    {
        // The first two lines take the input values and convert them from strings to Byte arrays
        byte[] HMACkey = (new System.Text.ASCIIEncoding()).GetBytes(key);
        byte[] HMACdata = (new System.Text.ASCIIEncoding()).GetBytes(value);

        // create a HMACMD5 object with the key set
        System.Security.Cryptography.HMACMD5 myhmacMD5 = new System.Security.Cryptography.HMACMD5(HMACkey);

        //calculate the hash (returns a byte array)
        byte[] HMAChash = myhmacMD5.ComputeHash(HMACdata);

        //loop through the byte array and add append each piece to a string to obtain a hash string
        string fingerprint = "";
        for (int i = 0; i < HMAChash.Length; i++)
        {
            fingerprint += HMAChash[i].ToString("x").PadLeft(2, '0');
        }

        return fingerprint;
    }

    public static string HashBySHA256(string myPassword)
    {
        System.Text.UnicodeEncoding UE = new System.Text.UnicodeEncoding();
        byte[] hashValue;
        byte[] message = UE.GetBytes(myPassword);

        System.Security.Cryptography.SHA256Managed hashString = new System.Security.Cryptography.SHA256Managed();
        string hex = "";

        hashValue = hashString.ComputeHash(message);
        foreach (byte x in hashValue)
        {
            hex += String.Format("{0:x2}", x);
        }
        return hex;
    }

    public static string UploadThumbnail(System.Web.HttpPostedFile postedFile, int maxSize, int maxWidth, int maxHeight, string folderName)
    {
        List<string> AllowedFileTypes = new List<string>() { ".jpg", ".jpeg", ".gif", ".png", ".bmp" };

        if (postedFile.ContentLength <= maxSize * 1048576)
        {
            string strFileType = Path.GetExtension(postedFile.FileName).ToLower();

            if (AllowedFileTypes.Contains(strFileType))
            {
                try
                {
                    Image imageToBeResized = Image.FromStream(postedFile.InputStream);

                    int imageHeight = imageToBeResized.Height;
                    int imageWidth = imageToBeResized.Width;

                    imageHeight = (imageHeight * maxWidth) / imageWidth;
                    imageWidth = maxWidth;

                    if (imageHeight > maxHeight)
                    {
                        imageWidth = (imageWidth * maxHeight) / imageHeight;
                        imageHeight = maxHeight;
                    }

                    Bitmap thumbnail = new Bitmap(imageWidth, imageHeight);
                    Graphics graphic = Graphics.FromImage(thumbnail);
                    graphic.CompositingQuality = CompositingQuality.HighQuality;
                    graphic.SmoothingMode = SmoothingMode.HighQuality;
                    graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    Rectangle oRectangle = new Rectangle(0, 0, imageWidth, imageHeight);
                    graphic.DrawImage(imageToBeResized, oRectangle);
                    Random r = new Random();
                    string strNewFileName = HMAC_MD5(string.Empty, postedFile.FileName + DateTime.Now.ToLongTimeString()) + r.Next(10000).ToString() + strFileType;

                    thumbnail.Save(folderName + strNewFileName);

                    return strNewFileName;

                }
                catch { return "Error-3"; }
            } return "Error-2";
        } return "Error-1";
    }

    public static string UploadImage(System.Web.HttpPostedFile postedFile, int maxSize, string folderName)
    {
        List<string> AllowedFileTypes = new List<string>() { ".jpg", ".jpeg", ".gif", ".png", ".bmp" };

        if (postedFile.ContentLength <= maxSize * 1048576)
        {
            string strFileType = Path.GetExtension(postedFile.FileName).ToLower();

            if (AllowedFileTypes.Contains(strFileType))
            {
                try
                {
                    string strNewFileName = HMAC_MD5(string.Empty, postedFile.FileName + DateTime.Now.ToLongTimeString()) + strFileType;
                    postedFile.SaveAs(folderName + strNewFileName);

                    return strNewFileName;
                }
                catch { return "Error-3"; }
            } return "Error-2";
        } return "Error-1";
    }

    public static string GeneratePhrase(string text)
    {
        //'Dấu Ngang
        text = text.Replace("A", "A");
        text = text.Replace("a", "a");
        text = text.Replace("Ă", "A");
        text = text.Replace("ă", "a");
        text = text.Replace("Â", "A");
        text = text.Replace("â", "a");
        text = text.Replace("E", "E");
        text = text.Replace("e", "e");
        text = text.Replace("Ê", "E");
        text = text.Replace("ê", "e");
        text = text.Replace("I", "I");
        text = text.Replace("i", "i");
        text = text.Replace("O", "O");
        text = text.Replace("o", "o");
        text = text.Replace("Ô", "O");
        text = text.Replace("ô", "o");
        text = text.Replace("Ơ", "O");
        text = text.Replace("ơ", "o");
        text = text.Replace("U", "U");
        text = text.Replace("u", "u");
        text = text.Replace("Ư", "U");
        text = text.Replace("ư", "u");
        text = text.Replace("Y", "Y");
        text = text.Replace("y", "y");

        //    'Dấu Huyền
        text = text.Replace("À", "A");
        text = text.Replace("à", "a");
        text = text.Replace("Ằ", "A");
        text = text.Replace("ằ", "a");
        text = text.Replace("Ầ", "A");
        text = text.Replace("ầ", "a");
        text = text.Replace("È", "E");
        text = text.Replace("è", "e");
        text = text.Replace("Ề", "E");
        text = text.Replace("ề", "e");
        text = text.Replace("Ì", "I");
        text = text.Replace("ì", "i");
        text = text.Replace("Ò", "O");
        text = text.Replace("ò", "o");
        text = text.Replace("Ồ", "O");
        text = text.Replace("ồ", "o");
        text = text.Replace("Ờ", "O");
        text = text.Replace("ờ", "o");
        text = text.Replace("Ù", "U");
        text = text.Replace("ù", "u");
        text = text.Replace("Ừ", "U");
        text = text.Replace("ừ", "u");
        text = text.Replace("Ỳ", "Y");
        text = text.Replace("ỳ", "y");

        //'Dấu Sắc
        text = text.Replace("Á", "A");
        text = text.Replace("á", "a");
        text = text.Replace("Ắ", "A");
        text = text.Replace("ắ", "a");
        text = text.Replace("Ấ", "A");
        text = text.Replace("ấ", "a");
        text = text.Replace("É", "E");
        text = text.Replace("é", "e");
        text = text.Replace("Ế", "E");
        text = text.Replace("ế", "e");
        text = text.Replace("Í", "I");
        text = text.Replace("í", "i");
        text = text.Replace("Ó", "O");
        text = text.Replace("ó", "o");
        text = text.Replace("Ố", "O");
        text = text.Replace("ố", "o");
        text = text.Replace("Ớ", "O");
        text = text.Replace("ớ", "o");
        text = text.Replace("Ú", "U");
        text = text.Replace("ú", "u");
        text = text.Replace("Ứ", "U");
        text = text.Replace("ứ", "u");
        text = text.Replace("Ý", "Y");
        text = text.Replace("ý", "y");

        //'Dấu Hỏi
        text = text.Replace("Ả", "A");
        text = text.Replace("ả", "a");
        text = text.Replace("Ẳ", "A");
        text = text.Replace("ẳ", "a");
        text = text.Replace("Ẩ", "A");
        text = text.Replace("ẩ", "a");
        text = text.Replace("Ẻ", "E");
        text = text.Replace("ẻ", "e");
        text = text.Replace("Ể", "E");
        text = text.Replace("ể", "e");
        text = text.Replace("Ỉ", "I");
        text = text.Replace("ỉ", "i");
        text = text.Replace("Ỏ", "O");
        text = text.Replace("ỏ", "o");
        text = text.Replace("Ổ", "O");
        text = text.Replace("ổ", "o");
        text = text.Replace("Ở", "O");
        text = text.Replace("ở", "o");
        text = text.Replace("Ủ", "U");
        text = text.Replace("ủ", "u");
        text = text.Replace("Ử", "U");
        text = text.Replace("ử", "u");
        text = text.Replace("Ỷ", "Y");
        text = text.Replace("ỷ", "y");

        //'Dấu Ngã    
        text = text.Replace("Ã", "A");
        text = text.Replace("ã", "a");
        text = text.Replace("Ẵ", "A");
        text = text.Replace("ẵ", "a");
        text = text.Replace("Ẫ", "A");
        text = text.Replace("ẫ", "a");
        text = text.Replace("Ẽ", "E");
        text = text.Replace("ẽ", "e");
        text = text.Replace("Ễ", "E");
        text = text.Replace("ễ", "e");
        text = text.Replace("Ĩ", "I");
        text = text.Replace("ĩ", "i");
        text = text.Replace("Õ", "O");
        text = text.Replace("õ", "o");
        text = text.Replace("Ỗ", "O");
        text = text.Replace("ỗ", "o");
        text = text.Replace("Ỡ", "O");
        text = text.Replace("ỡ", "o");
        text = text.Replace("Ũ", "U");
        text = text.Replace("ũ", "u");
        text = text.Replace("Ữ", "U");
        text = text.Replace("ữ", "u");
        text = text.Replace("Ỹ", "Y");
        text = text.Replace("ỹ", "y");

        //'Dẫu Nặng
        text = text.Replace("Ạ", "A");
        text = text.Replace("ạ", "a");
        text = text.Replace("Ặ", "A");
        text = text.Replace("ặ", "a");
        text = text.Replace("Ậ", "A");
        text = text.Replace("ậ", "a");
        text = text.Replace("Ẹ", "E");
        text = text.Replace("ẹ", "e");
        text = text.Replace("Ệ", "E");
        text = text.Replace("ệ", "e");
        text = text.Replace("Ị", "I");
        text = text.Replace("ị", "i");
        text = text.Replace("Ọ", "O");
        text = text.Replace("ọ", "o");
        text = text.Replace("Ộ", "O");
        text = text.Replace("ộ", "o");
        text = text.Replace("Ợ", "O");
        text = text.Replace("ợ", "o");
        text = text.Replace("Ụ", "U");
        text = text.Replace("ụ", "u");
        text = text.Replace("Ự", "U");
        text = text.Replace("ự", "u");
        text = text.Replace("Ỵ", "Y");
        text = text.Replace("ỵ", "y");
        text = text.Replace("Đ", "D");
        text = text.Replace("đ", "d");
        text = text.Replace("'", "");

        text = text.Trim();
        text = Regex.Replace(text, @"[^a-zA-Z0-9\-\s]", string.Empty);
        text = Regex.Replace(text, @"\s+", "-");
        return text.ToLower();
    }

    public static string GenerateRandomCode()
    {
        // For generating random numbers.
        Random random = new Random();

        string s = "";
        for (int i = 0; i < 6; i++)
            s = String.Concat(s, random.Next(10).ToString());
        return s;
    }

    /// <summary>
    /// Chuyển chuỗi tiếng việt có dấu sang không dấu dạng : chuoi-tieng-viet
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Encode(string text)
    {
        if (text == null || text == string.Empty)
            return string.Empty;

        // Fields
        String[] pattern = new String[7];
        Char[] replaceChar = new Char[14];

        // Khởi tạo giá trị thay thế
        replaceChar[0] = 'a';
        replaceChar[1] = 'd';
        replaceChar[2] = 'e';
        replaceChar[3] = 'i';
        replaceChar[4] = 'o';
        replaceChar[5] = 'u';
        replaceChar[6] = 'y';
        replaceChar[7] = 'A';
        replaceChar[8] = 'D';
        replaceChar[9] = 'E';
        replaceChar[10] = 'I';
        replaceChar[11] = 'O';
        replaceChar[12] = 'U';
        replaceChar[13] = 'Y';

        //Mẫu cần thay thế tương ứng
        pattern[0] = "(á|à|ả|ã|ạ|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ)"; //letter a
        pattern[1] = "đ";   //letter d
        pattern[2] = "(é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ)"; //letter e
        pattern[3] = "(í|ì|ỉ|ĩ|ị)"; //letter i
        pattern[4] = "(ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ)"; //letter o
        pattern[5] = "(ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự)"; //letter u
        pattern[6] = "(ý|ỳ|ỷ|ỹ|ỵ)"; //letter y

        //Thay the cac ky tu Tieng Viet co dau thanh khong dau
        char[] arr = replaceChar;
        for (int i = 0; i < pattern.Length; i++)
        {
            MatchCollection matchs = Regex.Matches(text, pattern[i], RegexOptions.IgnoreCase);
            foreach (Match m in matchs)
            {
                char ch = Char.IsLower(m.Value[0]) ? arr[i] : arr[i + 7];
                text = text.Replace(m.Value[0], ch);
            }
        }

        //Loại bỏ các ký tự đặc biệt
        text = text.Trim();
        text = text.Replace(" - ", "-");
        text = Regex.Replace(text, @"[^a-zA-Z0-9\-\s]", string.Empty);
        text = Regex.Replace(text, @"\s+", "-");

        //Chuyển tất cả về dạng chữ thường
        text = text.ToLower();

        return text;
    }


    public static void SaveAs(System.Web.HttpPostedFileBase postedFile, int maxWidth, int maxHeight, string folderName, string strNewFileName)
    {
        string strFileType = Path.GetExtension(postedFile.FileName).ToLower();
        Image imageToBeResized = Image.FromStream(postedFile.InputStream);

        int imageHeight = imageToBeResized.Height;
        int imageWidth = imageToBeResized.Width;

        imageHeight = (imageHeight * maxWidth) / imageWidth;
        imageWidth = maxWidth;

        if (imageHeight > maxHeight)
        {
            imageWidth = (imageWidth * maxHeight) / imageHeight;
            imageHeight = maxHeight;
        }

        Bitmap thumbnail = new Bitmap(imageWidth, imageHeight);
        Graphics graphic = Graphics.FromImage(thumbnail);
        graphic.CompositingQuality = CompositingQuality.HighQuality;
        graphic.SmoothingMode = SmoothingMode.HighQuality;
        graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
        Rectangle oRectangle = new Rectangle(0, 0, imageWidth, imageHeight);
        graphic.DrawImage(imageToBeResized, oRectangle);

        thumbnail.Save(HttpContext.Current.Server.MapPath(folderName) + strNewFileName, ImageFormat.Jpeg);
    }

}