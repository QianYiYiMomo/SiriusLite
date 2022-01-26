using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lang.Data
{

        public enum OpCode
        {
            /// <summary>
            /// 服务端正常推送消息
            /// </summary>
            Dispatch = 0,
            /// <summary>
            /// 客户端或服务端发送心跳
            /// </summary>
            Heartbeat = 1,
            /// <summary>
            /// 客户端发送心跳包
            /// </summary>
            Identify = 2,
            /// <summary>
            /// 客户端恢复连接
            /// </summary>
            Resume = 6,
            /// <summary>
            /// 服务端通知客户端重新连接
            /// </summary>
            Reconnect = 7,
            /// <summary>
            /// 当identify或resume的时候，如果参数有错，服务端会返回该消息
            /// </summary>
            InvalidSession = 9,
            /// <summary>
            /// 当客户端与网关建立ws连接之后，网关下发的第一条消息
            /// </summary>
            hello = 10,
            /// <summary>
            /// 当发送心跳成功之后，就会收到该消息
            /// </summary>
            HeartbeatACK = 11,
        }
    
}
