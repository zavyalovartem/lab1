using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1Clean
{
    class RSA
    {

        private BigInt[] openKeys;
        private BigInt[] privateKeys;

        public void GenerateKeyPair()
        {
            var primeNumbers = File.ReadAllText("primes.txt", Encoding.UTF8).Split(' ');
            var rnd = new Random();
            var pPosition = rnd.Next(0, primeNumbers.Length);
            var qPosition = rnd.Next(0, primeNumbers.Length);
            var p = new BigInt(primeNumbers[pPosition]);
            var q = new BigInt(primeNumbers[qPosition]);
            var n = p * q;
            var one = new BigInt(1);
            var fi = (p - one) * (q - one);
            var e = new BigInt(primeNumbers[rnd.Next(0, primeNumbers.Length)]);
            while (BigInt.GreatestCommonDivisor(e, fi, out _, out _) != one || e >= fi)
            {
                e = new BigInt(primeNumbers[rnd.Next(0, primeNumbers.Length)]);
            }
            var d = e.ModInverse(fi);
            openKeys = new[] { e, n };
            privateKeys = new[] { d, n };
        }

        public string Encrypt(string text, BigInt e, BigInt n)
        {
            var encryption = new List<string>();

            foreach (var elem in text)
            {
                encryption.Add(new BigInt(elem).ModPow(e, n).ToString());
            }

            return String.Join(' ', encryption);
        }

        public string Decrypt(string encryption, BigInt d, BigInt n)
        {
            var res = new List<char>();
            var splitted = encryption.Split(' ');

            foreach (var elem in splitted)
            {
                var mp = new BigInt(elem).ModPow(d, n);
                res.Add((char)Convert.ToInt32(mp.ToString()));

            }

            return String.Join("", res);
        }

        public BigInt[] getOpenKeys()
        {
            return openKeys;
        }

        public BigInt[] getPrivateKeys()
        {
            return privateKeys;
        }
    }
}
