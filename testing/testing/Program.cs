using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Xml;

namespace testing
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlTextReader reader = new XmlTextReader("public.xml");

            while (reader.Read())
            {
                Console.WriteLine(reader.Name);
            } 
            Console.WriteLine("Please input a string\n");

            string mingwen = Console.ReadLine();
            string cipher = RSAEncrypt("abc", mingwen);
            Console.WriteLine("mi wen is: \n" + cipher);
            string plain = RSADecrypt("abc", cipher);
            Console.WriteLine("ming wen is: \n" + plain);
            /*
            String publickey = @"<RSAKeyValue><Modulus>5m9m14XH3oqLJ8bNGw9e4rGpXpcktv9MSkHSVFVMjHbfv+SJ5v0ubqQxa5YjLN4vc49z7SVju8s0X4gZ6AzZTn06jzWOgyPRV54Q4I0DCYadWW4Ze3e+BOtwgVU1Og3qHKn8vygoj40J6U85Z/PTJu3hN1m75Zr195ju7g9v4Hk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(publickey);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(mingwen), false);
            string str = System.Text.Encoding.Default.GetString(cipherbytes);
            Console.WriteLine("mi wen is: \n" + str);
            for (int i = 0; i < cipherbytes.Length; i++)
            {
                Console.WriteLine(cipherbytes[i]);
            }
            String abc = Convert.ToBase64String(cipherbytes);
            Console.WriteLine(abc);
            string mingwen2 = Console.ReadLine();
            
            
            String privatekey = @"<RSAKeyValue><Modulus>5m9m14XH3oqLJ8bNGw9e4rGpXpcktv9MSkHSVFVMjHbfv+SJ5v0ubqQxa5YjLN4vc49z7SVju8s0X4gZ6AzZTn06jzWOgyPRV54Q4I0DCYadWW4Ze3e+BOtwgVU1Og3qHKn8vygoj40J6U85Z/PTJu3hN1m75Zr195ju7g9v4Hk=</Modulus><Exponent>AQAB</Exponent><P>/hf2dnK7rNfl3lbqghWcpFdu778hUpIEBixCDL5WiBtpkZdpSw90aERmHJYaW2RGvGRi6zSftLh00KHsPcNUMw==</P><Q>6Cn/jOLrPapDTEp1Fkq+uz++1Do0eeX7HYqi9rY29CqShzCeI7LEYOoSwYuAJ3xA/DuCdQENPSoJ9KFbO4Wsow==</Q><DP>ga1rHIJro8e/yhxjrKYo/nqc5ICQGhrpMNlPkD9n3CjZVPOISkWF7FzUHEzDANeJfkZhcZa21z24aG3rKo5Qnw==</DP><DQ>MNGsCB8rYlMsRZ2ek2pyQwO7h/sZT8y5ilO9wu08Dwnot/7UMiOEQfDWstY3w5XQQHnvC9WFyCfP4h4QBissyw==</DQ><InverseQ>EG02S7SADhH1EVT9DD0Z62Y0uY7gIYvxX/uq+IzKSCwB8M2G7Qv9xgZQaQlLpCaeKbux3Y59hHM+KpamGL19Kg==</InverseQ><D>vmaYHEbPAgOJvaEXQl+t8DQKFT1fudEysTy31LTyXjGu6XiltXXHUuZaa2IPyHgBz0Nd7znwsW/S44iql0Fen1kzKioEL3svANui63O3o5xdDeExVM6zOf1wUUh/oldovPweChyoAdMtUzgvCbJk1sYDJf++Nr0FeNW1RB1XG30=</D></RSAKeyValue>";
            RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider();
            byte[] cipherbytes2;
            rsa2.FromXmlString(privatekey);
            cipherbytes2 = rsa2.Decrypt(Convert.FromBase64String(abc), false);
            str = System.Text.Encoding.Default.GetString(cipherbytes2);
            String abc2 = Convert.ToBase64String(cipherbytes2);
            Console.WriteLine("mi wen is: " + abc2);
            */
            string mingwen3 = Console.ReadLine();
            
        }
        public static string RSAEncrypt(string publickey, string content)
        {
            publickey = @"<RSAKeyValue><Modulus>jyGTmIXGXd8D18K3mnkWluPCKLGc2r1o6bqz/YQATq6CL0Z4rsTNyUZuYfAuChBgaJEg5TM4uwSlxD7gMGyeq4HPee20pmZSr4XU5KmwO132OtjihuYRKNZ42fdUzZ7Gq8OZ3AL4jzeCXBgxjG6qiTabzhO32/KBr1OkbMcPQyE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(publickey);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSADecrypt(string privatekey, string content)
        {
            privatekey = @"<RSAKeyValue><Modulus>jyGTmIXGXd8D18K3mnkWluPCKLGc2r1o6bqz/YQATq6CL0Z4rsTNyUZuYfAuChBgaJEg5TM4uwSlxD7gMGyeq4HPee20pmZSr4XU5KmwO132OtjihuYRKNZ42fdUzZ7Gq8OZ3AL4jzeCXBgxjG6qiTabzhO32/KBr1OkbMcPQyE=</Modulus><Exponent>AQAB</Exponent><P>xL6qv8Sr2eO/bh8dMYl+DeUjBJM5HcP0AzELwMboMhBZoIodXLNicPhHGpXG0ga/hBcdj8GGFXM3ZAiwOQtAyQ==</P><Q>uj02yMDZB1Hn46WcsT+yzTFfn36keSH49f8Ykw15Kx3iO1/7pAxeHUGiIUPzArx3QBGI9wea9XmdRfZWbkGzmQ==</Q><DP>MkKbEB5H4IPD1XfxV0ROhXIxvet2PdpfLamL3RVzH8b5fn+J/LJ+xWlHkM0OW30J086r6JuBi11BmzZ0/j2XsQ==</DP><DQ>r4uvrf8JEvtDRrL+RZyzHvLhHWe0BHHfR5DSSxoyElAyxKCug20c8RrChfSUhQm8pelhXugOSj/93MpbocXQUQ==</DQ><InverseQ>Rg2Tu2pX8XSR3YSrIiViMG8CVfMpvw6nVomYX18dRNrZtmqciemlKrIjnx/wV96R4hc2fUKE3h8sX9rghW2HPA==</InverseQ><D>KbCc/hMRCWyZAPJ9aigU9VRXzGSXjHsZIOM55ADb2g8oaj67jiRS0GqMYkiEEHherHaBP2FhB9A9vGCYCmLJADocicoHJVtRbn9DqkIPL9eY9F2l3Xrq2+ADeqyqDl442XeBkWqHQ2K5cINPrU8YusINWprJOnWTvPEm9Q0RGsE=</D></RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(privatekey);
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);

            return Encoding.UTF8.GetString(cipherbytes);
        }
    }
}
