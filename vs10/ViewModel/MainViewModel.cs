using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace SoftGPL.vs10.ViewModel
{

    [XmlRoot(ElementName = "Settings")]
    public class MainViewModel : SoftGPL.Common.NotifyPropertyChanged
    {

        private FolderBrowserDialog _FolderBrowserDialog = null;


        public MainViewModel()
        {
            _FolderBrowserDialog = new FolderBrowserDialog();

            CompilerType = jsCompiler.Compiler.ECompilerType.JNI;
            NewDocument = false;

            JavaHomeBrowseCommand = new BaseCommand();
            JavaHomeBrowseCommand.ExecuteCommand += new EventHandler(JavaHomeBrowseCommand_ExecuteCommand);
        }


        public void Update(MainViewModel mainViewModel)
        {
            CompilerType = mainViewModel.CompilerType;
            MaxErrors = mainViewModel.MaxErrors;
            MaxWarnings = mainViewModel.MaxWarnings;
            JavaHome = mainViewModel.JavaHome;
            NewDocument = mainViewModel.NewDocument;
            StopOnError = mainViewModel.StopOnError;
            jsOptions = mainViewModel.jsOptions;
        }


        private void JavaHomeBrowseCommand_ExecuteCommand(object sender, EventArgs e)
        {
            _FolderBrowserDialog.SelectedPath = System.IO.Path.GetFullPath(JavaHome);
            DialogResult result = _FolderBrowserDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
                JavaHome = _FolderBrowserDialog.SelectedPath;
        }


        [XmlIgnore]
        public BaseCommand JavaHomeBrowseCommand
        {
            get;
            private set;
        }


        private jsCompiler.CompilerOptions _jsOptions = null;
        public jsCompiler.CompilerOptions jsOptions
        {
            get
            {
                if( _jsOptions == null )
                    _jsOptions = new jsCompiler.CompilerOptions();
                return _jsOptions;
            }
            set
            {
                _jsOptions = value;
                OnPropertyChanged("jsOptions");
            }
        }


        private jsCompiler.Compiler.ECompilerType _CompilerType = jsCompiler.Compiler.ECompilerType.JNI;
        public jsCompiler.Compiler.ECompilerType CompilerType
        {
            get { return _CompilerType; }
            set { _CompilerType = value; OnPropertyChanged("CompilerType"); }
        }


        public string JavaHome
        {
            get { return jsCompiler.Java.Home; }
            set { jsCompiler.Java.Home = value; OnPropertyChanged("JavaHome"); }
        }

        public bool StopOnError
        {
            get { return jsOptions.IdeMode == false; }
            set { jsOptions.IdeMode = value != true; OnPropertyChanged("StopOnError"); }
        }


        private bool _NewDocument = false;
        public bool NewDocument
        {
            get { return _NewDocument; }
            set { _NewDocument = value; OnPropertyChanged("NewDocument"); }
        }


        private int _MaxErrors = 1000;
        public int MaxErrors
        {
            get { return _MaxErrors; }
            set { _MaxErrors = value; OnPropertyChanged("MaxErrors"); }
        }


        private int _MaxWarnings = 1000;
        public int MaxWarnings
        {
            get { return _MaxWarnings; }
            set { _MaxWarnings = value; OnPropertyChanged("MaxWarnings"); }
        }


        #region Exposed enumeration types

        private Array _ECompilerType = System.Enum.GetValues(typeof(jsCompiler.Compiler.ECompilerType));
        [XmlIgnore]
        public Array ECompilerType
        {
            get { return _ECompilerType; }
        }


        private Array _ECompilerLevel = System.Enum.GetValues(typeof(jsCompiler.CompilerOptions.ECompilerLevel));
        [XmlIgnore]
        public Array ECompilerLevel
        {
            get { return _ECompilerLevel; }
        }


        private Array _EOutputFormatting = System.Enum.GetValues(typeof(jsCompiler.CompilerOptions.EOutputFormatting));
        [XmlIgnore]
        public Array EOutputFormatting
        {
            get { return _EOutputFormatting; }
        }


        private Array _EWarningLevel = System.Enum.GetValues(typeof(jsCompiler.CompilerOptions.EWarningLevel));
        [XmlIgnore]
        public Array EWarningLevel
        {
            get { return _EWarningLevel; }
        }


        private Array _ECheckLevel = System.Enum.GetValues(typeof(jsCompiler.ECheckLevel));
        [XmlIgnore]
        public Array ECheckLevel
        {
            get { return _ECheckLevel; }
        }

        #endregion

    }

}

