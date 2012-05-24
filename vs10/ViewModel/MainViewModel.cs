using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Controls;

namespace SoftGPL.vs10.ViewModel
{

    public class MainViewModel : NotifyPropertyChanged
    {

        private FolderBrowserDialog _FolderBrowserDialog = null;
        private jsCompiler.CompilerOptions _jsOptions = null;


        public MainViewModel()
        {
            _FolderBrowserDialog = new FolderBrowserDialog();
            _jsOptions = new jsCompiler.CompilerOptions();

            CompilerType = jsCompiler.Compiler.ECompilerType.JNI;
            NewDocument = false;

            JavaHomeBrowseCommand = new BaseCommand();
            JavaHomeBrowseCommand.ExecuteCommand += new EventHandler(JavaHomeBrowseCommand_ExecuteCommand);
        }


        void JavaHomeBrowseCommand_ExecuteCommand(object sender, EventArgs e)
        {
            _FolderBrowserDialog.SelectedPath = System.IO.Path.GetFullPath(JavaHome);
            DialogResult result = _FolderBrowserDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
                JavaHome = _FolderBrowserDialog.SelectedPath;
        }


        public BaseCommand JavaHomeBrowseCommand
        {
            get;
            private set;
        }


        public jsCompiler.CompilerOptions jsOptions
        {
            get { return _jsOptions; }
        }


        private Array _ECompilerType = System.Enum.GetValues(typeof(jsCompiler.Compiler.ECompilerType));
        public Array ECompilerType
        {
            get { return _ECompilerType; }
        }


        public Array _ECompilerLevel = System.Enum.GetValues(typeof(jsCompiler.CompilerOptions.ECompilerLevel));
        public Array ECompilerLevel
        {
            get { return _ECompilerLevel; }
        }


        private Array _EOutputFormatting = System.Enum.GetValues(typeof(jsCompiler.CompilerOptions.EOutputFormatting));
        public Array EOutputFormatting
        {
            get { return _EOutputFormatting; }
        }


        private Array _EWarningLevel = System.Enum.GetValues(typeof(jsCompiler.CompilerOptions.EWarningLevel));
        public Array EWarningLevel
        {
            get { return _EWarningLevel; }
        }


        private Array _ECheckLevel = System.Enum.GetValues(typeof(jsCompiler.ECheckLevel));
        public Array ECheckLevel
        {
            get { return _ECheckLevel; }
        }


        private jsCompiler.Compiler.ECompilerType _CompilerType = jsCompiler.Compiler.ECompilerType.JNI;
        public jsCompiler.Compiler.ECompilerType CompilerType
        {
            get { return _CompilerType; }
            set { _CompilerType = value; OnPropertyChanged("CompilerType"); }
        }


        public jsCompiler.CompilerOptions.ECompilerLevel CompilerLevel
        {
            get { return jsOptions.CompilerLevel; }
            set { jsOptions.CompilerLevel = value; }
        }


        public jsCompiler.CompilerOptions.EOutputFormatting OutputFormatting
        {
            get { return jsOptions.OutputFormatting; }
            set { jsOptions.OutputFormatting = value; }
        }


        public jsCompiler.CompilerOptions.EWarningLevel WarningLevel
        {
            get { return jsOptions.WarningLevel; }
            set { jsOptions.WarningLevel = value; }
        }

        
        public bool IdeMode
        {
            get { return jsOptions.IdeMode; }
            set { jsOptions.IdeMode = value; }
        }


        public bool StopOnError
        {
            get { return jsOptions.IdeMode == false; }
            set { jsOptions.IdeMode = value != true; }
        }


        public bool Debug
        {
            get { return jsOptions.Debug; }
            set { jsOptions.Debug = value; }
        }


        public string JavaHome
        {
            get { return jsOptions.JavaHome; }
            set { jsOptions.JavaHome = value; OnPropertyChanged("JavaHome"); }
        }


        public string ExtraArgs
        {
            get { return jsOptions.ExtraArgs; }
            set { jsOptions.ExtraArgs = value; }
        }


        public bool NewDocument
        {
            get;
            set;
        }


        private int _MaxErrors = 1000;
        public int MaxErrors
        {
            get { return _MaxErrors; }
            set { _MaxErrors = value; }
        }


        private int _MaxWarnings = 1000;
        public int MaxWarnings
        {
            get { return _MaxWarnings; }
            set { _MaxWarnings = value; }
        }


        public jsCompiler.DiagnosticGroup DiagnosticGroup
        {
            get { return jsOptions.DiagnosticGroup; }
        }


        /*
        public jsCompiler.DiagnosticGroup.EDiagnosticType this[string name]
        {
            get
            {
                return jsCompiler.DiagnosticGroup.EDiagnosticType.AccessControl;
            }
        }
        */


    }

}

