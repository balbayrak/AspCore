using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AspCore.Utilities
{
    public static class HashingHelper
    {
        public static void CreateHash(string text, out byte[] hash, out byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
            }
        }

        public static string CreateHash(string text, string key)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            using (MemoryStream stream = new MemoryStream(encoding.GetBytes(key)))
            {
                using (var hmac = new System.Security.Cryptography.HMACSHA512(stream.ToArray()))
                {
                    Byte[] bytes = hmac.ComputeHash(encoding.GetBytes(text));

                    return System.Convert.ToBase64String(bytes);
                }
            }
        }



        public static bool VerifyHash(string text, byte[] hash, byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != hash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
