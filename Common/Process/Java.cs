using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace SoftGPL.Common.Process
{

    /// <summary>
    /// An instance of this will allow you to execute Java commands in its own process
    /// via command line.
    /// </summary>
    /// 
    public class Java
    {

        /// <summary>
        /// Java HOME...
        /// @"C:\Program Files\Java\jre7"
        /// </summary>
        ///
        private static string _Home = null;
        public static string Home
        {
            get
            {
                // If _Home is null, then there hasn't been any attempts at finding
                // a JRE in the system yet... So, we give it a try.
                if (_Home == null)
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\java";
                    List<string> javahome = new List<string>(System.IO.Directory.GetDirectories(path));
                    List<string> jre = javahome.FindAll((a) => { return a.IndexOf("jre", StringComparison.InvariantCultureIgnoreCase) != -1; });
                    List<string> jdk = javahome.FindAll((a) => { return a.IndexOf("jdk", StringComparison.InvariantCultureIgnoreCase) != -1; });

                    int lastVersion = -1;
                    // I need logic to traverse through the jres and jdks to figure out
                    // which one to pick :)  But for now, we will just use the last entry
                    // in jre because that is most likely the newest jre in the list.
                    foreach (string iJre in jre)
                    {
                        int offset = iJre.IndexOf("jre", StringComparison.InvariantCultureIgnoreCase) + 3;
                        int version = Int32.Parse(iJre.Substring(offset));

                        // If the java.exe exists and it is a newer version, then we ste it
                        // as the new java home...
                        if (System.IO.File.Exists(iJre + "\\bin\\java.exe") && lastVersion < version)
                        {
                            lastVersion = version;
                            _Home = iJre;
                        }
                    }
                }

                return _Home;
            }
            set
            {
                _Home = value;
            }
        }
    

        /// <summary>
        /// Cached variable for whether or not the java.exe is available for
        /// execution.  This is always false, unless the Verify method successfully
        /// verifies that java.exe is available.
        /// </summary>
        /// 
        private static bool Exists = false;

        /// <summary>
        /// Safeguard from multiple threads coming into the Verify method trying
        /// to set the Exists flag multiple times...
        /// </summary>
        /// 
        private static Mutex ExistsMutex = new Mutex();


        public Java()
        {
        }


        public void Run(string command)
        {
            Run(command, null, null);
        }


        public void Run(string command, DataReceivedEventHandler dataReceived, DataReceivedEventHandler errorReceived)
        {
            Java.Verify();

            System.Diagnostics.Process process = new System.Diagnostics.Process();

            if ( dataReceived != null )
                process.OutputDataReceived += dataReceived;
            if ( errorReceived != null )
                process.ErrorDataReceived += errorReceived;

            process.StartInfo.FileName = Java.Home + "/bin/java.exe";
            process.StartInfo.Arguments = command;

            // Enable redirection of streams
            process.StartInfo.UseShellExecute = false;

            // UseShellExecute must be set to false for CreateNoWindow to work
            process.StartInfo.CreateNoWindow = true;

            // Enable redirection of the output stream from the process
            process.StartInfo.RedirectStandardOutput = dataReceived != null;
            process.StartInfo.RedirectStandardError = errorReceived != null;


            try
            {
                // Start process execution
                process.Start();

                // One of the streams must be asynch or the process will hang.
                // This because the process will write to stream and wait for
                // a read before it can continue to process more data.
                //
                // Right both are async, which is also fine...
                //
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
            }
            finally
            {
                // We will not allow checking out multiple things at the same time
                // because I have not implemented a way to resolve dependencies,
                // which might cause multiple checkins to try to write to the same
                // directory at the same time.  Hopefully this logic will be added
                // soon...
                process.WaitForExit();
                process.Close();
            }
        }


        /// <summary>
        /// Verify will run a test to determine whether java.exe can be executed
        /// as a process.  This method will also set the value of Exists to cache
        /// success to avoid unnecessary tests once java.exe has been verified.
        /// </summary>
        /// <returns>True is a JRE exists via command line, false otherwise</returns>
        /// 
        public static bool Verify()
        {
            if (Exists == true)
                return Exists;

            lock (ExistsMutex)
            {
                // This second time around is done in cases where there are
                // multiple threads waiting on verification...
                //
                if (Exists == true)
                    return Exists;

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = Java.Home + "/bin/java.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                try
                {
                    process.Start();
                    process.WaitForExit();
                    process.Close();
                    Exists = true;
                }
                catch(Exception)
                {
                    Exists = false;
                }
            }

            return Exists;
        }

    }

}
