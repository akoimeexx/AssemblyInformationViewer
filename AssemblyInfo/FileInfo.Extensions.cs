using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.akoimeexx.utilities.assemblyinformation {
    public static partial class FileInfoExtensions {
        public static Version GetFileVersion(this FileInfo instance) {
            Version v = null;
            try {
                var versionInfo = FileVersionInfo.GetVersionInfo(
                    instance.FullName
                );
                v = new Version(
                    versionInfo.FileMajorPart,
                    versionInfo.FileMinorPart,
                    versionInfo.FileBuildPart,
                    versionInfo.FilePrivatePart
                );
            } catch (Exception e) { throw e; }
            return v;
        }
        public static bool IsDirectory(this FileInfo instance) {
            bool b = false;
            try {
                b = (
                    instance.Attributes & 
                    FileAttributes.Directory
                ) == FileAttributes.Directory;
            } catch (Exception e) { throw e; }
            return b;
        }
    }
}
