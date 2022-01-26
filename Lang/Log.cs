using Lang.Plugin;

namespace Lang.Log
{
    public class Log
    {
        public static object _lock = new object();
        public static string LogFileName { set; get; }
        /// <summary>
        /// 在控制台输出一只小猫猫
        /// </summary>
        public static void LogCat()
        {
            Console.WriteLine("");
            string cat = "........................................\n.......,,,,.............................\n.....,,,,,,.................,,...,,.....\n...,,,.....................,,,,,,,,,....\n...,,,....................,,,,,,,,,,,...\n...,,,,By:XiaoCao&MiuxuE!.,,,,,,,,,,,...\n...,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,...\n...,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,....\n...,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,....\n.....,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,.....\n......,,,,,,,,,,,,,,,,,,,,,,,,,,........\n......,,,,,,,,,,,,,,,,,,,,,,,,,,........\n......,,,,,,,,,,,,,,,,,,,,,,,,,,........\n.......,,,,,,,............,,,,,.........\n.......,,,.,,..............,,,..........\n........................................";
            foreach (var item in cat)
            {
                string charData = item.ToString();
                Random random = new Random();
                if (charData == ".")
                {
                    Console.Write(" ");
                }
                else if (charData == ",")
                {
                    Console.Write(".");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(charData);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("咳咳! 欢迎使用天狼星！ 这是一个测试版！ 所以有一只小猫咪~\n");
        }
        /// <summary>
        /// 向控制台输出一条普通消息
        /// </summary>
        /// <param name="_msg"></param>
        public static void LogOut(object _msg)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.White; // 将颜色设置成白色
                Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}-输出] {_msg}");
                Console.ForegroundColor = ConsoleColor.White; // 将颜色设置成白色
            }
        }

        public static void LogOut_Plugin(object _msg, string key)
        {
            lock (_lock)
            {
                var pluginName = PluginManager.PluginKeyGetName(key);
                if (pluginName != "")
                {
                    Console.ForegroundColor = ConsoleColor.White; // 将颜色设置成白色
                    Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}-日志] [+] [{pluginName}] {_msg}");
                    Console.ForegroundColor = ConsoleColor.White; // 将颜色设置成白色
                }
            }
        }
        public static void LogOutErr_Plugin(object _msg, string key)
        {
            lock (_lock)
            {
                var pluginName = PluginManager.PluginKeyGetName(key);
                if (pluginName != "")
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed; // 将颜色设置成红色
                    Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}-错误] [=] [{pluginName}] {_msg}");
                    Console.ForegroundColor = ConsoleColor.White; // 将颜色设置成白色
                }
            }
        }
        public static void LogOutWar_Plugin(object _msg, string key)
        {
            lock (_lock)
            {
                var pluginName = PluginManager.PluginKeyGetName(key);
                if (pluginName != "")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow; // 将颜色设置成淡黄色
                    Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}-警告] [-] [{pluginName}] {_msg}");
                    Console.ForegroundColor = ConsoleColor.White; // 将颜色设置成白色
                }
            }
        }
        public static void LogOutPlugin_Plugin(object _msg, string key)
        {
            lock (_lock)
            {
                var pluginName = PluginManager.PluginKeyGetName(key);
                if (pluginName != "")
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow; // 将颜色设置成淡黄色
                    Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}-调用] [{pluginName}] {_msg}");
                    Console.ForegroundColor = ConsoleColor.White; // 将颜色设置成白色
                }
            }
        }
        public static void LogOutHttpPlugin_Plugin(object _msg, string pluginName)
        {
            lock (_lock)
            {
                if (pluginName != "")
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow; // 将颜色设置成淡黄色
                    Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}-调用] [HttpApi] [{pluginName}] {_msg}");
                    Console.ForegroundColor = ConsoleColor.White; // 将颜色设置成白色
                }
            }
        }
        public static void LogFileInit()
        {

        }

        /// <summary>
        /// 向控制台输出错误消息
        /// </summary>
        /// <param name="_msg"></param>
        public static void LogErr(object _msg)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.Red; // 将颜色设置成红色
                Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}-错误] {_msg}");
                Console.ForegroundColor = ConsoleColor.White; // 将颜色设置成白色
            }
        }
        /// <summary>
        /// 向控制台输出消息
        /// </summary>
        /// <param name="_msg"></param>
        public static void LogMsg(object _msg)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan; // 将颜色设置成青色
                Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}-消息] {_msg}");
                Console.ForegroundColor = ConsoleColor.White; // 将颜色设置成白色
            }
        }

        /// <summary>
        /// 向控制台输出消息
        /// </summary>
        /// <param name="_msg"></param>
        public static void LogOk(object _msg)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen; // 将颜色设置成青色
                Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}-完毕] {_msg}");
                Console.ForegroundColor = ConsoleColor.White; // 将颜色设置成白色
            }
        }

        /// <summary>
        /// 向控制台输出警告
        /// </summary>
        /// <param name="_msg"></param>
        public static void LogWar(object _msg)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow; // 将颜色设置成淡黄色
                Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}-警告] {_msg}");
                Console.ForegroundColor = ConsoleColor.White; // 将颜色设置成白色
            }
        }
        /// <summary>
        /// 向控制台输出DEBUG消息 该命令只会在Debug模式下执行
        /// </summary>
        /// <param name="_msg"></param>
        public static void LogDebug(object _msg)
        {
            lock (_lock)
            {
#if DEBUG // LogDebug只会在编译为Debug模式下执行
                Console.ForegroundColor = ConsoleColor.Gray; // 把颜设置成灰色 好区分
                Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}-调试] {_msg}");
                Console.ForegroundColor = ConsoleColor.White; // 将颜色设置成白色
#endif
            }
        }
        public static void LogStaus(object msg, string channel_Id, int statusCode)
        {
            lock (_lock)
            {
                switch (statusCode)
                {
                    case 200:
                        LogMsg($"在频道 {channel_Id} {msg} 成功");
                        break;
                    case 401:
                        LogErr($"在频道 {channel_Id} {msg} 失败,初始化失败，请尝试重启框架！");
                        break;
                    case 404:
                        LogErr($"在频道 {channel_Id} {msg} 失败,频道ID无效！");
                        break;
                    case 405:
                        LogErr($"在频道 {channel_Id} {msg} 失败,http method 不允许！");
                        break;
                    case 429:
                        LogErr($"在频道 {channel_Id} {msg} 失败,调用过于频繁！");
                        break;
                    case 500:
                        LogErr($"在频道 {channel_Id} {msg} 失败,处理失败！");
                        break;
                    case 504:
                        LogErr($"在频道 {channel_Id} {msg} 处理失败！");
                        break;
                    default:
                        break;
                }
            }
        }
        public static void LogStaus(object msg, string channel_Id)
        {
            lock (_lock)
            {
                LogMsg($"在频道 {channel_Id} {msg}");
            }
        }
    }
}