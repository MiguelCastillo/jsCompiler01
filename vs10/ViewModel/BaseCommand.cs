using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SoftGPL.vs10.ViewModel
{
    public class BaseCommand : ICommand
    {
        public event EventHandler ExecuteCommand;

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }


        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }


        public void Execute(object parameter)
        {
            if (ExecuteCommand != null)
                ExecuteCommand(this, EventArgs.Empty);
        }

        #endregion
    }
}
