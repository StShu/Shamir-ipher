using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Microsoft.VisualBasic;
using static System.String;

namespace ShamirСipher
{
    internal class Program
    {
        private static readonly Random RandomGenerator = new Random();
        private static readonly char[] Alphabet = {  '#', 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И',
                                        'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С',
                                        'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ',
                                        'Э', 'Ю', 'Я', ' ', '1', '2', '3', '4', '5', '6', '7',
                                        '8', '9','0'
        };

        public static int GetGcd(int a, int b)
        {
            while (b != 0)
            {
                var r = a % b;
                a = b;
                b = r;
            }

            return a;
        }

        public static (int X, int Y) GetGeneralizedEuclidAlgorithm(int a, int b)
        {
            var u = (a, 1, 0);
            var v = (b, 0, 1);

            while (v.b != 0)
            {
                var q = u.a / v.b;
                var T = (u.a % v.b, u.Item2 - q * v.Item2, u.Item3 - q * v.Item3);
                u = v;
                v = T;
            }

            return (u.Item2, u.Item3);
        }

        public static int GetInversionGeneralizedEuclidAlgorithm(int m, int c)
        {
            var u = (m, 0);
            var v = (c, 1);

            while (v.c != 0)
            {
                var q = u.m / v.c;
                var T = (u.m % v.c, u.Item2 - q * v.Item2);
                u = v;
                v = T;
            }

            return u.Item2 < 0 ? u.Item2 + m : u.Item2;
        }

        public static List<int> GetNumericString(string str)
        {
            return str.Select(t => Array.IndexOf(Alphabet, t)).ToList();
        }

        public static string GetStringNumeric(List<BigInteger> strList)
        {
            return Join("", strList.Select(t => Alphabet[(int)t]));
        }

        public static bool IsCoprime(int num1, int num2)
        {
            if (num1 == num2)
                return num1 == 1;

            return num1 > num2 ? IsCoprime(num1 - num2, num2) : IsCoprime(num2 - num1, num1);
        }

        public static bool IsPrimeNumber(int n)
        {
            if (n < 2)
                return false;

            for (var i = 2; i < n; i++)
            {
                if (n % i == 0)
                    return false;
            }

            return true;
        }

        public static int GenerateMutuallySimpleNumber(int p)
        {
            var number = RandomGenerator.Next(3, 100);

            while (!IsCoprime(number, p))
            {
                number = RandomGenerator.Next(3, 100);
            }

            return number;
        }

        public static int GetPrimeNumber()
        {
            var number = 0;
            do
            {
                number = RandomGenerator.Next(Alphabet.Length, 10000);

            } while (!IsPrimeNumber(number));

            return number;
        }

        public static void GetShamirsCipher(string msg)
        {

            Console.WriteLine($"Message: {msg}");

            List<int> msgInts = GetNumericString(msg);

            Console.WriteLine($" Message in numeric format -  {Join(" ", msgInts)}");


            List<BigInteger> decryptedList = new List<BigInteger>();


            var _p = GetPrimeNumber();

            foreach (var msgInt in msgInts)
            {
                var cA = GenerateMutuallySimpleNumber(_p - 1);
                var dA = GetInversionGeneralizedEuclidAlgorithm(_p - 1, cA);

                var cB = GenerateMutuallySimpleNumber(_p - 1);
                var dB = GetInversionGeneralizedEuclidAlgorithm(_p - 1, cB);

                Console.WriteLine($"p = {_p}/ Ca = {cA}/ Da = {dA}/ Cb = {cB}/ Db = {dB}/ ");

                var x1 = BigInteger.Pow(msgInt, cA) % _p;
                Console.WriteLine($"Subscriber 1 sent: {x1}");

                var x2 = BigInteger.Pow(x1, cB) % _p;
                Console.WriteLine($"received {x1} / Subscriber 2 sent: {x2}");

                var x3 = BigInteger.Pow(x2, dA) % _p;
                Console.WriteLine($"received {x2} / Subscriber 1 sent: {x3}");

                var x4 = BigInteger.Pow(x3, dB) % _p;
                Console.WriteLine($"received {x3} / АSubscriber 2 decrypted : {x4}");

                decryptedList.Add(x4);
            }

            Console.WriteLine($"Message in numeric format after decryption -  {Join(" ", msgInts)}");

            Console.WriteLine($"Description message :{GetStringNumeric(decryptedList)}");

        }

        private static void Main()
        {
            const string msg = "ШУГАРОВ АНАТОЛИЙ ЗП91";
            GetShamirsCipher(msg);
        }
    }
}
