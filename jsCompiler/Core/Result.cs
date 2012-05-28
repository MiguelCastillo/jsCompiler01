﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftGPL.jsCompiler.Core
{

    public class Result : IDisposable
    {
        private List<Error> _Errors = null;
        private List<Error> _Warnings = null;
        private String      _Output = String.Empty;
        private CompilerOptions     _Options = null;


        private Result(CompilerOptions options)
        {
            _Errors = new List<Error>();
            _Warnings = new List<Error>();
            _Options = options;
        }


        public void Dispose()
        {
        }


        #region Public configurable options

        /// <summary>
        /// Return is the compilation was successful
        /// </summary>
        public bool Success
        {
            get { return Errors.Count == 0; }
        }


        /// <summary>
        /// List of errors returned by the compiler
        /// </summary>
        public List<Error> Errors
        {
            get { return _Errors; }
        }


        /// <summary>
        /// List of warnings returned by the internal closure compiler
        /// </summary>
        public List<Error> Warnings
        {
            get { return _Warnings; }
        }


        /// <summary>
        /// Compiled JavaScript generated by the internal closure compiler
        /// </summary>
        public string Output
        {
            get { return _Output; }
            internal set { _Output = value; }
        }


        /// <summary>
        /// CompilerOptions there were passed in to the internal closure compiler.
        /// Notice that if only a list of files were provided to the gcCompiler,
        /// an CompilerOptions object is still created internally.  This internal
        /// CompilerOptions object is what this getter will return.
        /// </summary>
        public CompilerOptions Options
        {
            get { return _Options; }
            internal set { _Options = value; }
        }

        #endregion


        #region Helper methods used for processing the result from the compiler.

        internal static Result fromJNI(com.google.javascript.jscomp.Compiler compiler, com.google.javascript.jscomp.Result gcResult, CompilerOptions options)
        {
            Result result = new Result(options);

            result.Output = compiler.toSource();

            com.google.javascript.jscomp.JSError[] errors = compiler.getErrors();
            for (int index = 0, length = errors.Length; index < length; index++)
            {
                com.google.javascript.jscomp.JSError error = errors[index];
                result.Errors.Add(new Error(error.sourceName, error.description, error.getLineNumber(), error.getCharno(), Error.ESeverity.High));
            }

            com.google.javascript.jscomp.JSError[] warnings = compiler.getWarnings();
            for (int index = 0, length = warnings.Length; index < length; index++)
            {
                com.google.javascript.jscomp.JSError warning = warnings[index];
                result.Warnings.Add(new Error(warning.sourceName, warning.description, warning.getLineNumber(), warning.getCharno(), Error.ESeverity.Medium));
            }

            return result;
        }


        internal static Result fromCMD(CMDCompiler compiler, CompilerOptions options)
        {
            Result result = new Result(options);
            result.Output = compiler.OutputScript;
            result.Errors.AddRange(compiler.getErrors());
            result.Warnings.AddRange(compiler.getWarnings());
            return result;
        }

        #endregion

    }
}
