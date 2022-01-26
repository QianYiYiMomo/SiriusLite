using Lang.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lang
{
    public class Config
    {
        public string RunPath { get { return $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}"; } }
        public string ConfigDirectory { get { return @$"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Data\Sirius\"; } }
        public class ConfigData
        {
            public ConfigData(string key, string botid, string token, int port)
            {
                this.Key = key;
                this.BotID = botid;
                this.Token = token;
                this.Port = port;
            }
            [JsonPropertyName("key")]
            public string Key { get; set; }
            [JsonPropertyName("botid")]
            public string BotID { get; set; }
            [JsonPropertyName("token")]
            public string Token { get; set; }
            [JsonPropertyName("port")]
            public int Port { get; set; }
        }
        public void InitConfig()
        {
            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
                File.Create(ConfigDirectory + "sirius.sis").Close();
            }

            if (!File.Exists(ConfigDirectory + "sirius.sis"))
                File.Create(ConfigDirectory + "sirius.sis").Close();

        }
        public void ResetConfig()
        {
            Directory.Delete(ConfigDirectory, true);
            Directory.CreateDirectory(ConfigDirectory);
            File.Create(ConfigDirectory + "sirius.sis").Close();
        }
        public ConfigData? GetConfig()
        {
            try
            {
                var configData = Reandomer.setRandom(File.ReadAllBytes(ConfigDirectory + "sirius.sis"));
                if (configData != "")
                    return JsonSerializer.Deserialize<ConfigData>(configData)!;
                return null;
            }
            catch (Exception e)
            {
                Log.Log.LogErr($"读取配置文件失败:{e}");
                return null;
            }
        }
        public void SaveConfig(string key, string botid, string token, int port)
        {
            try
            {
                var configData = new ConfigData(key, botid, token, port);
                var data = JsonSerializer.Serialize(configData);
                File.WriteAllBytes(ConfigDirectory + "sirius.sis", Reandomer.getRandom(data));
            }
            catch (Exception e)
            {
                Log.Log.LogErr($"写配置文件失败:{e}");
            }
        }

        public class Reandomer
        {
            private const string randomChars = "D]RI|~>G~n0?gIZayLAy*F^b4WzRGTCN";
            private const string Iv = "wH2lJuDdtbt57q3k";
            /// <summary>  
            /// AES加密  
            /// </summary>  
            /// <param name="str">需要加密字符串</param>  
            /// <returns>加密后字符串</returns>  
            public static byte[] getRandom(string str)
            {
                return r(str, randomChars);
            }

            /// <summary>  
            /// AES解密  
            /// </summary>  
            /// <param name="str">需要解密字符串</param>  
            /// <returns>解密后字符串</returns>  
            public static string setRandom(byte[] str)
            {
                return d(str, randomChars);
            }
            /// <summary>
            /// AES加密
            /// </summary>
            /// <param name="str">需要加密的字符串</param>
            /// <param name="key">32位密钥</param>
            /// <returns>加密后的字符串</returns>
            public static byte[] r(string str, string key)
            {
                Byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(key);
                Byte[] toEncryptArray = System.Text.Encoding.UTF8.GetBytes(str);
                var rijndael = new System.Security.Cryptography.RijndaelManaged();
                rijndael.Key = keyArray;
                rijndael.Mode = System.Security.Cryptography.CipherMode.ECB;
                rijndael.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                rijndael.IV = System.Text.Encoding.UTF8.GetBytes(Iv);
                System.Security.Cryptography.ICryptoTransform cTransform = rijndael.CreateEncryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return resultArray;
            }
            /// <summary>
            /// AES解密
            /// </summary>
            /// <param name="str">需要解密的字符串</param>
            /// <param name="key">32位密钥</param>
            /// <returns>解密后的字符串</returns>
            public static string d(byte[] bytes, string key)
            {
                Byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(key);
                Byte[] toEncryptArray = bytes;
                var rijndael = new System.Security.Cryptography.RijndaelManaged();
                rijndael.Key = keyArray;
                rijndael.Mode = System.Security.Cryptography.CipherMode.ECB;
                rijndael.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                rijndael.IV = System.Text.Encoding.UTF8.GetBytes(Iv);
                System.Security.Cryptography.ICryptoTransform cTransform = rijndael.CreateDecryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return System.Text.Encoding.UTF8.GetString(resultArray);
            }
        }

        public void SetPort(ref ConfigData cfg)
        {
            while (true)
            {
                Console.Write("请重新输入端口 -> ");
                string port = Console.ReadLine()!;
                try
                {
                    int intPort = int.Parse(port);

                    if (intPort >= 0 && intPort <= 65535)
                    {
                        this.SaveConfig(cfg.Key, cfg.BotID, cfg.Token, intPort);
                        cfg = this.GetConfig()!;
                        Log.Log.LogMsg($"HttpApi端口:{cfg!.Port}");
                    }
                    else
                    {
                        Log.Log.LogErr("输入不合法! 请输入0-65535的整数。");
                    }
                }
                catch (Exception e)
                {
                    Log.Log.LogErr("输入不合法! 请输入0-65535的整数。");
                }
            }
        }
    }
}
