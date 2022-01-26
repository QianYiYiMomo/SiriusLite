using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lang.Plugin.Interface
{
    public interface IPlugin : IDisposable
    {
        public void OnStart();
        public void OnStop();
        public void OnMessage(string jsonData);
    }
}
