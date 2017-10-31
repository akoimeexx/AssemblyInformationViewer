using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AssemblyInformation {
    using ai = com.akoimeexx.utilities.assemblyinformation;
    [TestClass]
    public class ClassExtensionsTests {
        [TestMethod]
        [TestCategory("Class Extensions"), TestCategory("FileInfo")]
        public void FileInfoGetFileVersion() {
            Version v = ai.FileInfoExtensions.GetFileVersion(
                new System.IO.FileInfo(
                    @"..\..\ClassExtensions.Tests.cs"
                )
            );
            Assert.AreNotEqual(default(Version), v);
        }
        [TestMethod]
        [TestCategory("Class Extensions"), TestCategory("FileInfo")]
        public void FileInfoIsDirectory() {
            bool? b = ai.FileInfoExtensions.IsDirectory(
                new System.IO.FileInfo(
                    @"..\"
                )
            );
            Assert.AreNotEqual(default(bool?), b);
            Assert.AreEqual(true, b);
            b = ai.FileInfoExtensions.IsDirectory(
                new System.IO.FileInfo(
                    @"..\..\ClassExtensions.Tests.cs"
                )
            );
            Assert.AreEqual(false, b);
        }
        [TestMethod]
        [TestCategory("Class Extensions"), TestCategory("String")]
        public void StringToDiff() {
            string s = ai.StringExtensions.ToDiff(
                "Hello World!", "Hell World!"
            );
            Assert.AreNotEqual(String.Empty, s);
        }
        [TestMethod]
        [TestCategory("Class Extensions"), TestCategory("Uri")]
        public void UriOpenExternal() {
            System.Diagnostics.Process p = 
                ai.UriExtensions.OpenExternal(
                    new Uri("https://github.com/akoimeexx/AssemblyInformationViewer")
                );
            Assert.AreNotEqual(
                default(System.Diagnostics.Process),
                p
            );
        }
    }
}
