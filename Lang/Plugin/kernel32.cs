using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpClient
{
    public class kernel32
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibraryA(string lpLibFileName);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr dllHandle, string funcName);
        [DllImport("kernel32.dll")]
        public static extern IntPtr FreeLibrary(IntPtr dllHandle);
        [DllImport("kernel32.dll")]
        public static extern bool IsBadCodePtr(IntPtr dllHandle);
    }
}
