using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1Clean
{
    [TestFixture]
    class RSATests
    {
        [TestCase(@",.-'""!?:;'/\|@#$%^&*()=_{}[]<>~`—№")]
        [TestCase("0123456789")]
        [TestCase("АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ")]
        [TestCase("абвгдеёжзийклмнопрстуфхцчшщъыьэюя")]
        [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [TestCase("abcdefghijklmnopqrstuvwxyz")]
        [TestCase("Тест")]
        [TestCase("Тест более длинного текста dsfadsdsa")]
        public void RSATesting(string text)
        {
            var rsa = new RSA();
            rsa.GenerateKeyPair();
            var openKey = rsa.getOpenKeys();
            var e = openKey[0];
            var n = openKey[1];
            var ciphered = rsa.Encrypt(text, e, n);
            var privateKey = rsa.getPrivateKeys();
            var d = privateKey[0];
            var res = rsa.Decrypt(ciphered, d, n);
            Assert.AreEqual(text, res);
        }
    }
}
