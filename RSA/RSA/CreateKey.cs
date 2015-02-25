using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace RSA
{
    /// <summary>
    /// 创建公钥和私钥
    /// </summary>
    public static class CreateKey
    {
        #region GetPublicKey
        /// <summary>
        /// 产生公钥和私钥
        /// </summary>
        public static void GetPublicKey()
        {
            //RSA必须是一个对象，产生公钥和私钥
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                using (StreamWriter writer = new StreamWriter("PrivateKey.xml"))
                {
                    // ToXmlString中 true 表示同时包含 RSA 公钥和私钥；false 表示仅包含公钥。
                    writer.WriteLine(RSA.ToXmlString(true));
                }
                using (StreamWriter writer = new StreamWriter("PublicKey.xml"))
                {
                    writer.WriteLine(RSA.ToXmlString(false));
                }

            }

        }
        #endregion
       

    }
}
