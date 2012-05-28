using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftGPL.jsCompiler.Core
{

    public class Compiler
    {
        /// <summary>
        /// Currently, there are two modes for compiling JavaScript with google
        /// closure compiler:
        /// 
        /// 1. JNI, which compiles code directly by calling the google closure
        /// compiler in process via JNI.  This will meet most needs.  Less flexible
        /// if you need to use closure features that are only available via command
        /// line. It will outperform Command Line mode, particularly noteable with
        /// larger number of JavaScript source files.
        /// 
        /// 2. Command Line, which feeds all the data into the google closure
        /// command line compiler.  This will meet your needs if you need to use
        /// your own closure compiler flags that aren't currently handle via JNI.
        /// </summary>
        public enum ECompilerType
        {
            JNI,  // Java Native Interface
            CMD   // Command line
        }


        /// <summary>
        /// Instance of the Google Closure Compiler.  We feed compiler options into it
        /// to get compiled JavaScript, Warnings and Errors back out.
        /// </summary>
        protected jsCompiler.Core.ICompiler gcCompiler
        {
            get;
            private set;
        }


        /// <summary>
        /// Compiler type for the current instance.
        /// </summary>
        /// 
        public ECompilerType Type
        {
            get;
            private set;
        }


        public Compiler()
            : this(ECompilerType.JNI)
        {
        }


        public Compiler(ECompilerType type)
        {
            Type = type;

            switch (Type)
            {
                case ECompilerType.JNI:
                    {
                        // Create the instance of the JNI closure compiler
                        gcCompiler = new jsCompiler.Core.JNICompiler();
                        break;
                    }
                case ECompilerType.CMD:
                    {
                        // Create the instance of the command line compiler
                        gcCompiler = new jsCompiler.Core.CMDCompiler();
                        break;
                    }
            }
        }


        /// <summary>
        /// Invokes the google closure compiler with the provided options.  At the very least,
        /// a list of JavaScript files need to be provided.
        /// </summary>
        /// <param name="options">CompilerOptions for the google closure compiler.</param>
        /// <returns>Result object, which has the compiled JavaScript, Warnings and Errors</returns>
        /// 
        public jsCompiler.Core.Result Compile(CompilerOptions options)
        {
            return gcCompiler.Compile(options);
        }


        /// <summary>
        /// Get the version of the google closure compiler
        /// </summary>
        /// <returns>Returns google closure compiler version as a string</returns>
        /// 
        public string Version()
        {
            return gcCompiler.Version();
        }

    }
}

