using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;

namespace SoftGPL.vs10.Controller
{

    /// <summary>
    /// This class helps display in Visual Studio Output window and Status bar
    /// different messages that are relevant to the current status of the compiler.
    /// The compilation process itself is entirely independent. So, this class
    /// only represents the different states that are set and will display the
    /// corresponding messages.
    /// 
    /// Currently, the states are not really being checked to determine if they
    /// are being set in the "proper" sequence.  Not sure this will be needed yet.
    /// </summary>
    /// 
    sealed public class CompilerStatus
    {

        /// <summary>
        /// Valid status
        /// </summary>
        /// 
        public enum EStatus
        {
            Starting,
            Started,
            Stopped,
            LoadingFiles,
            Ready
        }


        /// <summary>
        /// EventBus instance
        /// </summary>
        /// 
        private EventBus _EventBus = null;

        /// <summary>
        /// Instance of the IDE
        /// </summary>
        /// 
        private EnvDTE80.DTE2 _DTE = null;

        /// <summary>
        /// Keeps track of when the status is set to Started.  This is to
        /// properly calculate how much time has elapsed up until the status
        /// is set to Stopped
        /// </summary>
        /// 
        private long _StartingTick = 0;

        /// <summary>
        /// Output Window instance.
        /// Reference: http://msdn.microsoft.com/en-us/library/envdte80.toolwindows.outputwindow.aspx
        /// </summary>
        /// 
        private OutputWindowPane _OutputWindow = null;

        /// <summary>
        /// Status Bar instance.
        /// Reference: http://msdn.microsoft.com/en-us/library/envdte.statusbar.progress.aspx
        /// </summary>
        /// 
        private StatusBar _StatusBar = null;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dte">Instance of DTE to get the Output Window and Status Bar from</param>
        /// 
        public CompilerStatus(EventBus eventBus)
        {
            _EventBus = eventBus;
            _DTE = _EventBus.DTE;
            _OutputWindow = _DTE.ToolWindows.OutputWindow.OutputWindowPanes.Item("Build");
            _StatusBar = _DTE.StatusBar;
        }


        /// <summary>
        /// Current status being tracked
        /// </summary>
        /// 
        private EStatus _Status = EStatus.Ready;
        public EStatus Status
        {
            get { return _Status; }
            set { UpdateStatus(value); }
        }


        private void UpdateStatus(EStatus status)
        {
            // Set the status before anything else to keep the running thread that checks
            // for status from stepping on any logic while the status is being set.
            _Status = status;

            switch (status)
            {
                case EStatus.Starting:
                    {
                        Message("Starting");
                        break;
                    }
                case EStatus.LoadingFiles:
                    {
                        Message("Reading js files");
                        _StatusBar.Progress(true, "jsCompiler: Reading js files", 20, 100);
                        _StatusBar.Animate(true, vsStatusAnimation.vsStatusAnimationSync);
                        break;
                    }
                case EStatus.Started:
                    {
                        _StartingTick = DateTime.Now.Ticks;
                        Message("Build Progress");
                        _StatusBar.Animate(false, vsStatusAnimation.vsStatusAnimationSync);
                        _StatusBar.Animate(true, vsStatusAnimation.vsStatusAnimationBuild);

                        // Start up a thread that keeps on spinning until the status is no longer
                        // started.  This important because it keeps Visual Studio in sync with
                        // the current status of the compiler
                        System.Threading.ThreadPool.QueueUserWorkItem((a) =>
                        {
                            while (_Status == EStatus.Started)
                            {
                                _StatusBar.Progress(true, "jsCompiler: Build Progress", 60, 100);

                                // Sleep 5 seconds, and check again
                                System.Threading.Thread.Sleep(5000);
                            }
                        });

                        break;
                    }
                case EStatus.Stopped:
                    {
                        _DTE.StatusBar.Progress(false, "Done", 100, 100);
                        _DTE.StatusBar.Animate(false, vsStatusAnimation.vsStatusAnimationBuild);
                        long endCompileTime = DateTime.Now.Ticks;
                        TimeSpan compileTime = new System.TimeSpan(endCompileTime - _StartingTick);

                        _OutputWindow.Activate();
                        Message(String.Format("Build Completed. Build time: {0}", compileTime.ToString("hh\\:mm\\:ss\\.ff")));
                        break;
                    }
                case EStatus.Ready:
                    {
                        break;
                    }
            }
        }


        /// <summary>
        /// Writes the string msg to the Outout Window in Visual Studio
        /// </summary>
        /// <param name="msg">String to be written to the Output Window in Visual Studio</param>
        /// 
        public void Message(string msg)
        {
            _OutputWindow.OutputString("jsCompiler: " + msg + "\n");
        }

    }

}

