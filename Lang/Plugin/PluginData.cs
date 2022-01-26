using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lang.Plugin
{
    public class PluginData
    {
        public static string ConfigDirectory { get { return @$"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Data\Plugin\"; } }
        public static string CreatePluginData(string PathName)
        {
            try
            {
                if (!Directory.Exists($"{ConfigDirectory}{PathName}\\"))
                    Directory.CreateDirectory($"{ConfigDirectory}{PathName}");
                return $"{ConfigDirectory}{PathName}\\";
            }catch { return ""; }
        }
    }
}
