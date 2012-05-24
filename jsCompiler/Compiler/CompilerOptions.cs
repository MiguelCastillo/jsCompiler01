using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace jsCompiler
{

    /// <summary>
    /// Option container that then gets tranlated to exactly what the
    /// closure compiler is expecting.
    /// </summary>
    sealed public class CompilerOptions
    {

        public enum ECompilerLevel
        {
            Default,
            WhiteSpaceOnly,
            SimpleOptimization,
            AdvancedOptimization
        }

        public enum EWarningLevel
        {
            Default,
            Quiet,
            Verbose
        }

        public enum EOutputFormatting
        {
            Default,
            PrettyPrint,
            PrintInputDelimeter
        }



        /// <summary>
        /// Java Home is the root directory for the jre or jdk. E.g.
        /// "C:\Program Files (x86)\Java\jdk1.7.0_04"
        /// "C:\Program Files (x86)\Java\jre7"
        /// </summary>
        public string JavaHome
        {
            get { return Java.Home; }
            set { Java.Home = value; }
        }


        /// <summary>
        /// Container for all the files that are passed to the closure compiler.
        /// This container only has fully qualfied string names.
        /// </summary>
        public ArrayList InputFiles = null;

        /// <summary>
        /// Name of the file the compiler writes the compiled JavaScript to.
        /// </summary>
        public string OutputFile = String.Empty;
        public bool Debug = false;

        /// <summary>
        /// IdeMode will configure the closure compiler to not stop when it encouters
        /// errors.
        /// </summary>
        public bool IdeMode = true;

        /// <summary>
        /// Compiler level for setting how much processing needs to be done to
        /// the JavaScript files.
        /// </summary>
        public ECompilerLevel CompilerLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Warning level control how verbose the compiler is when warnings
        /// and errors are encounter.
        /// </summary>
        public EWarningLevel WarningLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Tells the compiler how much to obfuscate and compresse the output script.
        /// </summary>
        public EOutputFormatting OutputFormatting
        {
            get;
            set;
        }

        /// <summary>
        /// Special variable to hold custom arguments that are passed directly
        /// to the closure compiler...
        /// </summary>
        public string ExtraArgs = String.Empty;

        /// <summary>
        /// Diagnostic group options to control how the compiler should treat
        /// different "problems" found while compiling the source files.
        /// </summary>
        public DiagnosticGroup DiagnosticGroup = null;


        public CompilerOptions()
        {
            InputFiles = new ArrayList();
            DiagnosticGroup = new DiagnosticGroup();
            CompilerLevel = ECompilerLevel.Default;
            WarningLevel = EWarningLevel.Default;
            OutputFormatting = EOutputFormatting.Default;
        }

    }

}

