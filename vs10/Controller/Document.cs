using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;

namespace SoftGPL.vs10.Controller
{

    /// <summary>
    /// Provides functionality to interact with the active document
    /// </summary>
    /// 
    sealed public class Document
    {
        /// <summary>
        /// Event bus instance
        /// </summary>
        /// 
        private EventBus _EventBus = null;
        private static int _FileCount = 0;


        public Document(EventBus eventBus)
        {
            _EventBus = eventBus;
        }


        /// <summary>
        /// Open up a new document, gives it the name passed in, and sets the content.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="content"></param>
        public void New(string content)
        {
            _EventBus.DTE.ItemOperations.NewFile(@"General\Text File", "New File" + (++_FileCount) + ".js");
            TextSelection textSelection = _EventBus.DTE.ActiveDocument.Selection as TextSelection;
            textSelection.SelectAll();
            textSelection.Text = "";
            textSelection.Insert(content);
        }


        /// <summary>
        /// Function for navigating in the active document to the specified line and column.
        /// Useful for keeping in sync the errors in the list and where they occur in the
        /// active document.
        /// </summary>
        /// <param name="line">Line to move the caret to</param>
        /// <param name="column">Column to move the caret to</param>
        /// 
        public void Navigate(int line, int column)
        {
            //
            // http://msdn.microsoft.com/en-us/library/cwda3d81(v=vs.71).aspx
            //
            if (_EventBus.DTE.ActiveDocument != null)
            {
                TextSelection textSelection = _EventBus.DTE.ActiveDocument.Selection as TextSelection;
                textSelection.MoveToLineAndOffset(line, column);
                textSelection.WordRight(true);
            }
        }
    }
}
