using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace PrimalEditor
{
   [DataContract(IsReference = true)] // allows for references to objects in serialization (save) filee
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged; // fires whenever a property is changed

        protected void OnPropertyChanged(string PropertyName) // fires when collection chagned activated
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
