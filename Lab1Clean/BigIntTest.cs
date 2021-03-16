using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1Clean
{
    [TestFixture]
    class BigIntTest
    {
        [Test]
        public void TestOnEmptyConstructor()
        {
            var bigInt = new BigInt();
            Assert.AreEqual(1, bigInt.Sign);
            var number = new List<int>();
            number.Add(0);
            Assert.AreEqual(number, bigInt.Number);
        }

        [TestCase("134", 1, new[] { 1, 3, 4 })]
        [TestCase("+134", 1, new[] { 1, 3, 4 })]
        [TestCase("-134", -1, new[] { 1, 3, 4 })]
        public void TestOnStringConstructor(string num, int sign, int[] digits)
        {
            var bigInt = new BigInt(num);
            Assert.AreEqual(sign, bigInt.Sign);
            Assert.AreEqual(digits, bigInt.Number);
        }

        [TestCase("3123a")]
        [TestCase("a132232")]
        [TestCase("-a2deqw//as2")]
        [TestCase("+a232")]
        [TestCase("+asd2")]
        public void TestStringConstructorWrongString(string num)
        {
            var exception = Assert.Throws<ArgumentException>(() => { new BigInt(num); });
            if (exception != null)
                Assert.AreEqual("Не число!", exception.Message);
        }

        [Test]
        public void TestBigIntConstructor()
        {
            BigInt expected = new BigInt("123");
            BigInt actual = new BigInt(expected);
            Assert.AreEqual(actual.Sign, expected.Sign);
            Assert.AreEqual(actual.Number, expected.Number);
        }

        [TestCase(-1, new[] { 1, 8 })]
        [TestCase(-1, new[] { 2, 4 })]
        public void TestOnSignAndDigitsConstructor(int sign, int[] digits)
        {
            BigInt bigInt = new BigInt(sign, digits.ToList());
            Assert.AreEqual(sign, bigInt.Sign);
            Assert.AreEqual(digits, bigInt.Number);
        }

        [Test]
        public void TestOnSignAndDigitsConstructorWrongDigits()
        {
            var sign = -1;
            var digits = new List<int> { 1, 432, 6 };
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var bigInt = new BigInt(sign, digits);
            });
            if (exception != null) Assert.AreEqual("Все числа в листе должны быть от 1 до 9", exception.Message);
        }

        [TestCase("456123", 4, "114030")]
        [TestCase("-1243567", 4, "-310891")]
        [TestCase("12", 4, "3")]
        [TestCase("1", 8, "0")]
        public void DivOnDigitTest(string num, int v, string res)
        {
            var bigInt = new BigInt(num);
            var expected = new BigInt(res);
            Assert.AreEqual(expected, bigInt.DivOnDigit(v));
        }

        [TestCase("14", "-14")]
        [TestCase("-22", "22")]
        [TestCase("0", "0")]
        public void TestOnUnaryMinus(string num, string res)
        {
            var bigInt1 = new BigInt(num);
            var expected = new BigInt(res);
            Assert.AreEqual(expected, -bigInt1);
        }

        [TestCase("123456789", "987654321", "1111111110")]
        [TestCase("-123456789", "-987654321", "-1111111110")]
        [TestCase("-123456789", "987654321", "864197532")]
        [TestCase("1234", "-1234", "0")]
        public void AddTest(string num1, string num2, string res)
        {
            var first = new BigInt(num1);
            var second = new BigInt(num2);
            var expected = new BigInt(res);
            Assert.AreEqual(expected, first + second);
        }

        [TestCase("4587", "98", "4489")]
        [TestCase("-123", "-98", "-25")]
        [TestCase("-123", "98", "-221")]
        [TestCase("123", "-98", "221")]
        [TestCase("123", "123", "0")]
        [TestCase("100", "95", "5")]
        public void SubtractionTest(string num1, string num2, string res)
        {
            var bigInt1 = new BigInt(num1);
            var bigInt2 = new BigInt(num2);
            var expected = new BigInt(res);
            Assert.AreEqual(expected, bigInt1 - bigInt2);
        }

        [TestCase("134", 0, "0")]
        [TestCase("134", 1, "134")]
        [TestCase("65", 4, "260")]
        [TestCase("-65", 4, "-260")]
        public void TestOnMultOnDigit(string num, int digit, string res)
        {
            var bigInt = new BigInt(num);
            var expected = new BigInt(res);
            Assert.AreEqual(expected, bigInt.MultOnDigit(digit));
        }

        [TestCase("134", -1)]
        [TestCase("134", 11)]
        public void TestOmMultOnNonDigit(string num, int digit)
        {
            var exception = Assert.Throws<ArgumentException>(() => { new BigInt(num).MultOnDigit(digit); });
            if (exception != null) Assert.AreEqual("Число должно находиться в диапазоне от 0 до 9 включительно", exception.Message);
        }

        [TestCase("12", 1, "120")]
        [TestCase("-12", 1, "-120")]
        [TestCase("12", 6, "12000000")]
        [TestCase("12", 0, "12")]
        public void TestOnMultOn10(string num, int pow, string res)
        {
            var bigInt = new BigInt(num);
            var expected = new BigInt(res);
            Assert.AreEqual(expected, bigInt.MultOn10(pow));
        }

        [TestCase("123", "145", "17835")]
        [TestCase("-123", "145", "-17835")]
        [TestCase("-123", "-145", "17835")]
        [TestCase("-123", "0", "0")]
        [TestCase("123", "0", "0")]
        public void TestOnMultOnBigInt(string num1, string num2, string res)
        {
            var bigInt1 = new BigInt(num1);
            var bigInt2 = new BigInt(num2);
            var expected = new BigInt(res);
            Assert.AreEqual(expected, bigInt1 * bigInt2);
        }

        [TestCase("1231234567123456789123456891234567812345123457123456",
            "1231234567123456789123456891234567812345123457123456", true)]
        [TestCase("-1231234567123456789123456891234567812345123457123456",
            "-1231234567123456789123456891234567812345123457123456", true)]
        [TestCase("1231234567123456789123456891234567812345123457123456",
            "-1231234567123456789123456891234567812345123457123456", false)]
        [TestCase("24324324", "87998756", false)]
        [TestCase("2312", "0", false)]
        [TestCase("0", "0", true)]
        public void EqualsTest(string num1, string num2, bool res)
        {
            var bigInt1 = new BigInt(num1);
            var bigInt2 = new BigInt(num2);
            Assert.AreEqual(res, bigInt1 == bigInt2);
            Assert.AreEqual(!res, bigInt1 != bigInt2);
        }

        [TestCase("1231234567123456789123456891234567812345123457123456",
            "123123456712345678912345689123456781234512345712", true)]
        [TestCase("-1231234567123456789123456891234567812345123457123456",
            "-1231234567123456789123456891234567812345123457123", false)]
        [TestCase("1231234567123456789123456891234567812345123457123456",
            "-1231234567123456789123456891234567812345123457123456", true)]
        [TestCase("23134543655", "32556456", true)]
        [TestCase("6546555", "0", true)]
        [TestCase("0", "32324224", false)]
        [TestCase("-8", "-5", false)]
        public void CompareTest(string num1, string num2, bool res)
        {
            var bigInt1 = new BigInt(num1);
            var bigInt2 = new BigInt(num2);
            Assert.AreEqual(res, bigInt1 > bigInt2);
            Assert.AreEqual(!res, bigInt1 < bigInt2);
        }

        [Test]
        public void DigitDivisionOnZeroTest()
        {
            var exception = Assert.Throws<DivideByZeroException>(() => { new BigInt("33121").DivOnDigit(0); });
            if (exception != null) Assert.AreEqual("На 0 разделить нельзя", exception.Message);
        }

        [TestCase(-4)]
        [TestCase(19)]
        public void DivOnDigitWrongDigitTest(int v)
        {
            var exception = Assert.Throws<ArgumentException>(() => { new BigInt("31221").DivOnDigit(v); });
            if (exception != null) Assert.AreEqual("Число должно быть от 1 до 9", exception.Message);
        }

        [TestCase("10", 3, 1)]
        [TestCase("450", 5, 0)]
        public void TestOnModOnDigit(string num, int n, int res)
        {
            var bigInt = new BigInt(num);
            ;
            Assert.AreEqual(res, bigInt.ModOnDigit(n));
        }

        [TestCase("200", "28", "7")]
        [TestCase("-200", "28", "-7")]
        [TestCase("0", "2", "0")]
        [TestCase("-200", "-28", "7")]
        [TestCase("-200", "3", "-66")]
        [TestCase("-200", "5", "-40")]
        [TestCase("123456789123456789", "23", "5367686483628556")]
        public void BigIntDivisionTest(string num1, string num2, string res)
        {
            var a = new BigInt(num1);
            var b = new BigInt(num2);
            var expected = new BigInt(res);
            Assert.AreEqual(expected, a / b);
        }


        [Test]
        public void BigIntZeroDivisionTest()
        {
            var zero = new BigInt();
            var num = new BigInt("1133122");
            var exception = Assert.Throws<DivideByZeroException>(() =>
            {
                var res = num / zero;
            });
            if (exception != null) Assert.AreEqual("На 0 делить нельзя", exception.Message);
        }


        [TestCase("200", "28", "4")]
        [TestCase("-200", "28", "-4")]
        [TestCase("0", "2", "0")]
        [TestCase("-200", "-28", "4")]
        [TestCase("123456789123456789", "23", "1")]
        public void ModBigIntegerTest(string num1, string num2, string res)
        {
            var a = new BigInt(num1);
            var b = new BigInt(num2);
            var expected = new BigInt(res);
            Assert.AreEqual(expected, a % b);
        }


        [TestCase("15", "2", "10", "5")]
        [TestCase("-15", "2", "10", "5")]
        [TestCase("-15", "3", "10", "-5")]
        [TestCase("21312", "32434", "5456", "64")]
        [TestCase("21312", "5", "5456", "5424")]
        public void TestOnPow(string num1, string num2, string n, string res)
        {
            var bigInt = new BigInt(num1);
            var pow = new BigInt(num2);
            var mod = new BigInt(n);
            var expected = new BigInt(res);
            Assert.AreEqual(expected, bigInt.ModPow(pow, mod));
        }

        [TestCase("11", "15", "11")]
        [TestCase("2423", "24321", "2138")]
        public void ModInversionTest(string num, string n, string res)
        {
            var bigInt = new BigInt(num);
            var mod = new BigInt(n);
            var expected = new BigInt(res);
            Assert.AreEqual(expected, bigInt.ModInverse(mod));
        }

        [TestCase("111", "15")]
        [TestCase("120", "5")]
        public void InversionNotExistTest(string num, string n)
        {
            var bigInt = new BigInt(num);
            var mod = new BigInt(n);
            var exception = Assert.Throws<Exception>(() => { bigInt.ModInverse(mod); });
            if (exception != null)
                Assert.AreEqual("Невозможно найти обратный элемент", exception.Message);
        }

        [TestCase("213", "31242", "3")]
        [TestCase("12345", "67890", "15")]
        public void GreatestCommonDivisorTest(string num1, string num2, string res)
        {
            var a = new BigInt(num1);
            var b = new BigInt(num2);
            var expected = new BigInt(res);
            Assert.AreEqual(expected, BigInt.GreatestCommonDivisor(a, b, out _, out _));
        }
    }
}
