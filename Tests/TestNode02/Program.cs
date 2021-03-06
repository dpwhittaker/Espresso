﻿//MIT, 2015-2017, EngineKit, brezza92
using System;
using System.IO;
using Espresso;

namespace TestNode01
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //-----------------------------------
            //1.
            //after we build nodejs in dll version
            //we will get node.dll
            //then just copy it to another name 'libespr'   
            string currentdir = System.IO.Directory.GetCurrentDirectory();
            string libEspr = @"../../../node-v8.1.4/Release/libespr.dll";
            if (File.Exists(libEspr))
            {
                //delete the old one
                File.Delete(libEspr);
            }
            File.Copy(
               @"../../../node-v8.1.4/Release/node.dll", //from
               libEspr);
            //-----------------------------------
            //2. load libespr.dll (node.dll)

            IntPtr intptr = LoadLibrary(libEspr);
            int errCode = GetLastError();
            int libesprVer = JsBridge.LibVersion;

            //change working dir to target app and run 
            //test with socket.io's chat sample
            System.IO.Directory.SetCurrentDirectory(@"../../../socket.io/examples/chat");

#if DEBUG
            JsBridge.dbugTestCallbacks();
#endif
            //------------ 
            JsEngine.RunJsEngine((IntPtr nativeEngine, IntPtr nativeContext) =>
            {

                JsEngine eng = new JsEngine(nativeEngine);
                JsContext ctx = eng.CreateContext(nativeContext);
                //-------------
                //this LibEspressoClass object is need,
                //so node can talk with us,
                //-------------
                JsTypeDefinition jstypedef = new JsTypeDefinition("LibEspressoClass");
                jstypedef.AddMember(new JsMethodDefinition("LoadMainSrcFile", args =>
                {
                    //since this is sample socket io app
                    string filedata = File.ReadAllText("index.js");
                    args.SetResult(filedata);
                }));
                jstypedef.AddMember(new JsMethodDefinition("C", args =>
                {

                    args.SetResult(true);
                }));
                jstypedef.AddMember(new JsMethodDefinition("E", args =>
                {
                    args.SetResult(true);
                }));
                if (!jstypedef.IsRegisterd)
                {
                    ctx.RegisterTypeDefinition(jstypedef);
                }
                //----------
                //then register this as x***       
                //this object is just an instance for reference        
                ctx.SetVariableFromAny("LibEspresso",
                      ctx.CreateWrapper(new object(), jstypedef));
            });

            string userInput = Console.ReadLine();

        }


        private static void Proc_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {

        }

        private static void Proc_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {

        }

        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        static extern IntPtr LoadLibrary(string dllname);
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        static extern int GetLastError();
    }
}
