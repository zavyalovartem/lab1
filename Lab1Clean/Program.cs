using System;
using System.IO;

namespace Lab1Clean
{
    class Program
    {
        static void Main(string[] args)
        {
            var rsa = new RSA();
            Console.WriteLine("0 - сгенерировать ключи, 1 - зашифровать текст, 2 - расшифровать текст");
            var option = Convert.ToInt32(Console.ReadLine());
            switch (option)
            {
                case 0:
                {
                        rsa.GenerateKeyPair();
                        var openKeys = rsa.getOpenKeys();
                        var privateKeys = rsa.getPrivateKeys();
                        var e = openKeys[0];
                        var d = privateKeys[0];
                        var n = openKeys[1];
                        Console.WriteLine("Открытый ключ: \n e = {0}, n = {1}", e, n);
                        Console.WriteLine("Закрытый ключ: \n d = {0}, n = {1}", d, n);
                        break;
                }
                case 1:
                {
                        Console.WriteLine("Введите открытый ключ:");
                        BigInt e, n;
                        try
                        {
                            Console.WriteLine("e:");
                            e = new BigInt(Console.ReadLine());
                            Console.WriteLine("n:");
                            n = new BigInt(Console.ReadLine());
                        }
                        catch
                        {
                            Console.WriteLine("Недопустимый формат ключа");
                            Console.ReadLine();
                            break;
                        }
                        Console.WriteLine("Введите полный путь к файлу для шифра");
                        var output = Console.ReadLine();
                        Console.WriteLine("Введите полный путь к файлу с исходным текстом");
                        var input = Console.ReadLine();
                        StreamReader sr;
                        try
                        {
                            sr = new StreamReader(input ?? String.Empty);
                        }
                        catch
                        {
                            Console.WriteLine("Неверный путь до файла с исходным текстом");
                            Console.ReadLine();
                            break;
                        }

                        var inp = "";
                        while (!sr.EndOfStream)
                        {
                            inp += sr.ReadLine();
                        }
                        sr.Close();

                        var res = rsa.Encrypt(inp, e, n);
                        try
                        {
                            StreamWriter sw = new StreamWriter(output ?? String.Empty);
                            sw.WriteLine(res);
                            sw.Close();
                        }
                        catch
                        {
                            Console.WriteLine("Неверный путь до файла для шифра");
                            Console.ReadLine();
                            break;
                        }
                        break;
                }
                case 2:
                    {
                        Console.WriteLine("Введите закрытый ключ:");
                        BigInt d, n;
                        try
                        {
                            Console.WriteLine("d:");
                            d = new BigInt(Console.ReadLine());
                            Console.WriteLine("n:");
                            n = new BigInt(Console.ReadLine());
                        }
                        catch
                        {
                            Console.WriteLine("Недопустимый формат ключа");
                            Console.ReadLine();
                            break;
                        }
                        Console.WriteLine("Введите полный путь к файлу с шифром");
                        var input = Console.ReadLine();
                        Console.WriteLine("Введите полный путь к файлу для расшифрованного текста");
                        var output = Console.ReadLine();
                        StreamReader sr;
                        try
                        {
                            sr = new StreamReader(input ?? String.Empty);
                        }
                        catch
                        {
                            Console.WriteLine("Неверный путь до файла с исходным текстом");
                            Console.ReadLine();
                            break;
                        }

                        var cipher = "";
                        while (!sr.EndOfStream)
                        {
                            cipher += sr.ReadLine();
                        }
                        sr.Close();

                        var res = rsa.Decrypt(cipher, d, n);
                        try
                        {
                            StreamWriter sw = new StreamWriter(output ?? String.Empty);
                            sw.WriteLine(res);
                            sw.Close();
                        }
                        catch
                        {
                            Console.WriteLine("Неверный путь до файла для расшифрованного текста");
                            Console.ReadLine();
                            break;
                        }
                        break;
                    }
                default:
                    Console.WriteLine("Вами выбрана неверная команда");
                    Console.ReadLine();
                    break;
            }
            Console.WriteLine("Действие выполнено успешно");
            Console.ReadLine();
        }
    }
}
