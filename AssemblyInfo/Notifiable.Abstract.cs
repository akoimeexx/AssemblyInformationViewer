using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace com.akoimeexx.utilities.assemblyinformation {
    public abstract class Notifiable :
    INotifyPropertyChanging, INotifyPropertyChanged {
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        internal void SendPropertyChanging(
            [CallerMemberName]string property = null
        ) {
            PropertyChanging?.Invoke(
                this,
                new PropertyChangingEventArgs(property)
            );
        }
        internal void SendPropertyChanged(
            [CallerMemberName]string property = null
        ) {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(property)
            );
        }
        internal bool Set<T>(
            ref T field, 
            T value, 
            [CallerMemberName] string property = null
        ) {
            bool b = false;
            try {
                SendPropertyChanging(property);
                field = value;
                SendPropertyChanged(property);
            } finally { b = field?.Equals(value) ?? true; }
            return b;
        }
    }
}
