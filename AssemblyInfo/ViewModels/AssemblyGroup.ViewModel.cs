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
    using Validators;

    public partial class AssemblyGroupViewModel : Notifiable {
#region Properties
        //See https://code.msdn.microsoft.com/windowsdesktop/CollectionView-Tips-MVVM-d6ebb4a7 for references on handling CollectionViews
        public ICollectionView Assemblies {
            get { return _assemblies; }
            set {
                Set(ref _assemblies, value);
                if (_assemblies != null)
                    _assemblies.Filter = AssemblyFilter ?? (_ => {
                        return filterByIString(_, AssemblyFilterInput);
                    });
            }
        } private ICollectionView _assemblies = default(ICollectionView);
        [EnumerableStringLength(3,3)]
        public ObservableSortedSet<string> AssemblyExtensions {
            get { return _assemblyExtensions; }
        } private ObservableSortedSet<string> _assemblyExtensions =
            new ObservableSortedSet<string>() {
                "dll",
                "exe"
            };
        public Predicate<object> AssemblyFilter {
            get { return _assemblyFilter; }
            set { Set(ref _assemblyFilter, value); }
        } private Predicate<object> _assemblyFilter =
            default(Predicate<object>);
        public string AssemblyFilterInput {
            get { return _assemblyFilterInput; }
            set {
                Set(ref _assemblyFilterInput, value);
                Assemblies?.Refresh();
            }
        } private string _assemblyFilterInput = default(string);
        public FileInfo AssemblyPath {
            get { return _assemblyPath; }
            set { Set(ref _assemblyPath, value); }
        } private FileInfo _assemblyPath = default(FileInfo);
        public static BrowseDirectoryDialog OpenDirectoryDialog {
            get { return _openPathDialog; }
            internal set { _openPathDialog = value; }
        } private static BrowseDirectoryDialog _openPathDialog =
            new BrowseDirectoryDialog() {
                InitialPath = Environment.CurrentDirectory,
                RestoreDirectory = true
            };
#endregion Properties
    }
    public partial class AssemblyGroupViewModel {
#region Commands
        public ICommand DetailsDialog {
            get { return 
                    _detailsDialog ?? 
                    (_detailsDialog = new CommandBase(
                        p => {
                            return (
                                p.GetType() == typeof(Models.AssemblyInfo) && 
                                File.Exists(
                                    Path.Combine(
                                        ((Models.AssemblyInfo)p).Path,
                                        ((Models.AssemblyInfo)p).Name
                                    )
                                )
                            );
                        }, 
                        a => {
                            new Dialogs.Details(
                                Path.Combine(
                                    ((Models.AssemblyInfo)a).Path,
                                    ((Models.AssemblyInfo)a).Name
                                )
                            ).ShowDialog();
                        }
                    ));
            }
            set { Set(ref _detailsDialog, value); }
        } private ICommand _detailsDialog = default(ICommand);
        public ICommand OpenAssemblyGroup {
            get {
                return
                    _openAssemblyGroup ?? 
                    (_openAssemblyGroup = new CommandBase(
                        a => {
                            bool b = false;
                            if (b = (
                                OpenDirectoryDialog.ShowDialog() == true
                            )) {
                                AssemblyPath = new FileInfo(
                                    OpenDirectoryDialog.SelectedPath
                                );
                                loadAssemblyPath();
                            }
                        }
                    ));
            }
            set { Set(ref _openAssemblyGroup, value); }
        } private ICommand _openAssemblyGroup = default(ICommand);
#endregion Commands
    }
    public partial class AssemblyGroupViewModel {
#region Methods
        /// <summary>
        /// Standard filtering method for wrapping in an ICollectionView filter predicate, case-insensitive
        /// </summary>
        /// <param name="haystack">object from (ICollectionView)AssemblyGroupViewModel.Assemblies</param>
        /// <param name="needle">string to compare for</param>
        /// <returns>true if object matches comparison, false otherwise</returns>
        internal bool filterByIString(
            object haystack, 
            string needle
        ) {
            bool b = false;
            try {
                if (!object.Equals(haystack, null)) {
                    b = true;
                    if (!String.IsNullOrWhiteSpace(needle)) {
                        b = haystack.ToString().ToLower().Contains(
                            needle.ToLower()
                        );
                    }
                }
            } catch { }
            return b;
        }
        internal void loadAssemblyPath() {
            try {
                List<string> files = new List<string>();
                if (
                    AssemblyPath.IsDirectory()
                ) {
                    foreach (string extension in AssemblyExtensions) {
                        files.AddRange(
                            Directory.GetFiles(
                                AssemblyPath.FullName,
                                String.Format("*.{0}", extension.ToLower()),
                                SearchOption.AllDirectories
                            )
                        );
                    }
                } else if (
                    AssemblyPath.Exists &&
                    AssemblyExtensions.Contains(AssemblyPath.Extension)
                ) { files.Add(AssemblyPath.FullName); }

                if (files.Count < 1) throw new FileNotFoundException(
                    String.Format(
                        "No files found matching the pattern \"*.[{0}]\"", 
                        String.Join("|", AssemblyExtensions)
                    )
                );

                List<Models.AssemblyInfo> versions = 
                    new List<Models.AssemblyInfo>();

                foreach (string filename in files) {
                    versions.Add(new Models.AssemblyInfo() {
                        Name = Path.GetFileName(filename),
                        Version = new FileInfo(filename).GetFileVersion(),
                        Path = Path.GetDirectoryName(filename)
                    });
                }

                Assemblies = CollectionViewSource.GetDefaultView(versions);
                Assemblies.GroupDescriptions.Add(
                    new PropertyGroupDescription("Path")
                );
            } catch (Exception e) { }
        }
        public string ToJson() {
            string s = default(string);
            if (
                Assemblies != null && 
                ((CollectionView)Assemblies)
                    .SourceCollection
                    .GetType() == typeof(Models.AssemblyInfo)
            ) {
                List<string> assemblyLines = new List<string>();

                foreach (
                    Models.AssemblyInfo info in 
                    Assemblies.SourceCollection
                ) {
                    assemblyLines.Add(String.Format(
                        "            {{ \"file\": \"{0}\", \"version\": \"{1}\" }}", 
                        info.Path, 
                        info.Version
                    ));
                }
                s = String.Join(
                    Environment.NewLine, 
                    "{", 
                    "    \"AssemblyGroup\": {", 
                    String.Format("        \"AssemblyPath\": \"{0}\", ", 
                        String.Join("\\\\", 
                            AssemblyPath.FullName.Split('\\')
                        )
                    ), 
                    "        \"Assemblies\": [", 
                    String.Join(
                        String.Format(", {0}", Environment.NewLine), 
                        assemblyLines
                    ),
                    "        ]",
                    "    }",
                    "}"
                );
            }
            return s;
        }
#endregion Methods
    }
    public partial class AssemblyGroupViewModel {
#region Constructors & Destructor
        public AssemblyGroupViewModel() : this(Environment.CurrentDirectory) { }
        public AssemblyGroupViewModel(
            string assemblyPath
        ) : this(new FileInfo(assemblyPath)) { }
        public AssemblyGroupViewModel(FileInfo assemblyPath) {
            AssemblyPath = assemblyPath;
            AssemblyFilter = _ => {
                return filterByIString(_, AssemblyFilterInput);
            };
            if (AssemblyPath != null)
                loadAssemblyPath();
        }
        public AssemblyGroupViewModel(
            IEnumerable<Models.AssemblyInfo> importedData
        ) {
            try {
                if (
                    importedData == null ||
                    importedData.ToArray().Length == 0
                ) throw new ArgumentNullException("Unable to import empty data");
                Assemblies = CollectionViewSource.GetDefaultView(importedData);
            } catch (Exception e) {
                e.Data.Add("Dictionary<string, Version> instance", importedData);
                Assemblies = CollectionViewSource.GetDefaultView(e);
            }
        }
#endregion Constructors & Destructor
    }
}
