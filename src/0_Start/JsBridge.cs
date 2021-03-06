﻿//MIT, 2015-2017, WinterDev, EngineKit, brezza92
using System;
using System.Runtime.InteropServices;
namespace Espresso
{

    public delegate void NativeEngineSetupCallback(IntPtr nativeEngine, IntPtr nativeContext);

    public static partial class JsBridge
    {
        public const string LIB_NAME = "libespr";
        static JsBridge()
        {

        }
        public static void V8Init()
        {
            NativeV8JsInterOp.V8Init();
        }
        public static int LibVersion
        {
            get { return getVersion(); }
        }

        [DllImport(JsBridge.LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        static extern int getVersion();
#if DEBUG
        public static void dbugTestCallbacks()
        {
            NativeV8JsInterOp.RegisterCallBacks();
            NativeV8JsInterOp.TestCallBack();
        }
#endif
    }

}