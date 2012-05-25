using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftGPL.jsCompiler
{

    /// <summary>
    /// Diagnostic group is a set of flags that google closure uses for coniguring
    /// different processing stragies.
    /// 
    /// Reference: http://code.google.com/p/closure-compiler/wiki/Warnings
    /// 
    /// NOTE: Everything in here is public because I need this class to be fully
    /// serialazable... I really don't want to duplicate all these Diagnostics just
    /// to give them public access.  These diagnostics also need public setters and
    /// getters for UI bindings to properly access them.
    /// </summary>
    /// 
    sealed public class DiagnosticGroup
    {

        public enum EDiagnosticType
        {
            AccessControl,
            AmbiguousFunctionDeclaration,
            CheckRegularExpression,
            CheckTypes,
            CheckVariables,
            ConstantProperty,
            Deprecated,
            DuplicateMessage,
            ES5Strict,
            ExternsValidation,
            FileOverviewTags,
            GlobalThis,
            InternetExplorerChecks,
            InvalidCasts,
            MissingProperties,
            NonStandard_JSDoc,
            StrictModuleDependencyCheck,
            TypeInvalidation,
            UndefinedNames,
            UndefinedVariables,
            UnknownDefines,
            UselessCode,
            Visibility
        }


        public DiagnosticGroup()
        {
            AccessControl = new Diagnostic(EDiagnosticType.AccessControl);
            AmbiguousFunctionDeclaration = new Diagnostic(EDiagnosticType.AmbiguousFunctionDeclaration, ECheckLevel.Warning);
            CheckRegularExpression = new Diagnostic(EDiagnosticType.CheckRegularExpression);
            CheckTypes = new Diagnostic(EDiagnosticType.CheckTypes);
            CheckVariables = new Diagnostic(EDiagnosticType.CheckVariables);
            ConstantProperty = new Diagnostic(EDiagnosticType.ConstantProperty);
            Deprecated = new Diagnostic(EDiagnosticType.Deprecated);
            DuplicateMessage = new Diagnostic(EDiagnosticType.DuplicateMessage);
            ES5Strict = new Diagnostic(EDiagnosticType.ES5Strict, ECheckLevel.Warning);
            ExternsValidation = new Diagnostic(EDiagnosticType.ExternsValidation, ECheckLevel.Warning);
            FileOverviewTags = new Diagnostic(EDiagnosticType.FileOverviewTags, ECheckLevel.Warning);
            GlobalThis = new Diagnostic(EDiagnosticType.GlobalThis);
            InternetExplorerChecks = new Diagnostic(EDiagnosticType.InternetExplorerChecks, ECheckLevel.Error);
            InvalidCasts = new Diagnostic(EDiagnosticType.InvalidCasts);
            MissingProperties = new Diagnostic(EDiagnosticType.MissingProperties);
            NonStandard_JSDoc = new Diagnostic(EDiagnosticType.NonStandard_JSDoc, ECheckLevel.Warning);
            StrictModuleDependencyCheck = new Diagnostic(EDiagnosticType.StrictModuleDependencyCheck);
            TypeInvalidation = new Diagnostic(EDiagnosticType.TypeInvalidation);
            UndefinedNames = new Diagnostic(EDiagnosticType.UndefinedNames);
            UndefinedVariables = new Diagnostic(EDiagnosticType.UndefinedVariables);
            UnknownDefines = new Diagnostic(EDiagnosticType.UnknownDefines, ECheckLevel.Warning);
            UselessCode = new Diagnostic(EDiagnosticType.UselessCode, ECheckLevel.Warning);
            Visibility = new Diagnostic(EDiagnosticType.Visibility);
        }


        public class Diagnostic
        {
            public EDiagnosticType DiagnosticType
            {
                get;
                set;
            }


            public ECheckLevel CheckLevel
            {
                get;
                set;
            }


            public string Description
            {
                get;
                set;
            }


            public Diagnostic()
            {
            }

            public Diagnostic(EDiagnosticType diagnosticType)
                : this( diagnosticType, ECheckLevel.Off, String.Empty )
            {
            }

            public Diagnostic(EDiagnosticType diagnosticType, ECheckLevel checkLevel)
                : this( diagnosticType, checkLevel, String.Empty )
            {
            }

            public Diagnostic(EDiagnosticType diagnosticType, ECheckLevel checkLevel, string decription)
            {
                DiagnosticType = diagnosticType;
                CheckLevel = checkLevel;
                Description = decription;
            }
        }



        public Diagnostic AccessControl
        {
            get;
            set;
        }
        public Diagnostic AmbiguousFunctionDeclaration
        {
            get;
            set;
        }
        public Diagnostic CheckRegularExpression
        {
            get;
            set;
        }
        public Diagnostic CheckTypes
        {
            get;
            set;
        }
        public Diagnostic CheckVariables
        {
            get;
            set;
        }
        public Diagnostic ConstantProperty
        {
            get;
            set;
        }
        public Diagnostic Deprecated
        {
            get;
            set;
        }
        public Diagnostic DuplicateMessage
        {
            get;
            set;
        }
        public Diagnostic ES5Strict
        {
            get;
            set;
        }
        public Diagnostic ExternsValidation
        {
            get;
            set;
        }
        public Diagnostic FileOverviewTags
        {
            get;
            set;
        }
        public Diagnostic GlobalThis
        {
            get;
            set;
        }
        public Diagnostic InternetExplorerChecks
        {
            get;
            set;
        }
        public Diagnostic InvalidCasts
        {
            get;
            set;
        }
        public Diagnostic MissingProperties
        {
            get;
            set;
        }
        public Diagnostic NonStandard_JSDoc
        {
            get;
            set;
        }
        public Diagnostic StrictModuleDependencyCheck
        {
            get;
            set;
        }
        public Diagnostic TypeInvalidation
        {
            get;
            set;
        }
        public Diagnostic UndefinedNames
        {
            get;
            set;
        }
        public Diagnostic UndefinedVariables
        {
            get;
            set;
        }
        public Diagnostic UnknownDefines
        {
            get;
            set;
        }
        public Diagnostic UselessCode
        {
            get;
            set;
        }
        public Diagnostic Visibility
        {
            get;
            set;
        }

    }

}



