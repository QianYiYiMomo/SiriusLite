using Lang.HttpApi.Entity;
using Lang.Plugin;
using System.Net.WebSockets;

namespace Lang.HttpApi
{
    public class HttpApiPlugin
    {
        private readonly AuthPlugin _authPlugin;
        private readonly string _pluginToken;
        public string pluginToken => _pluginToken;
        public AuthPlugin authPlugin => _authPlugin;
        public HttpApiPlugin(AuthPlugin authPlugin)
        {
            _authPlugin = authPlugin;
            _pluginToken = SiriusTools.GetRandomKey();
        }
    }
}
