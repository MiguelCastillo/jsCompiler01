using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SoftGPL.Common.Events
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
   
        /// <summary>
        /// Event handlers for INotifyPropertyChanged
        /// </summary>
        #region INotifyPropertyChanged EventHandler

        public event PropertyChangedEventHandler PropertyChanged = null;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
