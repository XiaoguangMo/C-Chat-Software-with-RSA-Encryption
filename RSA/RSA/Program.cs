using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Security.Cryptography;

namespace RSA
{
    class Program
    {
        static void Main(string[] args)
        {
            //判断是否含有私钥，如果没有创建
            if (ContainsKey.Create())
            {
                Console.WriteLine("*********请输入输入要加密的数据************");
                string encryptData=   Console.ReadLine();
                Console.WriteLine("加密后的数据：{0}", SRSA.Encrypt(encryptData));
                Console.WriteLine("解密后的数据：{0}", SRSA.Decrypt(SRSA.Encrypt(encryptData)));
                Console.ReadLine();
            }
        }
    }
}
