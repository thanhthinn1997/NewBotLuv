using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NewBotLuv
{
    static class MapleEngine
    {
        // interface callback definitions
        public delegate void TextCallBack(IntPtr data, int tag, [In, MarshalAs(UnmanagedType.LPStr)] String output);
        public delegate void ErrorCallBack(IntPtr data, IntPtr offset, [In, MarshalAs(UnmanagedType.LPStr)] String msg);
        public delegate void StatusCallBack(IntPtr data, IntPtr used, IntPtr alloc, double time);
        public delegate IntPtr ReadLineCallBack(IntPtr data, IntPtr debug);
        public delegate long RedirectCallBack(IntPtr data, [In, MarshalAs(UnmanagedType.LPStr)] String name, [In, MarshalAs(UnmanagedType.LPStr)] String mode);
        public delegate IntPtr StreamCallBack(IntPtr data, [In, MarshalAs(UnmanagedType.LPStr)] String stream, int nargs, String[] args);
        public delegate long QueryInterrupt(IntPtr data);
        public delegate IntPtr CallBackCallBack(IntPtr data, [In, MarshalAs(UnmanagedType.LPStr)] String output);

        public struct MapleCallbacks
        {
            public TextCallBack textCallBack;
            public ErrorCallBack errorCallBack;
            public StatusCallBack statusCallBack;
            public ReadLineCallBack readlineCallBack;
            public RedirectCallBack redirectCallBack;
            public StreamCallBack streamCallBack;
            public QueryInterrupt queryInterrupt;
            public CallBackCallBack callbackCallBack;
        }

        // OpenMaple API methods (there are many more commands in the API,
        // these are just the ones we are using in this example)
        [DllImport("maplec.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr StartMaple(int argc, String[] argv, ref MapleCallbacks cb, IntPtr data, IntPtr info, byte[] err);

        [DllImport("maplec.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr EvalMapleStatement(IntPtr kv, [In, MarshalAs(UnmanagedType.LPStr)] String statement);

        [DllImport("maplec.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr xIsMapleStop(IntPtr kv, IntPtr obj);
        public static bool IsMapleStop(IntPtr kv, IntPtr obj)
        {
            //IntPtr r = xIsMapleStop(kv,obj);
            //return r.ToInt32() == 0 ? true : false;
            return xIsMapleStop(kv, obj).ToInt32() == 0 ? true : false;
        }

        [DllImport("maplec.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void StopMaple(IntPtr kv);
    }
}
