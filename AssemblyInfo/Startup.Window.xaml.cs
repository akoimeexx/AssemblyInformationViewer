using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace com.akoimeexx.utilities.assemblyinformation {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Startup : Window, INotifyPropertyChanged {
#region Properties
        public ViewModels.StartupViewModel ViewModel {
            get { return _viewModel; }
            set {
                _viewModel = value;
                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("ViewModel")
                );
            }
        } private ViewModels.StartupViewModel _viewModel = 
            new ViewModels.StartupViewModel();
        #endregion Properties
    }
    public partial class Startup {
#region Events
        public event PropertyChangedEventHandler PropertyChanged;
#endregion Events
    }
    public partial class Startup {
        public Startup() {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
