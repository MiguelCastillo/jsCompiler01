using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftGPL.jsCompiler.gcBridge
{
    /// <summary>
    /// Handles the extern options used by the closure compiler.
    /// </summary>
    internal static class ExternFiles
    {
        public static com.google.javascript.jscomp.JSSourceFile[] Configure(jsCompiler.Core.CompilerOptions options)
        {
            // It really sucks that I have to read the default externs out of the
            // google closure commandlinecompiler to then feed it right back in
            // to Java...  Waste of cycles.  Hoepfully, these default externs will
            // become a flag in the compiler itself.
            java.util.List files = com.google.javascript.jscomp.CommandLineRunner.getDefaultExterns();

            // Copy over all the files so that we can create an array from them,
            // which is then passed to the closure compiler.
            List<com.google.javascript.jscomp.JSSourceFile> result = new List<com.google.javascript.jscomp.JSSourceFile>(files.size());

            for (int index = 0, length = files.size(); index < length; index++)
            {
                com.google.javascript.jscomp.SourceFile f = files.get(index) as com.google.javascript.jscomp.SourceFile;

                //
                // NOTE: Google Closure Compiler source defines JSSourceFile constructor
                // as private.  But for this to work proeprly, I had to change their
                // code to make the construtor public...  Look at JSSourceFile.java
                //
                result.Add(new com.google.javascript.jscomp.JSSourceFile(f));
            }

            return result.ToArray();
        }
    }
}
