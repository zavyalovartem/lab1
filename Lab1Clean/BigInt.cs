using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1Clean
{
    class BigInt
    {
        private List<int> number;
        private int sign;
        public List<int> Number
        {
            get => number;
            set
            {
                number = value;
            }
        }
        public int Sign
        {
            get => sign;
            set
            {
                sign = value;
            }
        }

        public BigInt()
        {
            Number = new List<int>();
            Number.Add(0);
            sign = 1;
        }

        public BigInt(long num)
        {
            if (num == 0)
            {
                Number = new List<int>(0);
                sign = 1;
            }
            else
            {
                sign = num > 0 ? 1 : -1;
                Number = new List<int>();
                while (num > 0)
                {
                    Number.Add((int) num % 10);
                    num /= 10;
                }
            }
            Number.Reverse();
        }

        public BigInt(int otherSign, List<int> otherNumber)
        {
            if (otherNumber.Any(digit => digit < 0 || digit > 9))
            {
                throw new ArgumentException("Все числа в листе должны быть от 1 до 9");
            }
            Sign = otherSign;
            Number = new List<int>(otherNumber);
        }

        public BigInt(string otherNumber)
        {
            Number = new List<int>();
            var i = 0;
            sign = 1;
            if (otherNumber[0].Equals('-'))
            {
                sign = -1;
                i = 1;
            }
            if (otherNumber[0].Equals('+'))
            {
                i = 1;
            }
            for (; i < otherNumber.Length; i++)
            {
                if (!Char.IsDigit(otherNumber[i]))
                {
                    throw new ArgumentException("Не число!");
                }
                number.Add(Int32.Parse(otherNumber[i].ToString()));
            }
        }

        public BigInt(BigInt otherNumber)
        {
            Sign = otherNumber.Sign;
            Number = new List<int>(otherNumber.Number);
        }

        public static BigInt operator +(BigInt number, BigInt otherNumber)
        {
            var sum = new BigInt(1, new List<int>());
            if (number.Number.Count > otherNumber.Number.Count)
            {
                otherNumber.AddZeros(number.Number.Count);
            }
            else if (number.Number.Count < otherNumber.Number.Count)
            {
                number.AddZeros(otherNumber.Number.Count);
            }

            if (number.Sign == otherNumber.Sign)
            {
                sum.Sign = number.Sign;
                var r = 0;
                var d = 0;
                for (var i = number.Number.Count - 1; i >= 0; i--)
                {
                    d = Mod(number.Number[i] + otherNumber.Number[i] + r, 10);
                    r = Div(number.Number[i] + otherNumber.Number[i] + r, 10);
                    sum.Number.Add(d);
                }
                if (r > 0)
                {
                    sum.Number.Add(1);
                }
            }
            else
            {
                if (number.CompareToWithoutSign(otherNumber) == 1)
                {
                    var r = 0;
                    var d = 0;
                    for (var i = number.Number.Count - 1; i >= 0; i--)
                    {
                        d = Mod(number.Number[i] - otherNumber.Number[i] + r, 10);
                        r = Div(number.Number[i] - otherNumber.Number[i] + r, 10);
                        sum.Number.Add(d);
                    }
                    sum.Sign = number.Sign;
                }
                else if (number.CompareToWithoutSign(otherNumber) == -1)
                {
                    var r = 0;
                    var d = 0;
                    for (var i = number.Number.Count - 1; i >= 0; i--)
                    {
                        d = Mod(otherNumber.Number[i] - number.Number[i] + r, 10);
                        r = Div(otherNumber.Number[i] - number.Number[i] + r, 10);
                        sum.Number.Add(d);
                    }
                    sum.Sign = number.Sign == 1 ? -1 : 1;
                }
                else
                {
                    sum.Number.Add(0);
                    sum.Sign = 1;
                }
            }
            sum.Number.Reverse();
            number.RemoveZeros();
            otherNumber.RemoveZeros();
            sum.RemoveZeros();
            return sum;
        }

        public static BigInt operator *(BigInt a, BigInt b)
        {
            if (a == new BigInt() || b == new BigInt())
            {
                return new BigInt();
            }
            var sum = new BigInt();
            for (var i = 0; i < b.Number.Count; i++)
            {
                var buf = a.MultOnDigit(b.Number[i]);
                buf = buf.MultOn10(b.Number.Count - i - 1);
                sum += buf;
            }
            sum.Sign = a.Sign * b.Sign;
            return sum;
        }


        //Unary Minus
        public static BigInt operator -(BigInt number)
        {
            if (number == new BigInt())
            {
                return new BigInt();
            }
            var sign = -number.Sign;
            var buf = new BigInt(sign, number.Number);
            return buf;
        }

        //Regular Minus
        public static BigInt operator -(BigInt a, BigInt b)
        {
            var buf = -b;
            buf += a;
            return buf;
        }

        public static BigInt operator /(BigInt a, BigInt b)
        {
            if (b == new BigInt())
            {
                throw new DivideByZeroException("На 0 делить нельзя");
            }

            if (a == new BigInt())
            {
                return new BigInt();
            }

            if (a == b)
            {
                return new BigInt("1");
            }

            if (a.CompareToWithoutSign(b) < 0)
            {
                return new BigInt();
            }

            var n = a.Number.Count;
            var t = b.Number.Count;
            var buf = new BigInt();
            var res = new BigInt(1, new List<int>());
            var sign = a.Sign / b.Sign;
            a.Sign = 1;
            b.Sign = 1;
            var cur = 0;
            for (var i = n; i >= t; i--)
            {
                buf = b.MultOn10(i - t);
                while (a >= buf)
                {
                    cur++;
                    a -= buf;
                }

                res.Number.Add(cur);
                cur = 0;
            }
            res.RemoveZeros();
            res.Sign = sign;
            return res;
        }

        public static BigInt operator %(BigInt a, BigInt b)
        {
            if (a == b)
            {
                return new BigInt("1");
            }

            if (a.CompareToWithoutSign(b) < 0)
            {
                return a;
            }

            var sign = a.Sign * b.Sign;
            var u = new BigInt(1, a.Number);
            var v = new BigInt(1, b.Number);
            var buf = u - ((u / v) * v);
            buf.Sign = sign;
            return buf;
        }

        public static bool operator >(BigInt a, BigInt b) => a.CompareTo(b) > 0;
        public static bool operator >=(BigInt a, BigInt b) => a.CompareTo(b) >= 0;
        public static bool operator <(BigInt a, BigInt b) => a.CompareTo(b) < 0;
        public static bool operator <=(BigInt a, BigInt b) => a.CompareTo(b) <= 0;
        public static bool operator ==(BigInt a, BigInt b) => a.CompareTo(b) == 0;
        public static bool operator !=(BigInt a, BigInt b) => a.CompareTo(b) != 0;

        public BigInt ModPow(BigInt pow, BigInt n)
        {
            if (pow.Sign == -1)
            {
                return new BigInt();
            }

            if (pow == new BigInt())
            {
                return new BigInt("1");
            }

            var a = new BigInt(this);
            var b = new BigInt("1");
            var zero = new BigInt();
            var one = new BigInt("1");
            while (pow != zero)
            {
                if (pow.ModOnDigit(2) == 0)
                {
                    pow = pow.DivOnDigit(2);
                    a = (a * a) % n;
                }
                else
                {
                    pow -= one;
                    b = (b * a) % n;
                }
            }
            return b;
        }

        public static BigInt GreatestCommonDivisor(BigInt a, BigInt b, out BigInt u, out BigInt v)
        {
            if (a == new BigInt())
            {
                u = new BigInt();
                v = new BigInt("1");
                return b;
            }

            var d = GreatestCommonDivisor(b % a, a, out var u1, out var v1);
            u = v1 - (b / a) * u1;
            v = u1;
            return d;
        }

        public BigInt ModInverse(BigInt b)
        {
            var a = new BigInt(this);
            var res = new BigInt();
            var gcd = GreatestCommonDivisor(a, b, out res, out _);
            if (gcd == new BigInt("1"))
            {
                res = (res % b + b) % b;
            }
            else
            {
                throw new Exception("Невозможно найти обратный элемент");
            }
            return res;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 48611;
                hash = (hash * 23537) ^ this.Sign.GetHashCode();
                var res = this.Number;
                return this.Number.Aggregate(hash, (cur, dig) => (cur * 23497) ^ dig.GetHashCode());
            }
        }

        public override bool Equals(object obj)
        {
            var n = (BigInt)obj;
            return !(n is null) && n.GetHashCode() == this.GetHashCode();
        }

        private static int _r;

        public BigInt DivOnDigit(int digit)
        {
            if (digit == 0)
            {
                throw new DivideByZeroException("На 0 разделить нельзя");
            }

            if (digit < 0 || digit > 9)
            {
                throw new ArgumentException("Число должно быть от 1 до 9");
            }

            var res = new BigInt(1, new List<int>());
            _r = 0;
            for (var i = 0; i < this.Number.Count; i++)
            {
                res.Number.Add(Div(_r * 10 + this.Number[i], digit));
                _r = Mod(_r * 10 + this.Number[i], digit);
            }
            res.Sign = this.Sign;
            res.RemoveZeros();
            return res;
        }

        public int ModOnDigit(int digit)
        {
            DivOnDigit(digit);
            return _r;
        }

        private static int Mod(int a, int b)
        {
            return a - b * Div(a, b);
        }

        private static int Div(int a, int b)
        {
            if (a >= 0)
                return a / b;
            if (a % b != 0)
            {
                return -(-a / b + 1);
            }
            return -(-a / b);
        }

        private int CompareTo(object obj)
        {
            BigInt compared = (BigInt)obj;

            if (compared.Sign != this.Sign)
            {
                return compared.Sign > this.Sign ? -1 : 1;
            }

            var res = this.CompareToWithoutSign(compared);
            return this.Sign == -1 ? -res : res;
        }

        private int CompareToWithoutSign(object obj)
        {
            BigInt compared = (BigInt)obj;
            if (compared.Number.Count > this.Number.Count)
            {
                return -1;
            }

            if (compared.Number.Count < this.Number.Count)
            {
                return 1;
            }

            for (var i = 0; i < compared.Number.Count; i++)
            {
                if (compared.Number[i] > this.Number[i])
                {
                    return -1;
                }
                else if (compared.Number[i] < this.Number[i])
                {
                    return 1;
                }
            }

            return 0;
        }


        public BigInt MultOnDigit(int digit)
        {
            var buf = new BigInt();
            if (digit < 0 || digit > 9)
                throw new ArgumentException("Число должно находиться в диапазоне от 0 до 9 включительно");
            if (digit == 0)
            {
                return buf;
            }
            if (digit == 1)
            {
                return this;
            }
            buf = this;
            for (var i = 1; i < digit; i++)
            {
                buf += this;
            }
            return buf;
        }

        public BigInt MultOn10(int n)
        {
            BigInt buf = new BigInt(this.Sign, this.Number);
            for (var i = 0; i < n; i++)
                buf.Number.Add(0);
            return buf;
        }

        private void AddZeros(int amount)
        {
            var res = new List<int>();
            var cnt = amount - Number.Count;
            for (var i = 0; i < cnt; i++)
            {
                res.Add(0);
            }
            res.AddRange(Number);
            Number = res;
        }

        private void RemoveZeros()
        {
            for (var i = 0; i < Number.Count; i++)
            {
                if (Number[i] != 0)
                {
                    Number.RemoveRange(0, i);
                    break;
                }
            }
        }

        public override string ToString()
        {
            var res = new List<string>();
            if (Sign == -1)
            {
                res.Add("-");
            }
            res.AddRange(Number.Select(dig => dig.ToString()));
            return String.Join("", res);
        }
    }
}
