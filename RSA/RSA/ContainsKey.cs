using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace RSA
{
    public static  class ContainsKey
    {
        #region Contain
        /// <summary>
        /// 是否含有文件名
        /// </summary>
        /// <param name="Name">传入的文件名</param>
        /// <returns></returns>
        public  static  bool Contain(string Name)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string full=path+Name;
            full = full.Replace("\\",System.IO.Path.DirectorySeparatorChar.ToString());
           
                if (!File.Exists(full))
                {
                    return false;
                }
                return true;
        }
        #endregion 

        #region Create
        /// <summary>
        /// 判断是否含有，如果有返回true，如果没有创建返回true
        /// </summary>
        /// <returns></returns>
        public static bool Create()
        {
            try
            {
                if (Contain("PrivateKey.xml"))
                {
                    return true;
                }
                else
                {
                    CreateKey.GetPublicKey();
                    return true;
                }
            }
            catch
            {
                return false;
            }
            
        }
        #endregion
    }
}
