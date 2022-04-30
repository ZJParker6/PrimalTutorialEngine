using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PrimalEditor
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged; // fires whenever a property is changed

        protected void OnPropertyChanged(string PropertyName) // fires when collection chagned activated
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
