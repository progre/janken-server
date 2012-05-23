using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Progressive.JankenServer.Commons.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged メンバー
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public ViewModelBase()
        {
        }

        protected void NotifyPropertyChanged(string name)
        {
            if (GetType().GetProperty(name) == null)
            {
                throw new ArgumentException();
            }
            if (PropertyChanged == null)
            {
                return;
            }
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
