using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Core.Security
{
	public class CryptographyService :ICryptographyService
	{
		public string EncryptPassword(string unencryptedPassword)
		{
			SHA512CryptoServiceProvider x = new SHA512CryptoServiceProvider();

			byte[] data = Encoding.ASCII.GetBytes(unencryptedPassword);
			data = x.ComputeHash(data);

			return Encoding.ASCII.GetString(data);
		}

        public string GetRandomString(int length, bool urlSafe = false, bool humanReadable = true)
        {
            if (length < 0)
                return string.Empty;

            StringBuilder str = new StringBuilder();

            while (str.Length < length)
            {
                str.Append(Guid.NewGuid().ToString());
            }

            string s = str.ToString();

            if (s.Length > length)
                s = s.Substring(0, length);

            return s;
        }
	}
}