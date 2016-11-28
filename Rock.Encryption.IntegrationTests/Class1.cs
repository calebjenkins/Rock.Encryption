using System;
using System.IO;
using NUnit.Framework;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Rock.Encryption.Bcl;
using Rock.Encryption.Bcl.Configuration.Xml;

namespace Rock.Encryption.IntegrationTests
{
    public class Class1
    {
        [Test]
        public void FooBar()
        {
            //var r = new RNGCryptoServiceProvider();

            //var plainTextKey = new byte[32]; r.GetBytes(plainTextKey);
            //var plainTextKeyString = Convert.ToBase64String(plainTextKey);
            //// 

            //var entropy = new byte[16]; r.GetBytes(entropy);
            //var entropyString = Convert.ToBase64String(entropy);
            //// 

            //var cipherTextKey = ProtectedData.Protect(plainTextKey, entropy, DataProtectionScope.CurrentUser);
            //var cipherTextKeyString = Convert.ToBase64String(cipherTextKey);
            //// 

            //var plainTextKey2 = ProtectedData.Unprotect(cipherTextKey, entropy, DataProtectionScope.CurrentUser);
            //var plainTextKey2String = Convert.ToBase64String(plainTextKey2);
            //// 

            var encrypted = Crypto.Encrypt("Hello, world!", "unprotectedKey");
            var decrypted = Crypto.Decrypt(encrypted, "protectedKey");

            Assert.That(decrypted, Is.EqualTo("Hello, world!"));
        }
    }
}
