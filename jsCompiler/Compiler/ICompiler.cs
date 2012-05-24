using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jsCompiler
{

    public interface ICompiler
    {
        Result Compile(CompilerOptions settings);
        string Version();
    }

}
