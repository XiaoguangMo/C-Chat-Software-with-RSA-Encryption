using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;


namespace RSA
{
   
    public  static  class ReadKey
    {
        public static string publicKey = ReadPublicKey();
        public static string privateKey=ReadPrivateKey();
        #region ReadPublicKey
        /// <summary>
        /// 读取公钥的值
        /// </summary>
        /// <returns>公钥的值</returns>
        public static string  ReadPublicKey()
        {
            try
            {
                using (StreamReader reader = new StreamReader("PublicKey.xml"))
                {
                  return  reader.ReadToEnd();
                }

            }
            catch
            {
                return null;
            }
        }
        #endregion 

        #region ReadPrivateKey
        /// <summary>
        /// 读取私钥
        /// </summary>
        /// <returns>私钥字符串</returns>
        public static string ReadPrivateKey()
        {
            try
            {
                using (StreamReader dr = new StreamReader("PrivateKey.xml"))
                {
                    return dr.ReadToEnd();
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
