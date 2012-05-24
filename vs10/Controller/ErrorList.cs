using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.TextManager.Interop;


namespace SoftGPL.vs10.Controller
{

    /// <summary>
    /// Provides functionality to interact with the Error List window in Visual Studio
    /// </summary>
    /// 
    sealed public class ErrorList
    {

        /// <summary>
        /// Instance of event bus
        /// </summary>
        /// 
        private EventBus _EventBus = null;

        /// <summary>
        /// Error list provider instance.  This is the actual window that displayed the
        /// errors/warnings
        /// </summary>
        /// 
        private ErrorListProvider _ErrorListProvider = null;


        public ErrorList(EventBus eventBus)
        {
            _EventBus = eventBus;

            // Setup the error list controller, which will update the IDE error list with
            // error coming back from the closure compiler.
            _ErrorListProvider = new Microsoft.VisualStudio.Shell.ErrorListProvider(_EventBus.Package);
            _ErrorListProvider.ProviderGuid = SoftGPL.vs10.GuidList.guidvs10Pkg;
            _ErrorListProvider.ProviderName = "SoftGPL.gcVS10";
        }


        ~ErrorList()
        {
            _ErrorListProvider.Dispose();
        }


        /// <summary>
        /// Clear up all the error in the error list
        /// </summary>
        /// 
        public void Clear()
        {
            _ErrorListProvider.Tasks.Clear();
        }


        #region Exception handlers

        public void HandleException(Exception ex)
        {
            ErrorTask task = new ErrorTask(ex);
            _ErrorListProvider.Tasks.Add(task);
            _ErrorListProvider.BringToFront();
        }

        #endregion


        #region Error handlers


        /// <summary>
        /// Updates the error task list window with all the errors from the closure
        /// compiler.
        /// </summary>
        /// <param name="errors"></param>
        /// 
        public void AddErrors(List<jsCompiler.Error> errors)
        {
            ErrorTask task = null;
            foreach (jsCompiler.Error error in errors)
            {
                task = new ErrorTask();
                task.ErrorCategory = TaskErrorCategory.Error;
                task.Document = error.File;

                // The error list control uses 0 base, so we have to subtract one from
                // the actual values from the compiler
                task.Column = error.CharNo - 1;
                task.Line = error.LineNo - 1;
                task.Text = error.Description;
                task.Priority = TaskPriority.High;

                if ( task.Document != null )
                    task.Navigate += new EventHandler(error_Navigate);

                _ErrorListProvider.Tasks.Add(task);
            }

            _ErrorListProvider.BringToFront();
        }


        /// <summary>
        /// Handles the navigation from the error list item to the document in the
        /// project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void error_Navigate(object sender, EventArgs e)
        {
            Task task = sender as Task;
            if (task == null)
                return;

            Task navigateToTask = new Task()
            {
                Document = task.Document,

                // The document navigation is 1 base, so we have to add the 1 we subtracted
                // for the error list control, which is 0 base.
                Line = task.Line + 1,

                // Weird that the error and warning window have inconsistent definitions
                // for what column offsets mean...
                Column = task.Column + 2
            };

            _ErrorListProvider.Navigate(navigateToTask, new Guid());
            _EventBus.Document.Navigate(navigateToTask.Line, navigateToTask.Column);
        }

        #endregion


        #region Warning Handlers


        /// <summary>
        /// Updates the error task list with the warnings from the closure
        /// compiler
        /// </summary>
        /// <param name="warnings"></param>
        /// 
        public void AddWarnings(List<jsCompiler.Error> warnings)
        {
            ErrorTask task = null;
            foreach (jsCompiler.Error error in warnings)
            {
                task = new ErrorTask()
                {
                    ErrorCategory = TaskErrorCategory.Warning,
                    Document = error.File,

                    // The error list control uses 0 base, so we have to subtract one from
                    // the actual values from the compiler
                    Column = error.CharNo - 1,
                    Line = error.LineNo - 1,
                    Text = error.Description,
                    Priority = TaskPriority.Normal,
                    CanDelete = true
                };

                if (task.Document != null)
                    task.Navigate += new EventHandler(warning_Navigate);

                _ErrorListProvider.Tasks.Add(task);
            }

            _ErrorListProvider.BringToFront();
        }


