using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace com.akoimeexx.utilities.assemblyinformation.Dialogs
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : Window, INotifyPropertyChanged {
#region Properties
        public ViewModels.DetailsViewModel ViewModel {
            get { return _viewModel; }
            set {
                _viewModel = value;
                PropertyChanged?.Invoke(
                    this, 
                    new PropertyChangedEventArgs("ViewModel")
                );
            }
        } private ViewModels.DetailsViewModel _viewModel = 
            new ViewModels.DetailsViewModel();
#endregion Properties
    }
    public partial class Details {
#region Events
        public event PropertyChangedEventHandler PropertyChanged;
#endregion Events
    }
    public partial class Details {
#region Constructors & Destructor
        public Details(string assemblyPath) {
            try {
                if (!File.Exists(assemblyPath))
                    throw new FileNotFoundException(String.Format(
                        "Could not find the file specified at \"{0}\"", 
                        assemblyPath
                    ));
                InitializeComponent();
                ViewModel.AssemblyDetails = 
                    ReflectionAnalyzer.ExamineAssembly(assemblyPath);
                DataContext = ViewModel;
            } catch (Exception e) {
                MessageBox.Show(
                    String.Join(
                        Environment.NewLine,
                        "An error occured while loading assembly details:",
                        e.Message
                    ),
                    "Exception thrown",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                Close();
            }
        }
#endregion Constructors & Destructor
    }
}
