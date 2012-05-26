using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Reflection;
using System.IO;

using net.sf.jni4net;


namespace SoftGPL.jsCompiler
{

    /// <summary>
    /// JNI implementation for the google closure compiler.
    /// </summary>
    internal class JNICompiler : ICompiler
    {

        /// <summary>
        /// BridgeSet for instanciating the JVM used for working with google closure compiler.
        /// </summary>
        private BridgeSetup _BridgeSetup = null;

        /// <summary>
        /// Home directory
        /// </summary>
        public static string Home = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);


        /// <summary>
        /// Constructor
        /// </summary>
        public JNICompiler()
        {
            com.google.javascript.jscomp.Compiler _gc = gcCompiler;
        }


        public Result Compile(CompilerOptions options)
        {
            com.google.javascript.jscomp.CompilerOptions gcOptions = jsCompiler.JNICompilerOptions.Instance.Configure(options);

            com.google.javascript.jscomp.JSSourceFile[] gcInputFile = jsCompiler.gcBridge.SourceFiles.Configure(options);
            com.google.javascript.jscomp.JSSourceFile[] gcExternFile = jsCompiler.gcBridge.ExternFiles.Configure(options);

            // Compile the source...
            com.google.javascript.jscomp.Result gcResult = gcCompiler.compile(gcExternFile, gcInputFile, gcOptions);
            Result result = Result.fromJNI(gcCompiler, gcResult, options);

            // We set this to null so that we get a new instance when we compile source again.
            gcCompiler = null;
            return result;
        }


        public string Version()
        {
            return String.Format("Version: {0}. Date: {1}", com.google.javascript.jscomp.Compiler.getReleaseVersion(), com.google.javascript.jscomp.Compiler.getReleaseDate());
        }


        /// <summary>
        /// google compiler instance...  This is where the magic happens!
        /// </summary>
        private com.google.javascript.jscomp.Compiler _gcCompiler = null;
        private com.google.javascript.jscomp.Compiler gcCompiler
        {
            get
            {
                if (_BridgeSetup == null)
                {
                    _BridgeSetup = new BridgeSetup(false);

                    if (SoftGPL.Common.Process.Java.Home != String.Empty)
                        _BridgeSetup.JavaHome = SoftGPL.Common.Process.Java.Home;

                    _BridgeSetup.AddJVMOption("-Xmx1024m");
                    _BridgeSetup.AddAllJarsClassPath(Home + "/lib/jni4net");
                    _BridgeSetup.AddAllJarsClassPath(Home + "/lib/proxygen.j4n");
                    _BridgeSetup.AddAllJarsClassPath(Home + "/lib/closure");
                    Bridge.CreateJVM(_BridgeSetup);
                    Bridge.RegisterAssembly(typeof(com.google.javascript.jscomp.Compiler).Assembly);
                }

                if (_gcCompiler == null)
                {
                    // We cannot cache because google compiler will check if
                    // the compiler object has been called more than once and
                    // it will throw an error if true.
                    //
                    _gcCompiler = new com.google.javascript.jscomp.Compiler();
                }

                return _gcCompiler;
            }
            set
            {
                _gcCompiler = value;
            }
        }

    }

}

