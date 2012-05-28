using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftGPL.jsCompiler.Core
{

    /// <summary>
    /// Handles the mapping of C# properties to the closure compiler's options.
    /// </summary>
    internal class JNICompilerOptions
    {

        private JNICompilerOptions()
        {
        }


        static internal JNICompilerOptions Instance = new JNICompilerOptions();


        public com.google.javascript.jscomp.CompilerOptions Configure(jsCompiler.Core.CompilerOptions options)
        {
            com.google.javascript.jscomp.CompilerOptions result = new com.google.javascript.jscomp.CompilerOptions();

            configureCompileLevel(options, result);
            configureWarningLevel(options, result);
            configureDiagnosticGroups(options, result);

            result.setIdeMode(options.IdeMode);
            return result;
        }


        /// <summary>
        /// Internal translation for compiler level between exposed setting in C# and java
        /// </summary>
        /// <param name="options1"></param>
        /// <param name="options2"></param>
        private void configureCompileLevel(jsCompiler.Core.CompilerOptions options1, com.google.javascript.jscomp.CompilerOptions options2)
        {
            switch (options1.CompilerLevel)
            {
                case jsCompiler.Core.CompilerOptions.ECompilerLevel.WhiteSpaceOnly:
                    {
                        com.google.javascript.jscomp.CompilationLevel.WHITESPACE_ONLY.setOptionsForCompilationLevel(options2);
                        break;
                    }
                case jsCompiler.Core.CompilerOptions.ECompilerLevel.SimpleOptimization:
                    {
                        com.google.javascript.jscomp.CompilationLevel.SIMPLE_OPTIMIZATIONS.setOptionsForCompilationLevel(options2);
                        break;
                    }
                case jsCompiler.Core.CompilerOptions.ECompilerLevel.AdvancedOptimization:
                    {
                        com.google.javascript.jscomp.CompilationLevel.ADVANCED_OPTIMIZATIONS.setOptionsForCompilationLevel(options2);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }


        /// <summary>
        /// Internal translation for warning level between exposed setting in C# and java
        /// </summary>
        /// <param name="options1"></param>
        /// <param name="options2"></param>
        private void configureWarningLevel(jsCompiler.Core.CompilerOptions options1, com.google.javascript.jscomp.CompilerOptions options2)
        {
            switch (options1.WarningLevel)
            {
                case jsCompiler.Core.CompilerOptions.EWarningLevel.Quiet:
                    {
                        com.google.javascript.jscomp.WarningLevel.QUIET.setOptionsForWarningLevel(options2);
                        break;
                    }
                case jsCompiler.Core.CompilerOptions.EWarningLevel.Verbose:
                    {
                        com.google.javascript.jscomp.WarningLevel.VERBOSE.setOptionsForWarningLevel(options2);
                        break;
                    }
                default:
                    {
                        com.google.javascript.jscomp.WarningLevel.DEFAULT.setOptionsForWarningLevel(options2);
                        break;
                    }
            }
        }


        /// <summary>
        /// Convert jsCompiler options to Google Closure Compiler options.
        /// </summary>
        /// <param name="options1"></param>
        /// <param name="options2"></param>
        private void configureDiagnosticGroups(jsCompiler.Core.CompilerOptions options1, com.google.javascript.jscomp.CompilerOptions options2)
        {
            // Set the compiler checks below tight...
            /*
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.MISSING_PROPERTIES, com.google.javascript.jscomp.CheckLevel.ERROR);
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.UNDEFINED_VARIABLES, com.google.javascript.jscomp.CheckLevel.ERROR);
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.CHECK_TYPES, com.google.javascript.jscomp.CheckLevel.ERROR);
            */

            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.ACCESS_CONTROLS, getCheckLevel(options1.DiagnosticGroup.AccessControl));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.AMBIGUOUS_FUNCTION_DECL, getCheckLevel(options1.DiagnosticGroup.AmbiguousFunctionDeclaration));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.CHECK_REGEXP, getCheckLevel(options1.DiagnosticGroup.CheckRegularExpression));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.CHECK_TYPES, getCheckLevel(options1.DiagnosticGroup.CheckTypes));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.CHECK_USELESS_CODE, getCheckLevel(options1.DiagnosticGroup.UselessCode));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.CHECK_VARIABLES, getCheckLevel(options1.DiagnosticGroup.CheckVariables));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.CONSTANT_PROPERTY, getCheckLevel(options1.DiagnosticGroup.ConstantProperty));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.DEPRECATED, getCheckLevel(options1.DiagnosticGroup.Deprecated));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.DUPLICATE_MESSAGE, getCheckLevel(options1.DiagnosticGroup.DuplicateMessage));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.ES5_STRICT, getCheckLevel(options1.DiagnosticGroup.ES5Strict));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.EXTERNS_VALIDATION, getCheckLevel(options1.DiagnosticGroup.ExternsValidation));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.FILEOVERVIEW_JSDOC, getCheckLevel(options1.DiagnosticGroup.FileOverviewTags));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.GLOBAL_THIS, getCheckLevel(options1.DiagnosticGroup.GlobalThis));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.INTERNET_EXPLORER_CHECKS, getCheckLevel(options1.DiagnosticGroup.InternetExplorerChecks));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.INVALID_CASTS, getCheckLevel(options1.DiagnosticGroup.InvalidCasts));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.MISSING_PROPERTIES, getCheckLevel(options1.DiagnosticGroup.MissingProperties));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.NON_STANDARD_JSDOC, getCheckLevel(options1.DiagnosticGroup.NonStandard_JSDoc));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.STRICT_MODULE_DEP_CHECK, getCheckLevel(options1.DiagnosticGroup.StrictModuleDependencyCheck));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.TYPE_INVALIDATION, getCheckLevel(options1.DiagnosticGroup.TypeInvalidation));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.UNDEFINED_NAMES, getCheckLevel(options1.DiagnosticGroup.UndefinedNames));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.UNDEFINED_VARIABLES, getCheckLevel(options1.DiagnosticGroup.UndefinedVariables));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.UNKNOWN_DEFINES, getCheckLevel(options1.DiagnosticGroup.UnknownDefines));
            options2.setWarningLevel(com.google.javascript.jscomp.DiagnosticGroups.VISIBILITY, getCheckLevel(options1.DiagnosticGroup.Visibility));
        }


        private com.google.javascript.jscomp.CheckLevel getCheckLevel(DiagnosticGroup.Diagnostic diagnostic)
        {
            switch (diagnostic.CheckLevel)
            {
                case ECheckLevel.Off:
                    return com.google.javascript.jscomp.CheckLevel.OFF;
                case ECheckLevel.Warning:
                    return com.google.javascript.jscomp.CheckLevel.WARNING;
                case ECheckLevel.Error:
                    return com.google.javascript.jscomp.CheckLevel.ERROR;
                default:
                    throw new Exception("Check Level option in diagnostic group is unknown");
            }
        }
    }
}

