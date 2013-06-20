using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Security
{
    public interface ICryptographyService
    {
        string EncryptPassword(string unencryptedPassword);
        string GetRandomString(int length, bool urlSafe = false, bool humanReadable = true);
    }
}