namespace Microsoft.Win32 {
    using System;
    using System.IO;
    using System.Text;
    using System.Runtime.InteropServices;

    public sealed partial class BrowseDirectoryDialog : CommonDialog {
#region Properties
        private int dialogFlags {
            get {
                int i = (int)(
                    PInvoke.BrowseStyles.NewDialogStyle 
                );
                if (!ShowNewFolderButton)
                    i |= (int)PInvoke.BrowseStyles.HideNewFolderButton;
                if (ShowTextbox)
                    i |= (int)PInvoke.BrowseStyles.ShowTextBox;
                if (ValidateSelection)
                    i |= (int)PInvoke.BrowseStyles.ValidateSelection;
                return i;
            }
        }
        public DirectoryInfo InitialDirectory {
            get; set;
        } = default(DirectoryInfo);
        public string InitialPath {
            get { return InitialDirectory?.FullName ?? default(string); }
            set {
                InitialDirectory = (Directory.Exists(value)) ?
                    new DirectoryInfo(value) :
                    default(DirectoryInfo);
            }
        }
        public bool RestoreDirectory { get; set; } = default(bool);
        public DirectoryInfo SelectedDirectory {
            get;
            private set;
        } = default(DirectoryInfo);
        public string SelectedPath {
            get { return SelectedDirectory?.FullName ?? default(string); }
            private set {
                SelectedDirectory = (Directory.Exists(value)) ?
                    new DirectoryInfo(value) :
                    default(DirectoryInfo);
            }
        }
        public bool ShowNewFolderButton { get; set; } = default(bool);
        public bool ShowTextbox { get; set; } = default(bool);
        public string Title { get; set; } = default(string);
        public bool ValidateSelection { get; set; } = default(bool);
#endregion Properties
    }
    public sealed partial class BrowseDirectoryDialog {
#region Methods
        private int browseDirectoryCallback(
            IntPtr hwnd, 
            uint message,
            IntPtr lp, 
            IntPtr data
        ) {
            int i = default(int);
            try {
                switch (message) {
                    case (uint)PInvoke.BrowseSignals.Initialized:
                        if (InitialDirectory != default(DirectoryInfo)) {
                            PInvoke.SendMessage(
                                new HandleRef(null, hwnd), 
                                (int)PInvoke.BrowseSignals.SetSelectionW, 
                                1, 
                                InitialPath
                            );
                        }
                        break;
                    case (uint)PInvoke.BrowseSignals.SelectionChanged:
                        IntPtr pathPtr = Marshal.AllocHGlobal(
                            260 * Marshal.SystemDefaultCharSize
                        );
                        if (
                            PInvoke.SHGetPathFromIDList(lp, pathPtr)
                        ) PInvoke.SendMessage(
                            new HandleRef(null, hwnd), 
                            (uint)PInvoke.BrowseSignals.SetStatusExtW, 
                            0, 
                            pathPtr
                        );
                        Marshal.FreeHGlobal(pathPtr);
                        break;
                }
            } catch (Exception e) { }
            return i;
        } 
        public override void Reset() {
            InitialDirectory = default(DirectoryInfo);
            RestoreDirectory = default(bool);
            SelectedDirectory = default(DirectoryInfo);
            ShowNewFolderButton = default(bool);
            ShowTextbox = default(bool);
            Title = default(string);
            ValidateSelection = default(bool);
        }

        protected override bool RunDialog(IntPtr hwndOwner) {
            bool b = false;
            IntPtr root = IntPtr.Zero;
            IntPtr res = IntPtr.Zero;
            try {
                IntPtr owner = (hwndOwner != IntPtr.Zero) ?
                    hwndOwner :
                    PInvoke.GetActiveWindow();
                PInvoke.SHGetSpecialFolderLocation(
                    owner,
                    (int)PInvoke.ShellFolders.MyComputer,
                    out root
                );
                if (root == IntPtr.Zero) throw new Exception(
                    "Could not access ShellFolders"
                );
                IntPtr buffer = Marshal.AllocHGlobal(260);
                PInvoke.BrowseInfo bi = new PInvoke.BrowseInfo() {
                    Root = root,
                    Owner = owner, 
                    Callback = new PInvoke.BrowseCallback(
                        browseDirectoryCallback
                    ), 
                    DisplayName = buffer,
                    Title = Title, 
                    Flags = dialogFlags
                };
                res = PInvoke.SHBrowseForFolder(ref bi);
                Marshal.FreeHGlobal(buffer);
                StringBuilder sb = new StringBuilder(260);
                if (
                    res != IntPtr.Zero &&
                    PInvoke.SHGetPathFromIDList(res, sb) != 0
                ) {
                    SelectedDirectory = new DirectoryInfo(
                        sb.ToString()
                    );
                    if (RestoreDirectory)
                        InitialDirectory = SelectedDirectory;
                    b = true;
                }
            } catch (Exception e) {
                System.Windows.MessageBox.Show(
                    e.Message,
                    "Exception thrown",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error
                );
            } finally {
                PInvoke.IMalloc malloc;
                PInvoke.SHGetMalloc(out malloc);
                malloc.Free(root);
                if (res != IntPtr.Zero)
                    malloc.Free(res);
            }
            return b;
        }
#endregion Methods
    }
}
namespace System.Runtime.InteropServices {
    using System.Text;

