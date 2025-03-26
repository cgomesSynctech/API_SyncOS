using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class MD5
    {
        //private const string _PALAVRACHAVE = "SYNCTECHsolucoes1011";
        public static string Get(string source)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider sKey = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(source);
            bs = sKey.ComputeHash(bs);
            System.Text.StringBuilder sString = new System.Text.StringBuilder();
            foreach(byte b in bs)
            {
                sString.Append(b.ToString("x2").ToLower());
            }

            return sString.ToString();
        }

        public static string GetSenhaMD5(string senha)
        {
            //return Get(string.Format("{0}{1}",senha,_PALAVRACHAVE));
            return Get(senha);
        }


    }
}
