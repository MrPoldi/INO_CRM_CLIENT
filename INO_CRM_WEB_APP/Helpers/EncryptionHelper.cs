using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace INO_CRM_WEB_APP.Helpers
{
    public static class EncryptionHelper
    {
        public static string GetMd5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            byte[] result = md5.Hash;

            StringBuilder stringBuilder = new StringBuilder();
            foreach(byte r in result)
            {
                stringBuilder.Append(r.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
