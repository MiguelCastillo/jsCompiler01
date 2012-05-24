using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jsCompiler
{

    sealed public class DiagnosticGroup
    {

        public enum EDiagnosticType
        {
            AccessControl,
            AmbiguousFunctionDeclaration,
            CheckRegularExpression,
            CheckTypes,
            CheckUselessCode,
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
            Visibility
        }

        
        internal DiagnosticGroup()
        {
            AccessControl = get(EDiagnosticType.AccessControl);
            AmbiguousFunctionDeclaration = get(EDiagnosticType.AmbiguousFunctionDeclaration);
            CheckRegularExpression = get(EDiagnosticType.CheckRegularExpression);
            CheckTypes = get(EDiagnosticType.CheckTypes);
            CheckUselessCode = get(EDiagnosticType.CheckUselessCode);
            CheckVariables = get(EDiagnosticType.CheckVariables);
            ConstantProperty = get(EDiagnosticType.ConstantProperty);
            Deprecated = get(EDiagnosticType.Deprecated);
            DuplicateMessage = get(EDiagnosticType.DuplicateMessage);
            ES5Strict = get(EDiagnosticType.ES5Strict);
            ExternsValidation = get(EDiagnosticType.ExternsValidation);
            FileOverviewTags = get(EDiagnosticType.FileOverviewTags);
            GlobalThis = get(EDiagnosticType.GlobalThis);
            InternetExplorerChecks = get(EDiagnosticType.InternetExplorerChecks);
            InvalidCasts = get(EDiagnosticType.InvalidCasts);
            MissingProperties = get(EDiagnosticType.MissingProperties);
            NonStandard_JSDoc = get(EDiagnosticType.NonStandard_JSDoc);
            StrictModuleDependencyCheck = get(EDiagnosticType.StrictModuleDependencyCheck);
            TypeInvalidation = get(EDiagnosticType.TypeInvalidation);
            UndefinedNames = get(EDiagnosticType.UndefinedNames);
            UndefinedVariables = get(EDiagnosticType.UndefinedVariables);
            UnknownDefines = get(EDiagnosticType.UnknownDefines);
            Visibility = get(EDiagnosticType.Visibility);
        }


        public class Diagnostic
        {
            public ECheckLevel CheckLevel
            {
                get;
                set;
            }


            public EDiagnosticType DiagnosticType
            {
                get;
                private set;
            }


            public string Description
            {
                get;
                private set;
            }


            internal Diagnostic(EDiagnosticType diagnosticType)
                : this( diagnosticType, ECheckLevel.Off, String.Empty )
            {
            }

            internal Diagnostic(EDiagnosticType diagnosticType, ECheckLevel checkLevel)
                : this( diagnosticType, checkLevel, String.Empty )
            {
            }

            internal Diagnostic(EDiagnosticType diagnosticType, ECheckLevel checkLevel, string decription)
            {
                DiagnosticType = diagnosticType;
                CheckLevel = checkLevel;
                Description = decription;
            }
        }


        private Diagnostic[] Groups = new Diagnostic[]{
            new Diagnostic(EDiagnosticType.AccessControl),
            new Diagnostic(EDiagnosticType.AmbiguousFunctionDeclaration),
            new Diagnostic(EDiagnosticType.CheckRegularExpression),
            new Diagnostic(EDiagnosticType.CheckTypes, ECheckLevel.Warning),
            new Diagnostic(EDiagnosticType.CheckUselessCode),
            new Diagnostic(EDiagnosticType.CheckVariables),
            new Diagnostic(EDiagnosticType.ConstantProperty),
            new Diagnostic(EDiagnosticType.Deprecated),
            new Diagnostic(EDiagnosticType.DuplicateMessage),
            new Diagnostic(EDiagnosticType.ES5Strict),
            new Diagnostic(EDiagnosticType.ExternsValidation),
            new Diagnostic(EDiagnosticType.FileOverviewTags),
            new Diagnostic(EDiagnosticType.GlobalThis),
            new Diagnostic(EDiagnosticType.InternetExplorerChecks, ECheckLevel.Error),
            new Diagnostic(EDiagnosticType.InvalidCasts),
            new Diagnostic(EDiagnosticType.MissingProperties, ECheckLevel.Warning),
            new Diagnostic(EDiagnosticType.NonStandard_JSDoc),
            new Diagnostic(EDiagnosticType.StrictModuleDependencyCheck),
            new Diagnostic(EDiagnosticType.TypeInvalidation),
            new Diagnostic(EDiagnosticType.UndefinedNames),
            new Diagnostic(EDiagnosticType.UndefinedVariables, ECheckLevel.Error),
            new Diagnostic(EDiagnosticType.UnknownDefines),
            new Diagnostic(EDiagnosticType.Visibility)
        };


        public Diagnostic AccessControl
        {
            get;
            private set;
        }
        public Diagnostic AmbiguousFunctionDeclaration
        {
            get;
            private set;
        }
        public Diagnostic CheckRegularExpression
        {
            get;
            private set;
        }
        public Diagnostic CheckTypes
        {
            get;
            private set;
        }
        public Diagnostic CheckUselessCode
        {
            get;
            private set;
        }
        public Diagnostic CheckVariables
        {
            get;
            private set;
        }
        public Diagnostic ConstantProperty
        {
            get;
            private set;
        }
        public Diagnostic Deprecated
        {
            get;
            private set;
        }
        public Diagnostic DuplicateMessage
        {
            get;
            private set;
        }
        public Diagnostic ES5Strict
        {
            get;
            private set;
        }
        public Diagnostic ExternsValidation
        {
            get;
            private set;
        }
        public Diagnostic FileOverviewTags
        {
            get;
            private set;
        }
        public Diagnostic GlobalThis
        {
            get;
            private set;
        }
        public Diagnostic InternetExplorerChecks
        {
            get;
            private set;
        }
        public Diagnostic InvalidCasts
        {
            get;
            private set;
        }
        public Diagnostic MissingProperties
        {
            get;
            private set;
        }
        public Diagnostic NonStandard_JSDoc
        {
            get;
            private set;
        }
        public Diagnostic StrictModuleDependencyCheck
        {
            get;
            private set;
        }
        public Diagnostic TypeInvalidation
        {
            get;
            private set;
        }
        public Diagnostic UndefinedNames
        {
            get;
            private set;
        }
        public Diagnostic UndefinedVariables
        {
            get;
            private set;
        }
        public Diagnostic UnknownDefines
        {
            get;
            private set;
        }
        public Diagnostic Visibility
        {
            get;
            private set;
        }


        public Diagnostic get(EDiagnosticType diagnosticType)
        {
            return Groups[(int)diagnosticType];
        }

    }

}



