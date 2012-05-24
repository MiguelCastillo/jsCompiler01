using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jsCompiler
{

    internal class CMDCompilerOptions
    {

        private CMDCompilerOptions()
        {
        }


        /// <summary>
        /// Singleton instance for tanforming an option object to a string that is compatible
        /// with the google closure command line compiler.
        /// </summary>
        static internal CMDCompilerOptions Instance = new CMDCompilerOptions();


        /// <summary>
        /// Builds up a string that is sent through the CommandLine google closure compiler.
        /// </summary>
        /// <param name="options">Compiler options that need to be processed</param>
        /// <returns>String containing all the options as needed by the compiler</returns>
        public string Configure(CompilerOptions options)
        {
            string input = getInputFiles(options);

            if (input == String.Empty)
                return String.Empty;

            string extraArgs = getExtraArg(options);
            string output = getOutputFile(options);
            string debug = getDebug(options);
            string compilation_level = getCompilationLevel(options);
            string warning_level = getWarningLevel(options);
            string outputformat = getOutputFormat(options);
            string idemode = getIdeMode(options);
            return debug + compilation_level + extraArgs + warning_level + outputformat + idemode + input + output;
        }


        /// <summary>
        /// Process the argument string and returns a jsCompiler ready string
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private string getExtraArg(CompilerOptions options)
        {
            string result = options.ExtraArgs.Trim();
            return result.Length != 0 ? " " + result : String.Empty;
        }


        /// <summary>
        /// Takes in an array of files and appends the --js required by the
        /// closure compiler.
        /// </summary>
        /// <param name="files">List of files that need processing</param>
        /// <returns>List of files with the --js closure setting</returns>
        private string getInputFiles(CompilerOptions options)
        {
            if (options.InputFiles.Count == 0)
                return String.Empty;

            StringBuilder result = new StringBuilder(5084);
            for (int iFile = 0, length = options.InputFiles.Count; iFile < length; iFile++)
                result.Append(" --js=\"" + options.InputFiles[iFile] + "\"");
            return result.ToString();
        }


        /// <summary>
        /// Extracts the output file, if one is provided, and applies the proper closure
        /// compiler flag.  If no outfile file is provided, then we don't provide anything
        /// to the closure compiler, which result in sending the resulting JS to the stdout.
        /// </summary>
        /// <param name="options"></param>
        /// <returns>js file name with the --js_output_file closure options</returns>
        private string getOutputFile(CompilerOptions options)
        {
            if (options.OutputFile != String.Empty)
                return " --js_output_file=\"" + options.OutputFile + "\"";
            return String.Empty;
        }


        /// <summary>
        /// Sets the debug flag in the closure compiler
        /// </summary>
        /// <param name="options"></param>
        /// <returns>If enabled, the string --debug closure options.  Otherwise nothing.</returns>
        private string getDebug(CompilerOptions options)
        {
            if (options.Debug == true)
                return " --debug";
            return String.Empty;
        }


        private string getIdeMode(CompilerOptions options)
        {
            return " --ideMode=" + options.IdeMode.ToString().ToLower();
        }


        /// <summary>
        /// Figures out the compilation level from the compiler options.
        /// </summary>
        /// <param name="options"></param>
        /// <returns>Returns --compilation_level string value for the closure compiler consumption</returns>
        private string getCompilationLevel(CompilerOptions options)
        {
            switch (options.CompilerLevel)
            {
                case CompilerOptions.ECompilerLevel.WhiteSpaceOnly:
                    {
                        return " --compilation_level=WHITESPACE_ONLY";
                    }
                case CompilerOptions.ECompilerLevel.SimpleOptimization:
                    {
                        return " --compilation_level=SIMPLE_OPTIMIZATIONS";
                    }
                case CompilerOptions.ECompilerLevel.AdvancedOptimization:
                    {
                        return " --compilation_level=ADVANCED_OPTIMIZATIONS";
                    }
                default:
                    {
                        break;
                    }
            }

            return String.Empty;
        }


        private string getWarningLevel(CompilerOptions options)
        {
            switch (options.WarningLevel)
            {
                case CompilerOptions.EWarningLevel.Quiet:
                    {
                        return " --warning_level=QUIET";
                    }
                case CompilerOptions.EWarningLevel.Verbose:
                    {
                        return " --warning_level=VERBOSE";
                    }
                default:
                    {
                        break;
                    }
            }

            return String.Empty;
        }


        private string getOutputFormat(CompilerOptions options)
        {
            switch (options.OutputFormatting)
            {
                case CompilerOptions.EOutputFormatting.PrettyPrint:
                    {
                        return " --formatting=PRETTY_PRINT";
                    }
                case CompilerOptions.EOutputFormatting.PrintInputDelimeter:
                    {
                        return " --formatting=PRINT_INPUT_DELIMITER";
                    }
                default:
                    {
                        break;
                    }
            }

            return String.Empty;
        }


    }

}

