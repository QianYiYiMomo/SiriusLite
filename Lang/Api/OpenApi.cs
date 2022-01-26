using Lang.Entity;
using Lang.Entity.Create;
using System.Text.Json;
using System.Net.Http.Json;
using System.Text;

namespace Lang.Api
{
    /// <summary>
    /// OpenAPI调用腾讯提供的接口来对Bot进行操作
    /// 开放接口,请参考腾讯API
    /// https://bot.q.qq.com/wiki/develop/api/
    /// </summary>
    public class OpenApi
    {
        public static HttpClient httpClient = new HttpClient();
        public static string _url = "";
        /// <summary>
        /// 初始化一下下
        /// </summary>
        /// <param name="_appid"></param>
        /// <param name="_token"></param> 
        /// <param name="_sandbox"></param>
        public static void intApi(string _appid, string _token, bool _sandbox)
        {
            httpClient.DefaultRequestHeaders.Authorization
                    = new System.Net.Http.Headers.AuthenticationHeaderValue("Bot", $"{_appid}.{_token}"); // 照官方文档所说，需要在访问头里加入此消息用于验证BOT身份 否则返回401
            // 连接腾讯的服务器 , 也就是 https://api.sgroup.qq.com/ 来获取可以连接的服务器 如果_sandbox是true则是腾讯则是https://sandbox.api.sgroup.qq.com
            // sandbox环境只会收到测试频道的事件，且调用openapi仅能操作测试频道
            _url = _sandbox ? "https://sandbox.api.sgroup.qq.com" : "https://api.sgroup.qq.com";
        }
        public static bool CheckBot(string appid, string token)
        {
            try
            {
                HttpClient hClient = new HttpClient(); ;
                hClient.DefaultRequestHeaders.Authorization
                    = new System.Net.Http.Headers.AuthenticationHeaderValue("Bot", $"{appid}.{token}");

                var task = hClient.GetAsync("https://api.sgroup.qq.com/users/@me");
                task.Wait();
                return ((int)task.Result.StatusCode != 401 && (int)task.Result.StatusCode != 400);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 在频道发送普通消息
        /// </summary>
        /// <param name="channel_Id">频道ID</param>
        /// <param name="msg">消息</param>
        /// <param name="msgId">消息ID (主动发送消息的话就""或null)</param>
        /// <returns>
        /// 返回消息 ID 发送失败返回空文本
        /// </returns>
        public static string SendMsg(string channel_Id, string msg, string? msgId)
        {
            try
            {
                var res = httpClient.PostAsJsonAsync($"{_url}/channels/{channel_Id}/messages", new
                {
                    content = msg,
                    msg_id = msgId
                });
                res.Wait();

                // 调试输出状态 通过状态码判断发送结果
                Log.Log.LogStaus($"发送消息：{msg}", channel_Id, (int)res.Result.StatusCode);

                if ((int)res.Result.StatusCode == 200 || (int)res.Result.StatusCode == 204)
                {
                    var task2 = res.Result.Content.ReadAsStringAsync();
                    task2.Wait();
                    var data = JsonDocument.Parse(task2.Result);
                    return data.RootElement.GetProperty("id").GetString()!;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.SendMsg] " + e.Message);
                return "";
            }
        }
        /// <summary>
        /// 在频道发送普通消息并且上传一张图片
        /// </summary>
        /// <param name="channel_Id">频道ID</param>
        /// <param name="msg">消息</param>
        /// <param name="msgId">消息ID (主动发送消息的话就""或null)</param>
        /// /// <param name="imageUrl">图片链接地址 (需要已备案域名)</param>
        /// <returns>
        /// 返回状态码 200 或 201 204 为正常
        /// 返回消息 ID 发送失败返回空文本
        /// </returns>
        public static string SendMsg_Image(string channel_Id, string msg, string? msgId, string imageUrl)
        {
            try
            {
                var res = httpClient.PostAsJsonAsync($"{_url}/channels/{channel_Id}/messages", new
                {
                    content = msg,
                    msg_id = msgId,
                    image = imageUrl,
                });
                res.Wait();

                // 调试输出状态 通过状态码判断发送结果
                Log.Log.LogStaus($"发送消息图片：{msg} + 图片{imageUrl}", channel_Id, (int)res.Result.StatusCode);

                if ((int)res.Result.StatusCode == 200 || (int)res.Result.StatusCode == 204)
                {
                    var task2 = res.Result.Content.ReadAsStringAsync();
                    task2.Wait();
                    var data = JsonDocument.Parse(task2.Result);
                    return data.RootElement.GetProperty("id").GetString()!;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.SendMsg_Image] " + e.Message);
            }
            return "";
        }
        /// <summary>
        /// 发送消息EX
        /// </summary>
        /// <param name="channel_Id">频道ID</param>
        /// <param name="msg">消息主题</param>
        /// <param name="msgId">消息ID 如果为空文本则是主动消息</param>
        /// <param name="imageUrl">消息主题附带的图片URL</param>
        /// <param name="msgArk">消息附件</param>
        /// <param name="msgEmbed">类似于卡片消息..</param>
        /// <returns>
        /// 返回消息ID 失败返回空文本
        /// </returns>
        public static string SendMsgEx(string channel_Id, string msg, string? msgId, string? imageUrl, MessageArk? msgArk, MessageEmbed? msgEmbed)
        {
            try
            {
                var res = httpClient.PostAsJsonAsync($"{_url}/channels/{channel_Id}/messages", new
                {
                    content = msg,
                    msg_id = msgId,
                    image = imageUrl,
                    embed = msgEmbed,
                    ark = msgArk,
                });
                res.Wait();

                // 调试输出状态 通过状态码判断发送结果
                Log.Log.LogStaus($"发送消息Ex：{msg} + 图片{imageUrl} ARK{msgArk} EMBED{msgEmbed}", channel_Id, (int)res.Result.StatusCode);

                if ((int)res.Result.StatusCode == 200 || (int)res.Result.StatusCode == 204)
                {
                    var task2 = res.Result.Content.ReadAsStringAsync();
                    task2.Wait();
                    var data = JsonDocument.Parse(task2.Result);
                    return data.RootElement.GetProperty("id").GetString()!;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.SendMsgEx] " + e.Message);
            }
            return "";
        }
        /// <summary>
        /// 取成员信息
        /// 错误返回NULL
        /// </summary>
        /// <param name="guild_Id">频道ID</param>
        /// <param name="userId">成员ID</param>
        /// <returns></returns>
        public static Member? GetMemberInfo(string guild_Id, string userId)
        {
            try
            {
                var res = httpClient.GetAsync($"{_url}/guilds/{guild_Id}/members/{userId}");

                // 读取Response
                var response = res.Result.Content.ReadAsStringAsync();
                response.Wait(); // 等~~~~待~~~

                return JsonSerializer.Deserialize<Member>(response.Result)!;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.GetMemberInfo] " + e.Message);
                return null;
            }
        }
        /// <summary>
        /// 取成员信息 JSON版本
        /// 错误返回NULL
        /// </summary>
        /// <param name="guild_Id">频道ID</param>
        /// <param name="userId">成员ID</param>
        /// <returns></returns>
        public static string GetMemberInfo_JSON(string guild_Id, string userId)
        {
            try
            {
                var res = httpClient.GetAsync($"{_url}/guilds/{guild_Id}/members/{userId}");

                // 读取Response
                var response = res.Result.Content.ReadAsStringAsync();
                response.Wait(); // 等~~~~待~~~

                return response.Result;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.GetMemberInfo_JSON] " + e.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取频道信息A
        /// </summary>
        /// <param name="guild_id">频道ID</param>
        /// <returns></returns>
        public static string GetGuildInfoA(string guild_id)
        {
            try
            {
                var res = httpClient.GetAsync($"{_url}/guilds/{guild_id}");

                var response = res.Result.Content.ReadAsStringAsync();
                response.Wait();

                return response.Result;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.GetGuildInfoA] " + e.Message);
                return "";
            }
        }
        /// <summary>
        /// 获取频道信息
        /// </summary>
        /// <param name="guild_id">频道ID</param>
        /// <returns></returns>
        public static Guild GetGuildInfo(string guild_id)
        {
            try
            {
                var res = httpClient.GetAsync($"{_url}/guilds/{guild_id}");

                var response = res.Result.Content.ReadAsStringAsync();
                response.Wait();

                return JsonSerializer.Deserialize<Guild>(response.Result);
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.GetGuildInfo] " + e.Message);
                return new();
            }
        }
        /// <summary>
        /// 创建频道身份组
        /// </summary>
        /// <param name="filter">设置信息</param>
        /// <param name="info">信息</param>
        /// <param name="guild_Id">频道ID</param>
        /// <returns></returns>
        public static CreateGuildRoleResult? CreateGuildRole(Filter filter, Info info, string guild_Id)
        {
            try
            {
                CreateGuildRole createGuildRole = new CreateGuildRole();
                createGuildRole.Filter = filter;
                createGuildRole.Info = info;
                var res = httpClient.PostAsJsonAsync($"{_url}/guilds/{guild_Id}/roles", createGuildRole);
                res.Wait();
                var response = res.Result.Content.ReadAsStringAsync();
                response.Wait();

                return JsonSerializer.Deserialize<CreateGuildRoleResult>(response.Result);
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.CreateGuildRole] " + e.Message);
                return null;
            }
        }
        /// <summary>
        /// 创建频道身份组_A
        /// </summary>
        /// <param name="filter">设置信息</param>
        /// <param name="info">信息</param>
        /// <param name="guild_Id">频道ID</param>
        /// <returns>返回身份组ID</returns>
        public static string CreateGuildRoleA(int name, int color, int hoist, string Name, int Color, int Hoist, string guild_Id)
        {
            try
            {
                CreateGuildRole createGuildRole = new CreateGuildRole();
                createGuildRole.Filter = new Filter(name, color, hoist);
                createGuildRole.Info = new Info(Name, (uint)Color, Hoist);
                var res = httpClient.PostAsJsonAsync($"{_url}/guilds/{guild_Id}/roles", createGuildRole);
                res.Wait();
                var response = res.Result.Content.ReadAsStringAsync();
                response.Wait();
                Log.Log.LogStaus($"创建身份组 {Name}", guild_Id, (int)res.Result.StatusCode);
                CreateGuildRoleResult data = JsonSerializer.Deserialize<CreateGuildRoleResult>(response.Result)!;
                return data.Role_Id;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.CreateGuildRoleA] " + e.Message);
                return "";
            }
        }
        /// <summary>
        /// 修改频道身份组_A
        /// </summary>
        /// <param name="filter">指定要修改那些</param>
        /// <param name="info">指定要修改那些信息</param>
        /// <param name="guild_Id">频道ID</param>
        /// <param name="role_Id">CreateGuildRoleResult 或 </param>
        /// <returns></returns>
        public static string SetGuildRoleA(int name, int color, int hoist, string Name, int Color, int Hoist, string guild_Id, string role_Id)
        {
            try
            {
                CreateGuildRole createGuildRole = new CreateGuildRole();
                createGuildRole.Filter = new Filter(name, color, hoist);
                createGuildRole.Info = new Info(Name, (uint)Color, Hoist);
                var content = new StringContent(JsonSerializer.Serialize(createGuildRole), Encoding.UTF8, "application/json");
                var res = httpClient.PatchAsync($"{_url}/guilds/{guild_Id}/roles/{role_Id}", content);
                var response = res.Result.Content.ReadAsStringAsync();
                response.Wait();
                Log.Log.LogStaus($"修改频道身份组{Name} 颜色:{Color} 单独显示:{(Hoist == 1 ? "是" : "否")}", guild_Id, (int)res.Result.StatusCode);
                return JsonSerializer.Deserialize<CreateGuildRoleResult>(response.Result)!.Role_Id;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.SetGuildRoleA] " + e.Message);
                return "";
            }
        } // 待测试
        /// <summary>
        /// 修改频道身份组
        /// </summary>
        /// <param name="filter">指定要修改那些</param>
        /// <param name="info">指定要修改那些信息</param>
        /// <param name="guild_Id">频道ID</param>
        /// <param name="role_Id">CreateGuildRoleResult 或 </param>
        /// <returns></returns>
        public static CreateGuildRoleResult? SetGuildRole(Filter filter, Info info, string guild_Id, string role_Id)
        {
            try
            {
                CreateGuildRole createGuildRole = new CreateGuildRole();
                createGuildRole.Filter = filter;
                createGuildRole.Info = info;

                var content = new StringContent(JsonSerializer.Serialize(createGuildRole), Encoding.UTF8, "application/json");

                var res = httpClient.PatchAsync($"{_url}/guilds/{guild_Id}/roles/{role_Id}", content);

                var response = res.Result.Content.ReadAsStringAsync();
                response.Wait();

                return JsonSerializer.Deserialize<CreateGuildRoleResult>(response.Result);
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.SetGuildRole] " + e.Message);
                return null;
            }
        }
        /// <summary>
        /// 删除频道身份组
        /// </summary>
        /// <param name="guild_Id">频道ID</param>
        /// <param name="role_id">身份组ID</param>
        /// <returns></returns>
        public static bool DeleteGuildRole(string guild_Id, string role_Id)
        {
            try
            {
                var res = httpClient.DeleteAsync($"{_url}/guilds/{guild_Id}/roles/{role_Id}");
                res.Wait();
                return (int)res.Result.StatusCode == 204 || (int)res.Result.StatusCode == 200; // 他说返回204就是好了
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.DeleteGuildRole] " + e.Message);
                return false;
            }
        }
        /// <summary>
        /// 获取频道身份组列表
        /// </summary>
        /// <param name="guild_Id">频道ID</param>
        /// <returns></returns>
        public static GetGuildRoleInfo? GetGuildRoleList(string guild_Id)
        {
            try
            {
                var res = httpClient.GetStringAsync($"{_url}/guilds/{guild_Id}/roles");
                res.Wait();
                return JsonSerializer.Deserialize<GetGuildRoleInfo>(res.Result);
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.DeleteGuildRole] " + e.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取频道身份组列表A
        /// </summary>
        /// <param name="guild_Id">频道ID</param>
        /// <returns></returns>
        public static string GetGuildRoleListA(string guild_Id)
        {
            try
            {
                var res = httpClient.GetStringAsync($"{_url}/guilds/{guild_Id}/roles");
                Log.Log.LogStaus("获取身份组列表", guild_Id);
                res.Wait();
                return res.Result;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.DeleteGuildRoleA] " + e.Message);
                return "";
            }
        }

        /// <summary>
        /// 增加频道身份组成员
        /// </summary>
        /// <param name="channel">频道信息</param>
        /// <param name="guild_Id">频道ID</param>
        /// <param name="role_Id">身份组ID</param>
        /// <param name="user_Id">用户ID</param>
        /// <returns></returns>
        public static bool AddGuildRole(Channel channel, string guild_Id, string role_Id, string user_Id)
        {
            try
            {
                var res = httpClient.PutAsJsonAsync($"{_url}/guilds/{guild_Id}/members/{user_Id}/roles/{role_Id}", channel);
                res.Wait();
                return (int)res.Result.StatusCode == 204 || (int)res.Result.StatusCode == 200; // 他说返回204就是好了
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.AddGuildRole] " + e.Message);
                return false;
            }
        }
        /// <summary>
        /// 增加频道身份组成员A
        /// </summary>
        /// <param name="channel_Id">频道ID</param>
        /// <param name="guild_Id">频道ID</param>
        /// <param name="role_Id">身份组ID</param>
        /// <param name="user_Id">用户ID</param>
        /// <returns></returns>
        public static bool AddGuildRoleA(string channel_Id, string guild_Id, string role_Id, string user_Id)
        {
            try
            {
                var res = httpClient.PutAsJsonAsync($"{_url}/guilds/{guild_Id}/members/{user_Id}/roles/{role_Id}", GetChannelInfo(channel_Id));
                res.Wait();
                return (int)res.Result.StatusCode == 204 || (int)res.Result.StatusCode == 200; // 他说返回204就是好了
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.AddGuildRoleA] " + e.Message);
                return false;
            }
        }
        /// <summary>
        /// 删除频道身份组成员A
        /// </summary>
        /// <param name="channel">频道信息</param>
        /// <param name="guild_Id">频道ID</param>
        /// <param name="role_Id">身份组ID</param>
        /// <param name="user_Id">用户ID</param>
        /// <returns></returns>
        public static bool DeleteGuildRoleA(string channel_Id, string guild_Id, string role_Id, string user_Id)
        {
            try
            {

                var content = new StringContent(JsonSerializer.Serialize(GetChannelInfo(channel_Id)), Encoding.UTF8, "application/json");
                HttpRequestMessage httpRequestMessage = new()
                {
                    Content = new StringContent(JsonSerializer.Serialize(GetChannelInfo(channel_Id)), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"{_url}/guilds/{guild_Id}/members/{user_Id}/roles/{role_Id}"),
                };
                var res = httpClient.SendAsync(httpRequestMessage);
                res.Wait();
                return (int)res.Result.StatusCode == 204 || (int)res.Result.StatusCode == 200; // 他说返回204就是好了
            }
            catch (Exception e)
            {//那你快冲！冲！冲完了
                Log.Log.LogErr("[OpenApi.DeleteGuildRoleA] " + e.Message);
                return false;
            }
        }
        /// <summary>
        /// 删除频道身份组成员
        /// </summary>
        /// <param name="channel">频道信息</param>
        /// <param name="guild_Id">频道ID</param>
        /// <param name="role_Id">身份组ID</param>
        /// <param name="user_Id">用户ID</param>
        /// <returns></returns>
        public static bool DeleteGuildRole(Channel channel, string guild_Id, string role_Id, string user_Id)
        {
            try
            {

                var content = new StringContent(JsonSerializer.Serialize(channel), Encoding.UTF8, "application/json");
                HttpRequestMessage httpRequestMessage = new()
                {
                    Content = new StringContent(JsonSerializer.Serialize(channel), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"{_url}/guilds/{guild_Id}/members/{user_Id}/roles/{role_Id}"),
                };
                var res = httpClient.SendAsync(httpRequestMessage);
                res.Wait();
                return (int)res.Result.StatusCode == 204 || (int)res.Result.StatusCode == 200; // 他说返回204就是好了
            }
            catch (Exception e)
            {//那你快冲！冲！冲完了
                Log.Log.LogErr("[OpenApi.DeleteGuildRole] " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// 增加子频道公告
        /// </summary>
        /// <param name="channel_Id">子频道ID</param>
        /// <param name="message_Id">消息ID</param>
        /// <returns>返回公告消息ID 失败返回空文本</returns>
        public static string CreateChannelAnnounce(string channel_Id, string message_Id)
        {
            try
            {
                var task = httpClient.PostAsJsonAsync($"{_url}/channels/{channel_Id}/announces", new { message_id = message_Id });
                task.Wait();

                if ((int)task.Result.StatusCode >= 200 && (int)task.Result.StatusCode < 210)
                {
                    var task2 = task.Result.Content.ReadAsStringAsync();
                    task2.Wait();

                    var response = JsonDocument.Parse(task2.Result);
                    return response.RootElement.GetProperty("message_id").GetString()!;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.CreateChannelAnnounce] " + e.Message);
                return "";
            }
        }
        /// <summary>
        /// 删除频道公告
        /// </summary>
        /// <param name="channel_Id">子频道ID</param>
        /// <param name="message_Id">欲删除消息ID</param>
        /// <returns></returns>
        public static bool DeleteChannelAnnounce(string channel_Id, string message_Id)
        {
            try
            {
                var task = httpClient.DeleteAsync($"{_url}/channels/{channel_Id}/announces/{message_Id}");
                task.Wait();
                return (int)task.Result.StatusCode == 200;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.DeleteChannelAnnounce] " + e.Message);
                return false;
            }
        }
        /// <summary>
        /// 获取子频道信息A
        /// </summary>
        /// <param name="channel_Id">子频道ID</param>
        /// <returns>失败不返回 null 返回啥也没有的Channel对象</returns>
        public static string GetChannelInfoA(string channel_Id)
        {
            try
            {
                var task = httpClient.GetStringAsync($"{_url}/channels/{channel_Id}");
                task.Wait();
                Log.Log.LogStaus($"获取子频道信息", channel_Id);
                return task.Result;
            }
            catch (Exception e)
            {
                Log.Log.LogStaus($"获取子频道信息 失败!", channel_Id);
                Log.Log.LogErr("[OpenApi.GetChannelInfoA] " + e.Message);
                return "";

            }
        }
        /// <summary>
        /// 获取子频道信息
        /// </summary>
        /// <param name="channel_Id">子频道ID</param>
        /// <returns>失败不返回 null 返回啥也没有的Channel对象</returns>
        public static Channel GetChannelInfo(string channel_Id)
        {
            Channel channel = new();
            try
            {
                var task = httpClient.GetStringAsync($"{_url}/channels/{channel_Id}");
                task.Wait();
                channel = JsonSerializer.Deserialize<Channel>(task.Result)!;
                return channel;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.GetChannelInfo] " + e.Message);
                return channel;
            }
        }

        /// <summary>
        /// 获取频道下的子频道列表
        /// (注意: 子频道分组信息也会获取)
        /// </summary>
        /// <param name="guild_Id">频道ID</param>
        /// <returns></returns>
        public static Channel[]? GetGuildChannelList(string guild_Id)
        {
            try
            {
                var task = httpClient.GetStringAsync($"{_url}/guilds/{guild_Id}/channels");
                task.Wait();
                return JsonSerializer.Deserialize<Channel[]>(task.Result)!;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.GetGuildChannelList] " + e.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取频道下的子频道列表A
        /// (注意: 子频道分组信息也会获取)
        /// </summary>
        /// <param name="guild_Id">频道ID</param>
        /// <returns></returns>
        public static string GetGuildChannelListA(string guild_Id)
        {
            try
            {
                var task = httpClient.GetStringAsync($"{_url}/guilds/{guild_Id}/channels");
                task.Wait();
                return task.Result;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.GetGuildChannelListA] " + e.Message);
                return "";
            }
        }

        /// <summary>        /// <summary>
        /// 获取指定子频道成员的权限
        /// </summary>
        /// <param name="channel_Id">子频道ID</param>
        /// <param name="user_Id">用户ID</param>
        /// <returns>失败返回null</returns>
        public static ChannelPermissions GetChannelPermissions(string channel_Id, string user_Id)
        {
            try
            {
                var task = httpClient.GetStringAsync($"{_url}/channels/{channel_Id}/members/{user_Id}/permissions");
                task.Wait();
                return JsonSerializer.Deserialize<ChannelPermissions>(task.Result);
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.ChannelPermissions] " + e.Message);
                return new();
            }
        }
        /// <summary>
        /// 获取指定子频道成员的权限A
        /// </summary>
        /// <param name="channel_Id">子频道ID</param>
        /// <param name="user_Id">用户ID</param>
        /// <returns>失败返回null</returns>
        public static string GetChannelPermissionsA(string channel_Id, string user_Id)
        {
            try
            {
                var task = httpClient.GetStringAsync($"{_url}/channels/{channel_Id}/members/{user_Id}/permissions");
                task.Wait();
                return task.Result;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.ChannelPermissions] " + e.Message);
                return "";
            }
        }
        /// 修改指定子频道的权限
        /// 参数包括add和remove两个字段，分别表示授予的权限以及删除的权限。要授予用户权限即把add对应位置1，删除用户权限即把remove对应位置1。当两个字段同一位都为1，表现为删除权限。
        /// </summary>
        /// <param name="channel_Id">子频道ID</param>
        /// <param name="user_Id">用户ID</param>
        /// <param name="addPermissions">-</param>
        /// <param name="removePermissions">-</param>
        /// <returns></returns>
        public static bool SetChannelPermissions(string channel_Id, string user_Id, int addPermissions, int removePermissions)
        {
            try
            {
                var task = httpClient.PutAsJsonAsync($"{_url}/channels/{channel_Id}/members/{user_Id}/permissions", new
                {
                    add = addPermissions,
                    remove = removePermissions
                });
                task.Wait();
                return (int)task.Result.StatusCode == 200 || (int)task.Result.StatusCode == 204;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.SetChannelPermissions] " + e.Message);
                return false;
            }
        }
        /// <summary>
        /// 控制音频
        /// </summary>
        /// <param name="channel_Id">子频道ID</param>
        /// <param name="audioContro">音频数据</param>
        /// <returns></returns>
        public static bool AudioControl(string channel_Id, AudioContro audioContro)
        {
            try
            {
                var task = httpClient.PostAsJsonAsync($"{_url}/channels/{channel_Id}/audio", audioContro);
                task.Wait();
                var task2 = task.Result.Content.ReadAsStringAsync();
                task2.Wait();
                return task2.Result == "{}" || (int)task.Result.StatusCode == 200 || (int)task.Result.StatusCode == 204; // 腾讯文档说返回空json就成功
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.AudioControl] " + e.Message);
                return false;
            }
        }
        /// <summary>
        /// 获取当前登录账号信息 (也就是bot信息
        /// </summary>
        /// <returns></returns>
        public static User GetBotInfo()
        {
            try
            {
                var task = httpClient.GetStringAsync($"{_url}/users/@me");
                task.Wait();
                return (JsonSerializer.Deserialize<User>(task.Result));
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.GetBotInfo] " + e.Message);
                return new();
            }
        }
        /// <summary>
        /// 获取当前登录账号信息A (也就是bot信息
        /// </summary>
        /// <returns></returns>
        public static string GetBotInfoA()
        {
            try
            {
                var task = httpClient.GetStringAsync($"{_url}/users/@me");
                task.Wait();
                return task.Result;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.GetBotInfo] " + e.Message);
                return "";
            }
        }
        /// <summary>
        /// 获取当前机器人加入的 频道列表
        /// </summary>
        /// <param name="before">读此id之前的数据 (before或after只能带一个)</param>
        /// <param name="after">读此id之后的数据 (before或after只能带一个)</param>
        /// <param name="limit">每次拉取多少条数据	最多100个</param>
        /// <returns></returns>
        public static Guild[] GetGuildList(string before, string after, int limit)
        {
            try
            {
                HttpRequestMessage httpRequestMessage = new()
                {
                    Content = new StringContent(JsonSerializer.Serialize(new
                    {
                        before = before,
                        after = after,
                        limit = limit,
                    }), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{_url}/users/@me/guilds"),
                };

                var task = httpClient.SendAsync(httpRequestMessage);
                task.Wait();
                var task2 = task.Result.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Guild[]>(task2.Result)!;

            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.GetGuildList] " + e.Message);
                return Array.Empty<Guild>();
            }
        }
        /// <summary>
        /// 获取频道日程列表
        /// </summary>
        /// <param name="channel_Id">子频道ID</param>
        /// <param name="since">起始时间戳(ms) 如果为null 则默认返回当天的日程列</param>
        /// <returns></returns>
        public static Schedule[] GetChannelScheduleList(string channel_Id, int? since)
        {
            try
            {
                HttpRequestMessage httpRequestMessage = new()
                {
                    Content = new StringContent(JsonSerializer.Serialize(new
                    {
                        since = since,
                    }), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{_url}/channels/{channel_Id}/schedules"),
                };

                var task = httpClient.SendAsync(httpRequestMessage);
                task.Wait();
                var task2 = task.Result.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Schedule[]>(task2.Result)!;

            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.GetChannelScheduleList] " + e.Message);
                return Array.Empty<Schedule>();
            }
        }
        /// <summary>
        /// 获取单个日程信息
        /// </summary>
        /// <param name="channel_Id">子频道ID</param>
        /// <param name="schedule_Id">日程ID</param>
        /// <returns></returns>
        public static Schedule GetChannelScheduleInfo(string channel_Id, string schedule_Id)
        {
            try
            {
                var task = httpClient.GetStringAsync($"{_url}/channels/{channel_Id}/schedules/{schedule_Id}");
                task.Wait();
                return JsonSerializer.Deserialize<Schedule>(task.Result)!;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.GetChannelScheduleInfo] " + e.Message);
                return new();
            }
        }
        /// <summary>
        /// 创建日程
        /// </summary>
        /// <param name="channel_Id">子频道ID</param>
        /// <param name="schedule">日程对象 不需要带ID</param>
        /// <returns>返回新的日程对象 错误返回null</returns>
        public static Schedule CreateChannelSchedule(string channel_Id, Schedule schedule)
        {
            try
            {
                var task = httpClient.PostAsJsonAsync($"{_url}/channels/{channel_Id}/schedules", schedule);
                task.Wait();
                var task2 = task.Result.Content.ReadAsStringAsync();
                task2.Wait();
                return JsonSerializer.Deserialize<Schedule>(task2.Result)!;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.CreateChannelSchedule] " + e.Message);
                return new();
            }
        }
        /// <summary>
        /// 修改日程
        /// </summary>
        /// <param name="channel_Id">子频道ID</param>
        /// <param name="schedule_Id">日程ID</param>
        /// <param name="schedule">日程对象 不需要带ID</param>
        /// <returns>返回修改后的日程对象</returns>
        public static Schedule SetChannelSchedule(string channel_Id, string schedule_Id, Schedule schedule)
        {
            try
            {
                HttpRequestMessage httpRequestMessage = new()
                {
                    Content = new StringContent(JsonSerializer.Serialize(schedule), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri($"{_url}/channels/{channel_Id}/schedules/{schedule_Id}"),
                };
                var task = httpClient.SendAsync(httpRequestMessage);
                task.Wait();
                var task2 = task.Result.Content.ReadAsStringAsync();
                task2.Wait();
                return JsonSerializer.Deserialize<Schedule>(task2.Result)!;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.SetChannelSchedule] " + e.Message);
                return new();
            }
        }
        /// <summary>
        /// 删除日程
        /// </summary>
        /// <param name="channel_Id">子频道ID</param>
        /// <param name="schedule_Id">日程ID</param>
        /// <returns></returns> 
        public static bool DeleteChannelSchedule(string channel_Id, string schedule_Id)
        {
            try
            {
                var task = httpClient.DeleteAsync($"{_url}/channels/{channel_Id}/schedules/{schedule_Id}");
                task.Wait();
                return (int)task.Result.StatusCode == 200 || (int)task.Result.StatusCode == 204;
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.DeleteChannelSchedule] " + e.Message);
                return false;
            }
        }
        /// <summary>
        /// 设置全频道禁言
        /// </summary>
        /// <param name="guild_Id">频道ID</param>
        /// <param name="muteEndTimestamp">禁言到期时间戳，绝对时间戳，单位：秒（与 mute_seconds 字段同时赋值的话，以该字段为准）</param>
        /// <param name="muteSeconds">禁言多少秒（两个字段二选一，默认以 mute_end_timestamp 为准）</param>
        /// <returns></returns>
        public static bool MuteGuild(string guild_Id,string muteEndTimestamp,string muteSeconds)
        {
            try
            {
                if(muteEndTimestamp != "")
                {
                    HttpRequestMessage httpRequestMessage = new()
                    {
                        Content = new StringContent(JsonSerializer.Serialize(new
                        {
                            mute_end_timestamp = muteEndTimestamp,
                        }), Encoding.UTF8, "application/json"),
                        Method = HttpMethod.Patch,
                        RequestUri = new Uri($"{_url}/guilds/{guild_Id}/mute"),
                    };
                    var task = httpClient.SendAsync(httpRequestMessage);
                    task.Wait();
                    Log.Log.LogStaus("置频道全体禁言", guild_Id, (int)task.Result.StatusCode);
                    return (int)task.Result.StatusCode == 200 || (int)task.Result.StatusCode == 204;
                }
                else
                {
                    HttpRequestMessage httpRequestMessage = new()
                    {
                        Content = new StringContent(JsonSerializer.Serialize(new
                        {
                            mute_seconds = muteSeconds
                        }), Encoding.UTF8, "application/json"),
                        Method = HttpMethod.Patch,
                        RequestUri = new Uri($"{_url}/guilds/{guild_Id}/mute"),
                    };
                    var task = httpClient.SendAsync(httpRequestMessage);
                    task.Wait();
                    Log.Log.LogStaus("置频道全体禁言", guild_Id, (int)task.Result.StatusCode);
                    return (int)task.Result.StatusCode == 200 || (int)task.Result.StatusCode == 204;
                }
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.MuteGuild] " + e.Message);
                return false;
            }
        }
        /// <summary>
        /// 设置成员禁言
        /// </summary>
        /// <param name="guild_Id">频道ID</param>
        /// <param name="user_Id">用户ID</param>
        /// <param name="muteEndTimestamp">禁言到期时间戳，绝对时间戳，单位：秒（与 mute_seconds 字段同时赋值的话，以该字段为准）</param>
        /// <param name="muteSeconds">禁言多少秒（两个字段二选一，默认以 mute_end_timestamp 为准）</param>
        /// <returns></returns>
        public static bool MuteGuildUser(string guild_Id, string user_Id, string muteEndTimestamp, string muteSeconds)
        {
            try
            {
                if(muteEndTimestamp != "")
                {
                    HttpRequestMessage httpRequestMessage = new()
                    {
                        Content = new StringContent(JsonSerializer.Serialize(new
                        {
                            mute_end_timestamp = muteEndTimestamp
                        }), Encoding.UTF8, "application/json"),
                        Method = HttpMethod.Patch,
                        RequestUri = new Uri($"{_url}/guilds/{guild_Id}/members/{user_Id}/mute"),
                    };
                    var task = httpClient.SendAsync(httpRequestMessage);
                    task.Wait();
                    Log.Log.LogStaus("置频道成员禁言", guild_Id, (int)task.Result.StatusCode);
                    return (int)task.Result.StatusCode == 200 || (int)task.Result.StatusCode == 204;
                }
                else
                {
                    HttpRequestMessage httpRequestMessage = new()
                    {
                        Content = new StringContent(JsonSerializer.Serialize(new
                        {
                            mute_seconds = muteSeconds
                        }), Encoding.UTF8, "application/json"),
                        Method = HttpMethod.Patch,
                        RequestUri = new Uri($"{_url}/guilds/{guild_Id}/members/{user_Id}/mute"),
                    };
                    var task = httpClient.SendAsync(httpRequestMessage);
                    task.Wait();
                    Log.Log.LogStaus("置频道成员禁言", guild_Id, (int)task.Result.StatusCode);
                    return (int)task.Result.StatusCode == 200 || (int)task.Result.StatusCode == 204;
                }
            }
            catch (Exception e)
            {
                Log.Log.LogErr("[OpenApi.MuteGuild] " + e.Message);
                return false;
            }
        }
    }
}