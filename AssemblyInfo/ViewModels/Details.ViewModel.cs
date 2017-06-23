using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace com.akoimeexx.utilities.assemblyinformation.ViewModels {
    public partial class DetailsViewModel : Notifiable {
#region Properties
        public AssemblyAnalysis AssemblyDetails {
            get { return _assemblyDetails; }
            set {
                SendPropertyChanging("InformationCollection");
                Set(ref _assemblyDetails, value);
                SendPropertyChanged("InformationCollection");
            }
        } private AssemblyAnalysis _assemblyDetails =
            default(AssemblyAnalysis);
        public IList InformationCollection {
            get {
                return new CompositeCollection() {
                    new CollectionContainer() {
                        Collection = AssemblyDetails.Classes
                    },
                    new CollectionContainer() {
                        Collection = AssemblyDetails.Enums
                    },
                    new CollectionContainer() {
                        Collection = AssemblyDetails.Interfaces
                    },
                    new CollectionContainer() {
                        Collection = AssemblyDetails.Structs
                    }
                };
            }
        }
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
