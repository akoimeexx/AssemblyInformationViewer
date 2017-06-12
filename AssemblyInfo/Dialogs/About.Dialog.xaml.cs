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
using System.Windows.Shapes;

namespace com.akoimeexx.utilities.assemblyinformation.Dialogs {
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window, INotifyPropertyChanged {
#region Properties
        public ViewModels.AboutViewModel ViewModel {
            get { return _viewModel; }
            private set {
                _viewModel = value;
                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs("ViewModel")
                );
            }
        } private ViewModels.AboutViewModel _viewModel = 
            new ViewModels.AboutViewModel();
#endregion Properties
    }
    public partial class About {
#region Events
        public event PropertyChangedEventHandler PropertyChanged;
#endregion Events
    }
    public partial class About {
#region Constructors & Destructor
        public About() {
            InitializeComponent();
            DataContext = ViewModel;
        }
#endregion Constructors & Destructor
    }
}
