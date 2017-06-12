using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

using Microsoft.Win32;

namespace com.akoimeexx.utilities.assemblyinformation.ViewModels {
    public partial class StartupViewModel : Notifiable {
#region Properties
        private const int MINGROUPS = 2;
        public ObservableCollection<AssemblyGroupViewModel> AssemblyGroups {
            get { return _assemblyGroups; }
            set { Set(ref _assemblyGroups, value); }
        } private ObservableCollection<AssemblyGroupViewModel> _assemblyGroups =
            new ObservableCollection<AssemblyGroupViewModel>() {
                new AssemblyGroupViewModel(),
                new AssemblyGroupViewModel()
            };
        public static SaveFileDialog ExportDialog {
            get { return _exportDialog; }
            internal set { _exportDialog = value; }
        } private static SaveFileDialog _exportDialog = new SaveFileDialog() {
            AddExtension = true, 
            Filter = "JavaScript Object Notaton files (*.json)|*.json|All files (*.*)|*.*", 
            OverwritePrompt = true
        };
        public bool IsHighlightingEnabled {
            get { return _isHighlightingEnabled; }
            set { Set(ref _isHighlightingEnabled, value); }
        } private bool _isHighlightingEnabled = true;
        public bool IsSelectionMatchingEnabled {
            get { return _isSelectionMatchingEnabled; }
            set {
                Set(ref _isSelectionMatchingEnabled, value);
            }
        } private bool _isSelectionMatchingEnabled = true;
        public ObservableStack<string> Messages {
            get { return _messages; }
            set { Set(ref _messages, value); }
        } private ObservableStack<string> _messages = 
            new ObservableStack<string>();
        public uint MinimumGroups {
            get { return _minimumGroups; }
            internal set { Set(ref _minimumGroups, value); }
        } private uint _minimumGroups = MINGROUPS;
#endregion Properties
    }
    public partial class StartupViewModel {
#region Commands
        public ICommand AboutDialog {
            get {
                return
                    _aboutDialog ??
                    (_aboutDialog = new CommandBase(
                        a => {
                            new Dialogs.About().ShowDialog();
                        }
                    ));
            }
        } private ICommand _aboutDialog = default(ICommand);
        public ICommand AddAssemblyGroup {
            get {
                return 
                    _addAssemblyGroup ?? 
                    (_addAssemblyGroup = new CommandBase(
                        a => {
                            AssemblyGroups.Add(
                                new AssemblyGroupViewModel()
                            );
                        }
                    ));
            }
            set { Set(ref _addAssemblyGroup, value); }
        } private ICommand _addAssemblyGroup = default(ICommand);
        public ICommand ExitApplication {
            get {
                return
                    _exitApplication ??
                    (_exitApplication = new CommandBase(
                        a => {
                            App.Current.Shutdown(0);
                        }
                    ));
            }
        } private ICommand _exitApplication = default(ICommand);
        public ICommand ExportJson {
            get {
                return 
                    _exportJson ?? 
                    (_exportJson = new CommandBase(
                        a => {
                            bool b = false;
                            try {
                                if (b = ExportDialog.ShowDialog() == true) {
                                    List<string> groups = new List<string>();
                                    foreach (var asm in AssemblyGroups) {
                                        groups.Add(asm.ToJson());
                                    }
                                    string json = String.Join(
                                        Environment.NewLine,
                                        "[",
                                        String.Join(
                                            String.Format(
                                                ", {0}", 
                                                Environment.NewLine
                                            ), 
                                            groups
                                        ), 
                                        "]"
                                    );
                                    using (var fs = File.Create(
                                        ExportDialog.FileName
                                    )) {
                                        fs.Write(
                                            Encoding.Default.GetBytes(json), 
                                            0,
                                            Encoding.Default.GetByteCount(json)
                                        );
                                        fs.Close();
                                    }
                                }
                            } catch (Exception e) { Messages.Push(e.Message); }
                        }
                    ));
            }
            set { Set(ref _exportJson, value); }
        } private ICommand _exportJson = default(ICommand);
        public ICommand MatchSelection {
            get { return 
                    _matchSelection ?? 
                    (_matchSelection = new CommandBase(
                        p => {
                            return IsSelectionMatchingEnabled;
                        },
                        a => {

                        }
                    ));
            }
            set { Set(ref _matchSelection, value); }
        } private ICommand _matchSelection = default(ICommand);
        public ICommand RemoveAssemblyGroup {
            get {
                return 
                    _removeAssemblyGroup ?? 
                    (_removeAssemblyGroup = new CommandBase(
                        p => {
                            return (AssemblyGroups?.Count ?? 0) > 1;
                        }, 
                        a => {
                            if (
                                (AssemblyGroups?.Count ?? 0) > 1
                            ) AssemblyGroups.Remove(
                                AssemblyGroups[AssemblyGroups.Count - 1]
                            );
                        }
                    ));
            }
            set { Set(ref _removeAssemblyGroup, value); }
        } private ICommand _removeAssemblyGroup = default(ICommand);
#endregion Commands
    }
    public partial class StartupViewModel {
#region Methods
        private bool exportToJson(string path) {
            bool b = false;
            try {
                if (b = ExportDialog.ShowDialog() == true) {

                }
            } catch (Exception e) {
                Messages.Push(
                    e.Message
                );
            } finally {
                b = File.Exists(path);
            }
            return b;
        }
#endregion Methods
    }
    public partial class StartupViewModel {
#region Constructors & Destructor
        public StartupViewModel() {

        }
#endregion Constructors & Destructor
    }
}
