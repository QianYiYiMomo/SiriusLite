using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;

namespace Lang
{
    public class Update
    {
        public static Uri url = new Uri("http://api.siriusbot.cn/update/");
        public static HttpClient httpClient = new HttpClient();
        public static void UpdateCheck()
        {
            try
            {
                var task = httpClient.GetStringAsync($"{url}?type=check&versions={FrameworkInfo.VersionName}");
                task.Wait();
                UpdateData updateData = JsonSerializer.Deserialize<UpdateData>(task.Result)!;

                if(updateData.code == 404 || updateData.code == 405)
                {
                    Environment.Exit(Environment.ExitCode); //使用了破解版
                }

                var task2 = httpClient.GetStringAsync($"{url}?type=getnew");
                task2.Wait();
                UpdateData update = JsonSerializer.Deserialize<UpdateData>(task2.Result)!;
                
                if(Convert.ToInt32(update.data.id) > FrameworkInfo.VersionID)
                {
                    Log.Log.LogWar($"\n检测到新版:{update.data.versions}\n目前版本:{FrameworkInfo.VersionName}\n下载地址:{update.data.download}" +
                        $"\n更新日志:\n{update.data.description} \n输入:update_ok 更新");
                }
                else if(Convert.ToInt32(update.data.id) == FrameworkInfo.VersionID)
                {
                    Log.Log.LogOk("检查更新成功,您目前是最新版本!");
                }
            }
            catch (Exception ex)
            {
                Log.Log.LogErr("获取更新失败:"+ex);
            }
        }
        public static UpdateInfo GetUpdate()
        {
            try
            {
                var task = httpClient.GetStringAsync($"{url}?type=check&versions={FrameworkInfo.Version}");
                task.Wait();
                UpdateData updateData = JsonSerializer.Deserialize<UpdateData>(task.Result)!;

                if (updateData.code == 404)
                {
                    Environment.Exit(Environment.ExitCode); //使用了破解版
                }

                var task2 = httpClient.GetStringAsync($"{url}?type=getnew");
                task2.Wait();
                UpdateData update = JsonSerializer.Deserialize<UpdateData>(task2.Result)!;

                return update.data;
            }
            catch (Exception ex)
            {
                Log.Log.LogErr("获取更新失败:" + ex);
                return new();
            }
        }
        public static void StartUpdate()
        {
            try
            {
                UpdateInfo update = GetUpdate();
                var res = HttpDownload(update.download, $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Sirius{update.versions}.zip");
                if (res)
                {
                    Log.Log.LogOk($"更新文件下载成功 文件在 : {AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Sirius{update.versions}.zip 解压覆盖即可~");
                }
                else
                {
                    Log.Log.LogErr("更新文件下载失败!");
                }
            }catch (Exception e)
            {
                Log.Log.LogErr("更新失败:" + e);
            }   
        }
        /// <summary>
        /// http下载文件
        /// </summary>
        /// <param name="url">下载文件地址</param>
        /// <param name="path">文件存放地址，包含文件名</param>
        /// <returns></returns>
        public static bool HttpDownload(string url, string path)
        {
            string tempPath = Path.GetDirectoryName(path) + @"\temp";
            Directory.CreateDirectory(tempPath);  //创建临时文件目录
            string tempFile = tempPath + @"\" + System.IO.Path.GetFileName(path) + ".temp"; //临时文件
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);    //存在则删除
            }
            try
            {
                FileStream fs = new FileStream(tempFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();
                //创建本地文件写入流
                //Stream stream = new FileStream(tempFile, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    //stream.Write(bArr, 0, size);
                    fs.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                //stream.Close();
                fs.Close();
                responseStream.Close();
                System.IO.File.Move(tempFile, path);
                Directory.Delete(tempPath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public class UpdateInfo
    {
        public string id { get; set; }
        public string versions { get; set; }
        public string description { get; set; }
        public string download { get; set; }
        public string update { get; set; }
        public string md5 { get; set; }
    }
    public class UpdateData
    {
        public int code { get; set; }
        public string msg { get; set; }
        public UpdateInfo data { get; set; }
    }

}
