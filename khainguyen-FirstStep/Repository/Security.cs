using System;
using System.Security.Cryptography;
using System.Text;

namespace MvcLibrary.Repository
{
    public class Security
    {
        public string GetHashPassword(string myPassword)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(myPassword);

            SHA256Managed hashString = new SHA256Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }
    }
}