        /// <summary>
        /// Handles the navigation from the error list item to the document in the
        /// project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void warning_Navigate(object sender, EventArgs e)
        {
            Task task = sender as Task;
            if (task == null)
                return;

            Task navigateToTask = new Task()
            {
                Document = task.Document,

                // The document navigation is 1 base, so we have to add the 1 we subtracted
                // for the error list control, which is 0 base.
                Line = task.Line + 1,

                // Weird that the error and warning window have inconsistent definitions
                // for what column offsets mean...
                Column = task.Column + 2
            };

            _ErrorListProvider.Navigate(navigateToTask, new Guid());
            _EventBus.Document.Navigate(navigateToTask.Line, navigateToTask.Column);
        }

        #endregion


        #region Generic Failure Handlers


        /// <summary>
        /// Handles all other failures that are not generated by the closure compiler.
        /// Errors such as system errors should be treated through this method.
        /// </summary>
        /// <param name="failures"></param>
        /// 
        public void AddFailures(List<string> failures)
        {
            ErrorTask task = null;
            foreach (string error in failures)
            {
                task = new ErrorTask()
                {
                    ErrorCategory = TaskErrorCategory.Error,
                    Text = error,
                    Priority = TaskPriority.High
                };

                _ErrorListProvider.Tasks.Add(task);
            }

            _ErrorListProvider.BringToFront();
        }

        #endregion


        #region Interesting approach for naviating to a document in a project. Not in use
        /// <summary>
        /// This is copied from http://social.msdn.microsoft.com/Forums/en-US/vsx/thread/81c2959c-a21a-4baa-88b2-757ce0769532
        /// Thank you Darren!!
        /// The good thing is that VS2010 supports most of this already...  Well, in one
        /// call you get to navigate to the document and the cursor is placed at that
        /// beginning of the line rather than where the error occured.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arguments"></param>
        /// 
        private void NavigateHandler(object sender, EventArgs arguments)
        {
            Microsoft.VisualStudio.Shell.Task task = sender as Microsoft.VisualStudio.Shell.Task;

            if (task == null)
            {
                throw new ArgumentException("sender parm cannot be null");
            }

            if (String.IsNullOrEmpty(task.Document))
            {
                return;
            }

            IVsUIShellOpenDocument openDoc = Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(IVsUIShellOpenDocument)) as IVsUIShellOpenDocument;

            if (openDoc == null)
            {
                return;
            }

            IVsWindowFrame frame;
            Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider;
            IVsUIHierarchy hierarchy;
            uint itemId;
            Guid logicalView = VSConstants.LOGVIEWID_Code;

            if (ErrorHandler.Failed(openDoc.OpenDocumentViaProject(
                task.Document, ref logicalView, out serviceProvider, out hierarchy, out itemId, out frame))
                || frame == null)
            {
                return;
            }

            object docData;
            frame.GetProperty((int)__VSFPROPID.VSFPROPID_DocData, out docData);

            VsTextBuffer buffer = docData as VsTextBuffer;
            if (buffer == null)
            {
                IVsTextBufferProvider bufferProvider = docData as IVsTextBufferProvider;
                if (bufferProvider != null)
                {
                    IVsTextLines lines;
                    ErrorHandler.ThrowOnFailure(bufferProvider.GetTextBuffer(out lines));
                    buffer = lines as VsTextBuffer;

                    if (buffer == null)
                    {
                        return;
                    }
                }
            }

            IVsTextManager mgr = Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(VsTextManagerClass)) as IVsTextManager;
            if (mgr == null)
            {
                return;
            }

            mgr.NavigateToLineAndColumn(buffer, ref logicalView, task.Line, task.Column + 1, task.Line, task.Column + 1);
        }
        #endregion

    }
}
