using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftGPL.jsCompiler.gcBridge
{

    /// <summary>
    /// Handles the mapping of the input JavaScript file names that will be
    /// processed by the closure compiler.  Here we take a list of strings
    /// and create JSSourceFile objects that then passed over to the compiler.
    /// </summary>
    internal static class SourceFiles
    {
        public static com.google.javascript.jscomp.JSSourceFile[] Configure(jsCompiler.Core.CompilerOptions options)
        {
            List<com.google.javascript.jscomp.JSSourceFile> result = new List<com.google.javascript.jscomp.JSSourceFile>(options.InputFiles.Count);

            foreach (string file in options.InputFiles)
            {
                result.Add(com.google.javascript.jscomp.JSSourceFile.fromFile(file));
            }

            return result.ToArray();
        }
    }
}
