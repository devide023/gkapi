using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Configuration;

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

        public List<dynamic> GetDLLInfo(string dllpath)
        {
            string domain = ConfigurationManager.AppSettings["domain"] ?? "";
            List<dynamic> list = new List<dynamic>();
            Assembly ass = Assembly.LoadFrom(dllpath);
            int pos = dllpath.LastIndexOf("\\");
            string dllfile = dllpath.Substring(pos + 1, dllpath.Length - (pos + 1));
            string[] exist = new string[] { "ValuesController", "HomeController", "AccountController", "HelpController" };
            IEnumerable<Type> types = ass.GetTypes().Where(t => t.Name.Contains("Controller") && (!exist.Contains(t.Name)));
            foreach (Type type in types)
            {
                string baseurl = string.Empty;
                List<dynamic> mlist = new List<dynamic>();
                IEnumerable<Attribute> attrs = type.GetCustomAttributes();
                IEnumerable<MethodInfo> minfos = type.GetMethods().Where(t => t.Module.Name.ToLower() == dllfile.ToLower());
                foreach (Attribute attr in attrs.Where(t => t.GetType().Name == "RoutePrefixAttribute"))
                {
                    baseurl = domain + ((dynamic)attr).Prefix;
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
                        parlist.Add(new { type = item.ParameterType.Name, name = item.Name, fullname = item.ParameterType.FullName });
                    }
                    string[] marray = new string[] { "RouteAttribute", "HttpGetAttribute", "HttpPostAttribute" };
                    foreach (Attribute mattr in mattrs.Where(t => marray.Contains(t.GetType().Name)))
                    {
                        switch (mattr.GetType().Name)
                        {
                            case "RouteAttribute":
                                methord_attr = ((dynamic)mattr).Template;
                                int pos1 = methord_attr.LastIndexOf("{");
                                int pos2 = methord_attr.LastIndexOf("}");
                                if (pos1 > 0 && pos2 > pos1)
                                {
                                    string removestr = methord_attr.Substring(pos1, pos2+1 - pos1);
                                    methord_attr = methord_attr.Replace(removestr, "");
                                    int pos3 = methord_attr.LastIndexOf("/");
                                    if (pos3 == methord_attr.Length - 1)
                                    {
                                        methord_attr = methord_attr.Remove(pos3, 1);
                                    }
                                }
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
                    mlist.Add(new { name = methord_attr, way = way, param = parlist, fullurl = baseurl + "/" + methord_attr });
                }
                list.Add(new { baseurl = baseurl, funlist = mlist });
            }
            return list;
        }
        /// <summary>
        /// 对象转换为字典
        /// </summary>
        /// <param name="obj">待转化的对象</param>
        /// <param name="isIgnoreNull">是否忽略NULL 这里我不需要转化NULL的值，正常使用可以不穿参数 默认全转换</param>
        /// <returns></returns>
        public static Dictionary<string, object> ObjectToMap(object obj, bool isIgnoreNull = false)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();

            Type t = obj.GetType(); // 获取对象对应的类， 对应的类型

            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance); // 获取当前type公共属性

            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetGetMethod();

                if (m != null && m.IsPublic)
                {
                    // 进行判NULL处理 
                    if (m.Invoke(obj, new object[] { }) != null || !isIgnoreNull)
                    {
                        map.Add(p.Name, m.Invoke(obj, new object[] { })); // 向字典添加元素
                    }
                }
            }
            return map;
        }

        public List<string> VueComponents(string dir)
        {
            List<string> vuelist = new List<string>();
            DirectoryInfo di = new DirectoryInfo(dir);
            FileSystemInfo[] fsis = di.GetFileSystemInfos("*.vue", SearchOption.AllDirectories);
            foreach (FileSystemInfo file in fsis)
            {
                string viewpath = file.FullName.Replace(dir, "").Replace(".vue","").Replace("\\","/");
                vuelist.Add(viewpath);
            }
            return vuelist;
        }        
    }
}
