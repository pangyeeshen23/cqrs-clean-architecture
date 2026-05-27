using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Application.Common.Hasher
{
    internal class MD5Hasher
    {
        public static string ToMd5(string input)
        {
            using var md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes).ToLower();
        }
    }
}
