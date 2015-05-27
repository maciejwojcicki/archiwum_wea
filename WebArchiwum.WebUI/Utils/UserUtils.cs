using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebArchiwum.WebUI.Utils
{
    public class UserUtils
    {
        public static string PasswordHash(string text)
        {
            var hashAlgorithm = HashAlgorithm.Create("SHA1");
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(text);
            var hash = hashAlgorithm.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}