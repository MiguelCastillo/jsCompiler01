using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Reflection;
using System.IO;


namespace SoftGPL.jsCompiler
{

    /// <summary>
    /// CommandLineCompiler is for using the google closure compiler via command line.
    /// Why would you want to do this?  Well, the JNI compiler requires implementation
    /// for the specific features before you can use it. But with the command line you
    /// could use ExtraArgs in the Option object passed to the compiler and you can
    /// do whatever the latest and greatest google closure compiler can do.
    /// I would say this is more because you really need to use a feature that is only
    /// currently available via command line.
    /// </summary>
    internal class CMDCompiler : ICompiler
    {

        private enum ECompileState
        {
            Running,
            ProcessingWarning,
            ProcessingError,
            Finished
        }


        /// <summary>
        /// Java runtime that executes the closure compiler
        /// </summary>
        private SoftGPL.Common.Process.Java Java = null;

        /// <summary>
        /// Flag to keep track if we are processing a compile error or
        /// a warning
        /// </summary>
        private ECompileState CompilerState = ECompileState.Running;

        /// <summary>
        /// String containing
        /// </summary>
        private string _OutputScript = String.Empty;

        /// <summary>
        /// Output contains the compiled script IF there wasn't a file specified.
        /// </summary>
        public string OutputScript
        {
            get { return _OutputScript; }
            private set { _OutputScript = value; }
        }

        /// <summary>
        /// List of compilation errors
        /// </summary>
        private List<Error> Errors = null;

        /// <summary>
        /// List of compilation warngings
        /// </summary>
        private List<Error> Warnings = null;

        /// <summary>
        /// Error messages that are not compiler errors.  E.g. compiler.jar not found.
        /// </summary>
        private List<string> Failures = null;

        /// <summary>
        /// String that holds help information from the command line call into google
        /// closude compiler
        /// </summary>
        private string _Help = String.Empty;

        /// <summary>
        /// String that holds version information from the command line call into google
        /// closure compiler
        /// </summary>
        private string _Version = String.Empty;

        /// <summary>
        /// Static string pointing to the closure compiler.jar
        /// </summary>
        public static string Home = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string Closure = Home + "\\lib\\closure\\compiler.jar";

        
        /// <summary>
        /// Constructor
        /// </summary>
        public CMDCompiler()
        {
            Java = new SoftGPL.Common.Process.Java();
            Errors = new List<Error>();
            Warnings = new List<Error>();
            Failures = new List<string>();
        }


        public Result Compile(CompilerOptions options)
        {
            OutputScript = String.Empty;
            Errors.Clear();
            Warnings.Clear();
            Failures.Clear();

            string command = CMDCompilerOptions.Instance.Configure(options);

            if (command != String.Empty)
            {
                var file = createConfigFile(command);
                CompilerState = ECompileState.Running;
                Java.Run("-jar \"" + CMDCompiler.Closure + "\"" + " --flagfile=\"" + file + "\"", compile_OutputDataReceived, compile_ErrorDataReceived);
                File.Delete(file);
                CompilerState = ECompileState.Finished;
            }

            return Result.fromCMD(this, options);;
        }


        public string Help()
        {
            _Help = "";
            Java.Run("-jar \"" + CMDCompiler.Closure + "\" --help", help_OutputDataReceived, help_ErrorDataReceived);
            return _Help;
        }


        public string Version()
        {
            _Version = "";
            Java.Run("-jar \"" + CMDCompiler.Closure + "\" --version", version_OutputDataReceived, version_ErrorDataReceived);
            return _Version;
        }


        public List<Error> getErrors()
        {
            return Errors;
        }


        public List<Error> getWarnings()
        {
            return Warnings;
        }


        private string createConfigFile(string command)
        {
            string fileName = CMDCompiler.Home + "\\config_" + (DateTime.Now.Ticks).ToString();

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
            {
                file.Write(command);
            }

            return fileName;
        }



        #region Java callbacks


        private void compile_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
                return;
            OutputScript += e.Data;
        }


        private void compile_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
                return;
            ParseError(e.Data);
        }


        private void help_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            PrintToConsole(e);
        }


        private void help_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            PrintToConsole(e);
            if (e.Data != null)
                _Help += e.Data;
        }


        private void version_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            PrintToConsole(e);
        }


        private void version_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            PrintToConsole(e);
            if (e.Data != null)
                _Version += e.Data;
        }


        private void PrintToConsole(DataReceivedEventArgs e)
        {
            if (e.Data == null)
                return;

            System.Console.Out.Write(e.Data);
        }


        /// <summary>
        /// It is public for now because I need it unit tested...  This code is pretty fagile.
        /// I am looking to get XML output rather than parsing the cmd lines one at a time...
        /// </summary>
        /// <param name="error"></param>
        public void ParseError(string error)
        {
            if (error.Length == 0)
            {
                // We are done...  Set the state back to running so that we can detect new errors/warnings.
                CompilerState = ECompileState.Running;
                return;
            }

            int start = 0, end = 0;

            if (CompilerState == ECompileState.Running)
            {
                //
                // Check for errors...
                //
                if ((start = error.IndexOf(": ERROR - ")) != -1)
                {
                    string message = error.Substring(start + ": ERROR - ".Length);

                    end = start - 1; // Skip over ':'
                    start = error.LastIndexOf(':', end);
                    string line = error.Substring(start + 1, end - start);
                    string file = error.Substring(0, start);

                    Error e = new Error(file, message, Int32.Parse(line), -1, Error.ESeverity.High);
                    Errors.Add(e);
                    CompilerState = ECompileState.ProcessingError;
                }
                //
                // Check for warnings...
                //
                else if ((start = error.IndexOf(": WARNING - ")) != -1)
                {
                    string message = error.Substring(start + ": WARNING - ".Length);

                    end = start - 1; // Skip over ':'
                    start = error.LastIndexOf(':', end);
                    string line = error.Substring(start + 1, end - start);
                    string file = error.Substring(0, start);

                    Error e = new Error(file, message, Int32.Parse(line), -1, Error.ESeverity.Low);
                    Warnings.Add(e);
                    CompilerState = ECompileState.ProcessingWarning;
                }
                else if (error.StartsWith(Errors.Count.ToString() + " error(s)") == false)
                {
                    Failures.Add(error);
                }
            }
            //
            // Little check sum that helps us determine if we have the right number of errors as
            // well as when we need to reset the state of the compiler
            //
            else if (error.StartsWith(Errors.Count.ToString() + " error(s)") == true)
            {
                // We are done...  Set the state back to running so that we can detect new errors/warnings.
                CompilerState = ECompileState.Running;
            }
            //
            // Process everything else...
            //
            else
            {
                switch (CompilerState)
                {
                    case ECompileState.ProcessingError:
                        {
                            if (error[error.Length - 1] == '^')
                                Errors.Last().CharNo = error.Length - 1;
                            else
                                Errors.Last().Line += error;
                            break;
                        }
                    case ECompileState.ProcessingWarning:
                        {
                            if (error[error.Length - 1] == '^')
                                Warnings.Last().CharNo = error.Length - 1;
                            else
                                Warnings.Last().Line += error;
                            break;
                        }
                    default:
                        {
                            Failures.Add(error);
                            break;
                        }
                }
            }
        }


        #endregion
    }

}

