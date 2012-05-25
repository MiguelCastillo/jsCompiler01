using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell;
using System.IO;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;


namespace SoftGPL.vs10.Controller
{

    /// <summary>
    /// Contains all relevant components that handle different events.  E.g. Compiler, error
    /// window, active document...  This is the common place where all these components can
    /// get functionality and access to properties in a uniform way.
    /// </summary>
    /// 
    sealed public class EventBus
    {

        /// <summary>
        /// Local reference to the Visual Studio Solution event handler. This is a common approach
        /// to keep Solution event handler instances from getting garbage collected.
        /// </summary>
        /// 
        private SolutionEvents _SolutionEvents = null;


        /// <summary>
        /// Instance of the VS10 package.  Use this object to communicate with the instance
        /// of Visual Studio hosting this add in.
        /// </summary>
        /// 
        public Package Package
        {
            get;
            private set;
        }


        /// <summary>
        /// Instance of the IDE
        /// </summary>
        /// 
        public EnvDTE80.DTE2 DTE
        {
            get;
            private set;
        }


        public ViewModel.MainViewModel MainViewModel
        {
            get;
            private set;
        }


        /// <summary>
        /// Error list controller keeps the error window and the google closure compiler
        /// errors and warnings in sync.
        /// </summary>
        /// 
        private ErrorList _ErrorList = null;
        public ErrorList ErrorList
        {
            get
            {
                if (_ErrorList == null)
                {
                    _ErrorList = new ErrorList(this);
                }

                return _ErrorList;
            }
        }


        /// <summary>
        /// Instance of the compiler controller, which delegates into the jsCompiler and
        /// relevant part of the UI via the EventBus itself.
        /// </summary>
        /// 
        private Compiler _Compiler = null;
        public Compiler Compiler
        {
            get
            {
                return getCompiler();
            }
        }


        /// <summary>
        /// Compiler status instance to display in Visual Studio the different states in
        /// which in the compilation process is at.
        /// </summary>
        /// 
        private CompilerStatus _CompilerStatus = null;
        public CompilerStatus CompilerStatus
        {
            get
            {
                if (_CompilerStatus == null)
                {
                    _CompilerStatus = new CompilerStatus(this);
                }

                return _CompilerStatus;
            }
        }


        /// <summary>
        /// Helper for managing resources in the solution
        /// </summary>
        /// 
        private Helper.DTE _DTEHelper = null;
        public Helper.DTE DTEHelper
        {
            get
            {
                if (_DTEHelper == null)
                {
                    _DTEHelper = new Helper.DTE(DTE);
                }

                return _DTEHelper;
            }
        }


        /// <summary>
        /// Active document handler to enabled interaction with the current active document.
        /// </summary>
        /// 
        private Document _ActiveDocument = null;
        public Document Document
        {
            get
            {
                if (_ActiveDocument == null)
                {
                    _ActiveDocument = new Document(this);
                }

                return _ActiveDocument;
            }
        }

        

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="package">VSPackage instance to get all the different Visual Studio
        /// component references from</param>
        /// 
        public EventBus(Package package, ViewModel.MainViewModel mainViewMode)
        {
            Package = package;
            MainViewModel = mainViewMode;

            // Setup the DTE helper
            DTE = Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;

            _SolutionEvents = DTE.Events.SolutionEvents;
            _SolutionEvents.Opened += new _dispSolutionEvents_OpenedEventHandler(SolutionEvents_Opened);
            _SolutionEvents.BeforeClosing += new _dispSolutionEvents_BeforeClosingEventHandler(SolutionEvents_BeforeClosing);
        }


        /// <summary>
        /// Callback called whenever a solution is opened.
        /// 
        /// NOTE: This will most likely make it into its own controller when more
        /// functionality is needed from it.
        /// </summary>
        /// 
        private void SolutionEvents_Opened()
        {
            // We are going to be loading up JS files here once and keep track 
            // of when js files are added or removed...  This way jsCompiler
            // does not have to process the projects every time

            // Initialize the compiler.
            getCompiler();
            LoadSettings();
        }


        private void SolutionEvents_BeforeClosing()
        {
            SaveSettings();
        }


        private void LoadSettings()
        {
            string fileName = DTE.Solution.FullName + ".jsCompiler";
            if (System.IO.File.Exists(fileName) == true)
            {
                try
                {
                    using (System.IO.StreamReader file = new System.IO.StreamReader(fileName))
                    {
                        SoftGPL.Common.Serializer<SoftGPL.vs10.ViewModel.MainViewModel> serializer1 = new SoftGPL.Common.Serializer<SoftGPL.vs10.ViewModel.MainViewModel>();
                        ViewModel.MainViewModel mainViewMode = serializer1.Deserialize(file);
                        MainViewModel.Update( mainViewMode );
                    }
                }
                catch (Exception)
                {
                    System.IO.File.Delete(fileName);
                }
            }
        }


        private void SaveSettings()
        {
            string fileName = DTE.Solution.FullName + ".jsCompiler";
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
                {
                    SoftGPL.Common.Serializer<SoftGPL.vs10.ViewModel.MainViewModel> serializer1 = new SoftGPL.Common.Serializer<SoftGPL.vs10.ViewModel.MainViewModel>();
                    string content = serializer1.Serialize(MainViewModel);
                    file.Write(content);
                }
            }
            catch (Exception)
            {
                System.IO.File.Delete(fileName);
            }
        }


        /// <summary>
        /// Get an instance of the compiler controller.  If one does not exist, then it
        /// creates it.
        /// </summary>
        /// <returns>Instance of the compiler controller</returns>
        /// 
        private Compiler getCompiler()
        {
            if (_Compiler == null)
            {
                try
                {
                    _Compiler = new Compiler(this);
                }
                catch (Exception ex)
                {
                    ErrorList.HandleException(ex);
                }
            }

            return _Compiler;
        }

    }

}

