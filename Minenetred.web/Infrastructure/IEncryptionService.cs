using System;

namespace Minenetred.Web.Infrastructure
{
    public interface IEncryptionService
    {
        String Encrypt(string encryptString);

        string Decrypt(string cipherText);
    }
}