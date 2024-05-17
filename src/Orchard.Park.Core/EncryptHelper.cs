using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Orchard.Park.Core
{
    /// <summary>
    /// 加密帮助类
    /// </summary>
    public static class EncryptHelper
    {
        /// <summary>
        /// MD5withRSA签名
        /// </summary>
        /// <param name="content">待加密字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>加密后字符串</returns>
        public static string Sign(string content, string privateKey)
        {
            var sig = SignerUtilities.GetSigner("MD5withRSA");

            sig.Init(true, CreateKEY(true, privateKey));

            var bytes = Encoding.GetEncoding("UTF-8").GetBytes(content);

            sig.BlockUpdate(bytes, 0, bytes.Length);
            byte[] signature = sig.GenerateSignature();

            /* Base 64 encode the sig so its 8-bit clean */
            var signedString = Convert.ToBase64String(signature);

            return signedString;
        }

        private static AsymmetricKeyParameter CreateKEY(bool isPrivate, string key)
        {
            byte[] keyInfoByte = Convert.FromBase64String(key);

            return isPrivate ? PrivateKeyFactory.CreateKey(keyInfoByte) : PublicKeyFactory.CreateKey(keyInfoByte);
        }

        /// <summary>
        /// AES加密 对应java中的 aes/cbc/pkcs5padding 模式的算法
        /// </summary>
        /// <param name="str">待加密的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AesEncrypt(string str, string key, string iv)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            using var rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.PKCS7;
            rDel.IV = Encoding.UTF8.GetBytes(iv);

            using var cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// AES解密  对应java中的 aes/cbc/pkcs5padding 模式的算法
        /// </summary>
        /// <param name="str">待解密的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv"></param>
        /// <returns>返回空为解析失败</returns>
        public static string AesDecrypt(string str, string key, string iv)
        {
            using var rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            byte[] encryptedData = Convert.FromBase64String(str);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
                len = keyBytes.Length;
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = Encoding.UTF8.GetBytes(iv);

            using var transform = rijndaelCipher.CreateDecryptor();
            byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            return Encoding.UTF8.GetString(plainText);
        }

        public static string HmacMD5(string source, string key)
        {
            using var hmaCmd = new HMACMD5(Encoding.Default.GetBytes(key));
            var byteArray = hmaCmd.ComputeHash(Encoding.Default.GetBytes(source));

            var sb = new StringBuilder();
            foreach (var t in byteArray)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ComputeMD5(string input)
        {
            // 将输入字符串转换为字节数组
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            // 创建MD5对象
            using MD5 md5 = MD5.Create();
            // 计算哈希值
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            // 将哈希值转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 转换escape编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Escape(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;

            StringBuilder sb = new StringBuilder();
            byte[] ba = Encoding.Unicode.GetBytes(s);
            for (int i = 0; i < ba.Length; i += 2)
            {    /**/
                sb.Append("%u");
                sb.Append(ba[i + 1].ToString("X2"));
                sb.Append(ba[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 解码Escape
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UnEscape(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Remove(0, 2);//删除最前面两个＂%u＂
                string[] strArr = str.Split(new string[] { "%u" }, StringSplitOptions.None);//以子字符串＂%u＂分隔
                byte[] byteArr = new byte[strArr.Length * 2];
                for (int i = 0, j = 0; i < strArr.Length; i++, j += 2)
                {
                    byteArr[j + 1] = Convert.ToByte(strArr[i].Substring(0, 2), 16);  //把十六进制形式的字串符串转换为二进制字节
                    byteArr[j] = Convert.ToByte(strArr[i].Substring(2, 2), 16);
                }
                str = Encoding.Unicode.GetString(byteArr); //把字节转为unicode编码
            }
            return str;
        }
    }
}