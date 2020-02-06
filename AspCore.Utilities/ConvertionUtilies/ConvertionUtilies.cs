using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AspCore.Utilities
{
    public class ConvertionUtilies
    {
        public static Guid GetMd5HashFromObject(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            return new Guid(GetMd5HashBytesFromObject(obj));
        }

        public static byte[] GetMd5HashBytesFromObject(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            using (MD5 md5 = MD5.Create())
            {
                return md5.ComputeHash(GetBytesFromObject(obj));
            }
        }

        public static byte[] GetBytesFromObject(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}
