using System.Net.WebSockets;

namespace Lang.HttpApi
{
    public class HttpApiWebsocketDelegates
    {
        public delegate void OnMessage(HttpApiPlugin webSocket, string message);
        public delegate void OnConnect(HttpApiPlugin websocket);
        public delegate void OnDisconnect(HttpApiPlugin websocket);
    }
}
