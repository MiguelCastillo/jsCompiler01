// Guids.cs
// MUST match guids.h
using System;

namespace SoftGPL.vs10
{
    static class GuidList
    {
        public const string guidvs10PkgString = "85CB7DD9-A648-46DD-BD61-110FCC6A9778";
        public const string guidvs10CmdSetString = "4d6452fe-496e-46e7-ad50-375a996667d8";
        public const string guidToolWindowPersistanceString = "18cce485-38b6-4bcd-9e7f-c760c9716f45";

		public static readonly Guid guidvs10Pkg = new Guid(guidvs10PkgString);
        public static readonly Guid guidvs10CmdSet = new Guid(guidvs10CmdSetString);
    };
}