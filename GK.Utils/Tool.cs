using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace GK.Utils
{
    public class Tool
    {
        private string Base64To16(byte[] buffer)
        {
            string md_str = string.Empty;
            for (int i = 0; i < buffer.Length; i++)
            {
                md_str += buffer[i].ToString("x2");
            }
            return md_str;
        }
        public string SHAmd5Encrypt(string normalTxt)
        {
            var bytes = Encoding.Default.GetBytes(normalTxt);//求Byte[]数组  
            var Md5 = new MD5CryptoServiceProvider();
            var encryptbytes = Md5.ComputeHash(bytes);//求哈希值  
            return Base64To16(encryptbytes);//将Byte[]数组转为净荷明文(其实就是字符串)  
        }

        public string SHA1Encrypt(string normalTxt)
        {
            var bytes = Encoding.Default.GetBytes(normalTxt);
            var SHA = new SHA1CryptoServiceProvider();
            var encryptbytes = SHA.ComputeHash(bytes);
            return Base64To16(encryptbytes);
        }
        public string SHA256Encrypt(string normalTxt)
        {
            var bytes = Encoding.Default.GetBytes(normalTxt);
            var SHA256 = new SHA256CryptoServiceProvider();
            var encryptbytes = SHA256.ComputeHash(bytes);
            return Base64To16(encryptbytes);
        }
        public string SHA384Encrypt(string normalTxt)
        {
            var bytes = Encoding.Default.GetBytes(normalTxt);
            var SHA384 = new SHA384CryptoServiceProvider();
            var encryptbytes = SHA384.ComputeHash(bytes);
            return Base64To16(encryptbytes);
        }
        public string SHA512Encrypt(string normalTxt)
        {
            var bytes = Encoding.Default.GetBytes(normalTxt);
            var SHA512 = new SHA512CryptoServiceProvider();
            var encryptbytes = SHA512.ComputeHash(bytes);
            return Base64To16(encryptbytes);
        }

        public string Md5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] palindata = Encoding.Default.GetBytes(str);//将要加密的字符串转换为字节数组
            byte[] encryptdata = md5.ComputeHash(palindata);//将字符串加密后也转换为字符数组
            return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为加密字符串
        }

        public string Encryption(string express, string key)
        {
            CspParameters param = new CspParameters();
            param.KeyContainerName = key;//密匙容器的名称，保持加密解密一致才能解密成功
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] plaindata = Encoding.Default.GetBytes(express);//将要加密的字符串转换为字节数组
                byte[] encryptdata = rsa.Encrypt(plaindata, false);//将加密后的字节数据转换为新的加密字节数组
                return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为字符串
            }
        }

        //解密
        public string Decrypt(string ciphertext, string key)
        {
            CspParameters param = new CspParameters();
            param.KeyContainerName = key;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] encryptdata = Convert.FromBase64String(ciphertext);
                byte[] decryptdata = rsa.Decrypt(encryptdata, false);
                return Encoding.Default.GetString(decryptdata);
            }
        }

        public int RandNum()
        {
            byte[] randomBytes = new byte[8];
            RNGCryptoServiceProvider rngCrypto = new RNGCryptoServiceProvider();
            rngCrypto.GetBytes(randomBytes);
            int rngNum = BitConverter.ToInt32(randomBytes, 0);
            return Math.Abs(rngNum);
        }

        public List<string> IconList()
        {
            try
            {
                List<string> icons = new List<string>();
                string path = AppDomain.CurrentDomain.BaseDirectory;
                string filepath = path + "icon\\icons.txt";
                StreamReader sr = new StreamReader(filepath, Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    icons.Add(line.ToString());
                }
                sr.Close();
                sr.Dispose();
                return icons;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<dynamic>  GetDLLInfo(string dllpath)
        {
            List<dynamic> list = new List<dynamic>();
            Assembly ass = Assembly.LoadFrom(dllpath);
            string[] exist = new string[] { "ValuesController", "HomeController", "AccountController", "HelpController" };
            IEnumerable<Type> types = ass.GetTypes().Where(t => t.Name.Contains("Controller") && (!exist.Contains(t.Name)));
            foreach (Type type in types)
            {
                string baseurl = string.Empty;
                IEnumerable<Attribute> attrs = type.GetCustomAttributes();
                IEnumerable<MethodInfo> minfos = type.GetMethods().Where(t=>t.Module.Name.ToLower()== "GoldKeyWebApi.dll".ToLower());
                foreach (Attribute attr in attrs.Where(t=>t.GetType().Name=="RoutePrefixAttribute"))
                {
                    baseurl = ((dynamic)attr).Prefix;
                }
                foreach (MethodInfo mi in minfos)
                {
                    string methord_attr = string.Empty;
                    string way = string.Empty;
                    List<dynamic> parlist = new List<dynamic>();
                    IEnumerable<Attribute> mattrs = mi.GetCustomAttributes();
                    ParameterInfo[] pars = mi.GetParameters();
                    foreach (var item in pars)
                    {
                        parlist.Add(new { type = item.ParameterType.Name,name=item.Name,fullname=item.ParameterType.FullName });
                    }
                    string[] marray = new string[] { "RouteAttribute", "HttpGetAttribute", "HttpPostAttribute" };
                    foreach (Attribute mattr in mattrs.Where(t=>marray.Contains(t.GetType().Name)))
                    {
                        switch (mattr.GetType().Name)
                        {
                            case "RouteAttribute":
                                methord_attr = ((dynamic)mattr).Template;
                                break;
                            case "HttpGetAttribute":
                                way = "Get";
                                break;
                            case "HttpPostAttribute":
                                way = "Post";
                                break;
                            default:
                                break;
                        }
                    }
                    list.Add(new { url = baseurl + "/" + methord_attr, way = way,param=parlist });
                }
            }
            return list;
        }
    }
}
