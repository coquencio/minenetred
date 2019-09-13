using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minenetred.web.Infrastructure
{
    public interface IEncryptionService
    {
        String Encrypt(string encryptString);
        string Decrypt(string cipherText);
    }
}
