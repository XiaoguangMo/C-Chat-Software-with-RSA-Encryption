using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Xml;

namespace MyRSA
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("\n encrypt and de_encrypt test:\n===========================");

            RSAUtil rsa = new RSAUtil();

            //rsa.CreateRSAKey()  

            Console.WriteLine("Please input a string\n");

            string mingwen = Console.ReadLine();

            string miwen = rsa.EnCrypt(mingwen);

            string jiemiwen = rsa.DoEncrypt(miwen);

            Console.WriteLine("Ming Wen is :" + mingwen + "\n");

            Console.WriteLine("Mi Wen is :" + miwen + "\n");

            Console.WriteLine("Jie Mi hou is :" + jiemiwen + "\n");

            Console.ReadLine();
        }
    }

    class RSAUtil
    {
        public void CreateRSAKey()
        {

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            RSAParameters keys = rsa.ExportParameters(true);
            String pkxml = "<RSAKeyValue>\n<Modulus>" + ToHexString(keys.Modulus) + "</Modulus>";
            pkxml += "\n<Exponent>" + ToHexString(keys.Exponent) + "</Exponent>\n</RSAKeyValue>";
            String psxml = "<RSAKeyValue>\n<Modulus>" + ToHexString(keys.Modulus) + "</Modulus>";
            psxml += "\n<Exponent>" + ToHexString(keys.Exponent) + "</Exponent>";
            psxml += "\n<D>" + ToHexString(keys.D) + "</D>";
            psxml += "\n<DP>" + ToHexString(keys.DP) + "</DP>";
            psxml += "\n<P>" + ToHexString(keys.P) + "</P>";
            psxml += "\n<Q>" + ToHexString(keys.Q) + "</Q>";
            psxml += "\n<DQ>" + ToHexString(keys.DQ) + "</DQ>";
            psxml += "\n<InverseQ>" + ToHexString(keys.InverseQ) + "</InverseQ>\n</RSAKeyValue>";

            SaveToFile("publickey.xml", pkxml);
            SaveToFile("privatekey.xml", psxml);

        }
        public RSACryptoServiceProvider CreateRSADEEncryptProvider(String privateKeyFile)
        {
            RSAParameters parameters1;
            parameters1 = new RSAParameters();
            StreamReader reader1 = new StreamReader(privateKeyFile);
            XmlDocument document1 = new XmlDocument();
            document1.LoadXml(reader1.ReadToEnd());
            XmlElement element1 = (XmlElement)document1.SelectSingleNode("RSAKeyValue");
            parameters1.Modulus = ReadChild(element1, "Modulus");
            parameters1.Exponent = ReadChild(element1, "Exponent");
            parameters1.D = ReadChild(element1, "D");
            parameters1.DP = ReadChild(element1, "DP");
            parameters1.DQ = ReadChild(element1, "DQ");
            parameters1.P = ReadChild(element1, "P");
            parameters1.Q = ReadChild(element1, "Q");
            parameters1.InverseQ = ReadChild(element1, "InverseQ");
            CspParameters parameters2 = new CspParameters();
            parameters2.Flags = CspProviderFlags.UseMachineKeyStore;
            RSACryptoServiceProvider provider1 = new RSACryptoServiceProvider(parameters2);
            provider1.ImportParameters(parameters1);
            return provider1;
        }
        public RSACryptoServiceProvider CreateRSAEncryptProvider(String publicKeyFile)
        {
            RSAParameters parameters1;
            parameters1 = new RSAParameters();
            StreamReader reader1 = new StreamReader(publicKeyFile);
            XmlDocument document1 = new XmlDocument();
            document1.LoadXml(reader1.ReadToEnd());
            XmlElement element1 = (XmlElement)document1.SelectSingleNode("RSAKeyValue");
            parameters1.Modulus = ReadChild(element1, "Modulus");
            parameters1.Exponent = ReadChild(element1, "Exponent");
            CspParameters parameters2 = new CspParameters();
            parameters2.Flags = CspProviderFlags.UseMachineKeyStore;
            RSACryptoServiceProvider provider1 = new RSACryptoServiceProvider(parameters2);
            provider1.ImportParameters(parameters1);
            return provider1;
        }

        private byte[] ReadChild(XmlElement parent, string name)
        {
            XmlElement element1 = (XmlElement)parent.SelectSingleNode(name);
            return hexToBytes(element1.InnerText);
        }

        private string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "  
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
        public byte[] hexToBytes(String src)
        {
            int l = src.Length / 2;
            String str;
            byte[] ret = new byte[l];

            for (int i = 0; i < l; i++)
            {
                str = src.Substring(i * 2, 2);
                ret[i] = Convert.ToByte(str, 16);
            }
            return ret;
        }

        public void SaveToFile(String filename, String data)
        {
            System.IO.StreamWriter sw = System.IO.File.CreateText(filename);
            sw.WriteLine(data);
            sw.Close();
        }

        public string EnCrypt(string str)
        {
            RSACryptoServiceProvider rsaencrype = CreateRSAEncryptProvider("publickey.xml");

            String text = str;

            byte[] data = new UnicodeEncoding().GetBytes(text);

            byte[] endata = rsaencrype.Encrypt(data, true);

            return ToHexString(endata);
        }

        public string DoEncrypt(string hexstr)
        {
            RSACryptoServiceProvider rsadeencrypt = CreateRSADEEncryptProvider("privatekey.xml");

            byte[] miwen = hexToBytes(hexstr);

            byte[] dedata = rsadeencrypt.Decrypt(miwen, true);

            return System.Text.UnicodeEncoding.Unicode.GetString(dedata);
        }
    }
}