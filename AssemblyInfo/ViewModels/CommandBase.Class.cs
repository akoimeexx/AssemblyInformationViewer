using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace com.akoimeexx.utilities.assemblyinformation.ViewModels {
    public partial class CommandBase : Notifiable, ICommand {
#region Properties
        public virtual Action<object> Callback {
            get { return _callback; }
            internal set { Set(ref _callback, value); }
        } private Action<object> _callback = default(Action<object>);
        public virtual Predicate<object> Validation {
            get { return _validation; }
            internal set {
                Set(ref _validation, value);
            }
        } private Predicate<object> _validation = default(Predicate<object>);
#endregion Properties
    }
    public partial class CommandBase {
#region Events
        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
#endregion Events
    }
    public partial class CommandBase {
#region Methods
        public virtual bool CanExecute(object parameter) {
            return (
                Callback != null && 
                (Validation?.Invoke(parameter) ?? true)
            );
        }
        public virtual void Execute(object parameter) {
            Callback?.Invoke(parameter);
        }
        public virtual void RaiseCanExecuteChanged() {
            CommandManager.InvalidateRequerySuggested();
        }
#endregion Methods
    }
    public partial class CommandBase {
#region Constructors & Destructor
        public CommandBase() : this(default(Action<object>)) { }
        public CommandBase(
            Action<object> callback
        ) : this(default(Predicate<object>), callback) { }
        public CommandBase(
            Predicate<object> validation, 
            Action<object> callback
        ) {
            Validation = validation;
            Callback = callback;
        }
#endregion Constructors & Destructor
    }
}
