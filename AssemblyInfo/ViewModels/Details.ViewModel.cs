using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace com.akoimeexx.utilities.assemblyinformation.ViewModels {
    public partial class DetailsViewModel : Notifiable {
#region Properties
        public ReflectionAnalyzer.AssemblyAnalysis AssemblyDetails {
            get { return _assemblyDetails; }
            set { Set(ref _assemblyDetails, value); }
        } private ReflectionAnalyzer.AssemblyAnalysis _assemblyDetails =
            default(ReflectionAnalyzer.AssemblyAnalysis);
#endregion Properties
    }
    public partial class DetailsViewModel {
#region Commands
        public ICommand CloseWindow {
            get {
                return
                    _closeWindow ??
                    (_closeWindow = new CommandBase(
                        a => {
                            if ((a as Window) != null) {
                                ((Window)a).Close();
                            }
                        }
                    ));
            }
            private set { Set(ref _closeWindow, value); }
        } private ICommand _closeWindow = default(ICommand);
#endregion Commands
    }
}
