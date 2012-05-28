using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftGPL.jsCompiler.Core
{

    sealed public class Error
    {
        public enum ESeverity
        {
            High,
            Medium,
            Low
        }


        public Error(string file, string description, int line)
            : this(file, description, line, -1, ESeverity.High)
        {
        }


        public Error(string file, string description, int lineno, int charno, ESeverity severity)
        {
            File = file;
            Description = description;
            LineNo = lineno;
            CharNo = charno;
            Line = String.Empty;
        }

        /// <summary>
        /// File with offending code
        /// </summary>
        public string File
        {
            get;
            internal set;
        }

        /// <summary>
        /// Description of the error
        /// </summary>
        public string Description
        {
            get;
            internal set;
        }

        /// <summary>
        /// Offending source code
        /// </summary>
        public string Line
        {
            get;
            internal set;
        }

        /// <summary>
        /// Line number in the file the error was found
        /// </summary>
        public int LineNo
        {
            get;
            internal set;
        }

        /// <summary>
        /// Position in the where the error occured
        /// </summary>
        public int CharNo
        {
            get;
            internal set;
        }

        /// <summary>
        /// Severity of the error
        /// </summary>
        public ESeverity Severity
        {
            get;
            internal set;
        }


    }

}