    internal sealed partial class PInvoke {
#region COM Enums
        public enum BrowseSignals {
            WindowManager = 0x400, 
            Initialized      = 1, 
            SelectionChanged = 2, 
            SetSelectionW = WindowManager + 103, 
            SetStatusExtW = WindowManager + 104
        }
        [Flags]
        public enum BrowseStyles {
            RestrictToFileSystem = 0x0001, 
            RestrictToDomain     = 0x0002, 
            RestrictToSubfolders = 0x0008, 
            ShowTextBox          = 0x0010, 
            ValidateSelection    = 0x0020, 
            NewDialogStyle       = 0x0040,
            HideNewFolderButton  = 0x0200, 
            BrowserForComputer   = 0x1000, 
            BrowserForPrinter    = 0x2000, 
            BrowserForEverything = 0x4000
        }
        public enum ShellFolders {
            Desktop                 = 0x0000,
            Printers                = 0x0004,
            MyDocuments             = 0x0005,
            Favorites               = 0x0006,
            Recent                  = 0x0008,
            SendTo                  = 0x0009,
            StartMenu               = 0x000b,
            MyComputer              = 0x0011,
            NetworkNeighborhood     = 0x0012,
            Templates               = 0x0015,
            MyPictures              = 0x0027,
            NetAndDialUpConnections = 0x0031
        }
#endregion COM Enums
    }
    internal sealed partial class PInvoke {
#region COM Guids
        private static class ComGuids {
            public const string IMalloc = 
                "00000002-0000-0000-C000-000000000046";
        }
#endregion COM Guids
    }
    internal sealed partial class PInvoke {
#region COM Interfaces
        [
            InterfaceType(ComInterfaceType.InterfaceIsIUnknown), 
            Guid(ComGuids.IMalloc)
        ] public interface IMalloc {
            [PreserveSig] IntPtr Alloc([In] int cb);
            [PreserveSig] IntPtr Realloc([In] IntPtr pv, [In] int cb);
            [PreserveSig] void Free([In] IntPtr pv);
            [PreserveSig] int GetSize([In] IntPtr pv);
            [PreserveSig] int DidAlloc(IntPtr pv);
            [PreserveSig] void HeapMinimize();

        }
#endregion COM Interfaces
    }
    internal sealed partial class PInvoke {
#region Delegates
        public delegate int BrowseCallback(
            IntPtr hwnd, 
            uint umsg, 
            IntPtr lparam, 
            IntPtr lpdata
        );
#endregion Delegates
    }
    internal sealed partial class PInvoke {
#region Methods
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", PreserveSig=true)]
        public static extern IntPtr SendMessage(
            HandleRef hWnd, 
            uint Msg, 
            int wParam, 
            IntPtr lParam
        );
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessage(
            HandleRef hWnd, 
            int msg, 
            int wParam, 
            string lParam
        );
        [DllImport("shell32.dll")]
        public static extern int SHGetMalloc(out IMalloc malloc);
        [DllImport("shell32.dll")]
        public static extern int SHGetSpecialFolderLocation(
            IntPtr owner, int folder, out IntPtr Id
        );
        [DllImport("shell32.dll", CharSet=CharSet.Unicode)]
        public static extern bool SHGetPathFromIDList(
            IntPtr pidl, IntPtr pszPath
        );
        [DllImport("shell32.dll")]
        public static extern int SHGetPathFromIDList(
            IntPtr Id, StringBuilder Path
        );
        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SHBrowseForFolder(ref BrowseInfo bi);
#endregion Methods
    }
    internal sealed partial class PInvoke {
#region Structs
        [StructLayout(LayoutKind.Sequential, Pack=8)]
        public struct BrowseInfo {
            public IntPtr Owner;
            public IntPtr Root;
            public IntPtr DisplayName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string Title;
            public int Flags;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public BrowseCallback Callback;
            public IntPtr Parameters;
            public int Image;
        }
#endregion Structs
    }
}
