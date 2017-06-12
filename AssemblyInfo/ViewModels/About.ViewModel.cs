using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace com.akoimeexx.utilities.assemblyinformation.ViewModels {
    public partial class AboutViewModel : Notifiable {
#region Properties
        public Uri GithubLink {
            get { return _githubLink; }
            private set { Set(ref _githubLink, value); }
        } private Uri _githubLink = new Uri("https://github.com/akoimeexx");
        public string Version {
            get {
                return _version ?? (
                    _version = new System.IO.FileInfo(
                        new Uri(
                            Assembly.GetExecutingAssembly().CodeBase
                        ).LocalPath
                    ).GetFileVersion().ToString()
                );
            }
            private set { Set(ref _version, value); }
        } private string _version = default(string);
#endregion Properties
    }
    public partial class AboutViewModel {
#region Commands
        public ICommand CloseDialog {
            get {
                return
                    _closeDialog ??
                    (_closeDialog = new CommandBase(
                        a => {
                            if ((a as Window) != null)
                                ((Window)a).DialogResult = true;
                        }
                    ));
            }
            private set { Set(ref _closeDialog, value); }
        } private ICommand _closeDialog = default(ICommand);
        public ICommand OpenGithub {
            get {
                return
                    _openGithub ??
                    (_openGithub = new CommandBase(
                        a => {
                            GithubLink.OpenExternal();
                            if ((a as Window) != null)
                                ((Window)a).DialogResult = true;
                        }
                    ));
            }
            private set { Set(ref _openGithub, value); }
        } private ICommand _openGithub = default(ICommand);
#endregion Commands
    }
}
