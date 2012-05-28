using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using EnvDTE;

namespace SoftGPL.vs10
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    // Microsoft.VisualStudio.VSConstants.UICONTEXT_NoSolution
    [ProvideAutoLoad("ADFC4E64-0397-11D1-9F4E-00A0C911004F")]
    // Microsoft.VisualStudio.VSConstants.UICONTEXT_SolutionExistsAndFullyLoaded
    [ProvideAutoLoad("10534154-102D-46E2-ABA8-A6BFA25BA0BE")]
    // Microsoft.VisualStudio.VSConstants.UICONTEXT_HasSolution
    [ProvideAutoLoad("f1536ef8-92ec-443c-9ed7-fdadf150da82")]
    // This attribute registers a tool window exposed by this package.
    [ProvideToolWindow(typeof(SoftGPL.vs10.UI.MyToolWindow))]
    [Guid(GuidList.guidvs10PkgString)]
    public sealed class vs10Package : Package
    {
        /// <summary>
        /// EventBus controls the interaction with jsCompiler by:
        /// 1. Providing an interface for interaction with the jsCompiler
        /// 2. Updating the UI
        /// 3. Tie together UI component interaction with the jsCompiler
        /// </summary>
        private Controller.EventBus _EventBus = null;


        /// <summary>
        /// Local reference to the Visual Studio Solution event handler. This is a common approach
        /// to keep Solution event handler instances from getting garbage collected.
        /// </summary>
        /// 
        private DTEEvents _DTEEvents = null;


        /// <summary>
        /// MyToolWindow options
        /// </summary>
        private SoftGPL.vs10.UI.MyToolWindow _MyToolWindow = null;
        private SoftGPL.vs10.UI.MyToolWindow MyToolWindow
        {
            get
            {
                if (_MyToolWindow == null)
                {
                    // Get the instance number 0 of this tool window. This window is single instance so this instance
                    // is actually the only one.
                    // The last flag is set to true so that if the tool window does not exists it will be created.
                    _MyToolWindow = this.FindToolWindow(typeof(SoftGPL.vs10.UI.MyToolWindow), 0, true) as SoftGPL.vs10.UI.MyToolWindow;
                    if ((null == _MyToolWindow) || (null == _MyToolWindow.Frame))
                    {
                        throw new NotSupportedException(Resources.CanNotCreateWindow);
                    }
                }

                return _MyToolWindow;
            }
        }


        /// <summary>
        /// Main window view mode object
        /// </summary>
        private ViewModel.MainViewModel MainViewModel
        {
            get
            {
                return MyToolWindow.MainViewModel;
            }
        }


        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public vs10Package()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        /// <summary>
        /// This function is called when the user clicks the menu item that shows the 
        /// tool window. See the Initialize method to see how the menu item is associated to 
        /// this function using the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            IVsWindowFrame windowFrame = (IVsWindowFrame)MyToolWindow.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }


        /////////////////////////////////////////////////////////////////////////////
        // Overriden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(GuidList.guidvs10CmdSet, (int)PkgCmdIDList.cmdidjsCompiler);
                MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
                mcs.AddCommand(menuItem);
                // Create the command for the tool window
                CommandID toolwndCommandID = new CommandID(GuidList.guidvs10CmdSet, (int)PkgCmdIDList.cmdidjsCompilerTool);
                MenuCommand menuToolWin = new MenuCommand(ShowToolWindow, toolwndCommandID);
                mcs.AddCommand(menuToolWin);
            }

            EnvDTE80.DTE2 dte = Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
            _DTEEvents = dte.Events.DTEEvents;
            _DTEEvents.OnStartupComplete += new _dispDTEEvents_OnStartupCompleteEventHandler(DTEEvents_OnStartupComplete);
        }

        #endregion

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            ShowToolWindow(sender, e);
        }


        void DTEEvents_OnStartupComplete()
        {
            if (_EventBus == null)
            {
                // We have to create the event bus once the vspackage is fully loaded in order to
                // properly get an instance of the view model which is created by the options window...
                // This options window cannot be created once the IDE is loaded.
                _EventBus = new Controller.EventBus(this, MainViewModel);
            }
        }

    }
}
