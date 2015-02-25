using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace AsyncTcpClient
{
    public static class SRSA
    {
        #region RSADeCrtypto
        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="DataToDeCrypto">要解密的数据</param>
        /// <param name="RSAKeyInfo"></param>
        /// <param name="DoOAEPPadding"></param>
        /// <returns></returns>
        static public byte[] RSADeCrtypto(byte[] DataToDeCrypto, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                // System.Security.Cryptography.RSA 的参数。
                RSA.ImportParameters(RSAKeyInfo);
                //
                // 参数:
                //   
                //     要解密的数据。
                //
                // 
                //     如果为 true，则使用 OAEP 填充（仅在运行 Microsoft Windows XP 或更高版本的计算机上可用）执行直接的 System.Security.Cryptography.RSA
                //     解密；否则，如果为 false，则使用 PKCS#1 1.5 版填充。
                return RSA.Decrypt(DataToDeCrypto, DoOAEPPadding);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        #endregion

        #region RSAEnCrypto
        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="DataToEnCrypto"></param>
        /// <param name="RSAKeyInfo"></param>
        /// <param name="DoOAEPPadding"></param>
        /// <returns></returns>
        static public byte[] RSAEnCrypto(byte[] DataToEnCrypto, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.ImportParameters(RSAKeyInfo);
                return RSA.Encrypt(DataToEnCrypto, DoOAEPPadding);

            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        #endregion

        #region Decrypt
        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="base64code">传入加密数据</param>
        /// <returns>返回解密数据</returns>
        static public string Decrypt(string base64code)
        {
            try
            {
                UnicodeEncoding ByteConverter = new UnicodeEncoding();

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.FromXmlString(ReadKey.privateKey);

                byte[] encryptedData;
                byte[] decryptedData;

                encryptedData = Convert.FromBase64String(base64code);

                decryptedData = RSADeCrtypto(encryptedData, RSA.ExportParameters(true), false);
                return ByteConverter.GetString(decryptedData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }


        }
        #endregion

        #region Encrypt
        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="toEncryptString">要解密的数据</param>
        /// <returns></returns>
        static public string Encrypt(string toEncryptString)
        {
            try
            {
                UnicodeEncoding ByteConverter = new UnicodeEncoding();


                byte[] encrypteData;
                byte[] decrypteData;
                decrypteData = ByteConverter.GetBytes(toEncryptString);

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.FromXmlString(ReadKey.publicKey);
                encrypteData = RSAEnCrypto(decrypteData, RSA.ExportParameters(false), false);

                return Convert.ToBase64String(encrypteData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        #endregion

    }
}
