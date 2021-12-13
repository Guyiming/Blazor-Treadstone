using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpsMain.Shared.Toolkit
{
    public static class CustomExtensions
    {
        public static string ToSHA256(this string input)
        {
            using(SHA256 sha=SHA256.Create())
            {
                byte[] values = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder();
                for(int i=0;i<values.Length;i++)
                {
                    sb.Append($"{values[i]:X2}");
                }
                return sb.ToString();
            }
        }
        public static string ListToString<T>(this List<T> list,string separator)
        {
            return string.Join(separator, list);
        }
    }
}
