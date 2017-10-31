using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AssemblyInformation {
    using ai = com.akoimeexx.utilities.assemblyinformation;

    [TestClass]
    public class StartupViewModelTests {
        [TestMethod]
        [TestCategory("Startup Window"), TestCategory("Commands")]
        public void CommandsNotNull() {
            ai.ViewModels.StartupViewModel vm = 
                new ai.ViewModels.StartupViewModel();

            string errorMessage =
                "{0} command should default to auto-generated command on Get, returned null instead.";

            Assert.AreNotEqual(
                null,
                vm.AboutDialog,
                String.Format(
                    errorMessage,
                    "AboutDialog"
                )
            );
            Assert.AreNotEqual(
                null,
                vm.AddAssemblyGroup,
                String.Format(
                    errorMessage,
                    "AddAssemblyGroup"
                )
            );
            Assert.AreNotEqual(
                null,
                vm.ExitApplication,
                String.Format(
                    errorMessage, 
                    "ExitApplication"
                )
            );
            Assert.AreNotEqual(
                null,
                vm.ExportJson,
                String.Format(
                    errorMessage, 
                    "ExportJson"
                )
            );
            Assert.AreNotEqual(
                null,
                vm.MatchSelection,
                String.Format(
                    errorMessage,
                    "MatchSelection"
                )
            );
            Assert.AreNotEqual(
                null,
                vm.RemoveAssemblyGroup,
                String.Format(
                    errorMessage,
                    "RemoveAssemblyGroup"
                )
            );
        }
        [TestMethod]
        [TestCategory("Startup Window"), TestCategory("Commands")]
        public void AddAssemblyGroup() {
            ai.ViewModels.StartupViewModel vm = 
                new ai.ViewModels.StartupViewModel();

            int i = vm.AssemblyGroups.Count;
            vm.AddAssemblyGroup.Execute(null);

            Assert.AreEqual(i+1, vm.AssemblyGroups.Count);
        }
        [TestMethod]
        [TestCategory("Startup Window"), TestCategory("Commands")]
        public void RemoveAssemblyGroup() {
            ai.ViewModels.StartupViewModel vm = 
                new ai.ViewModels.StartupViewModel();

            int i = vm.AssemblyGroups.Count;
            vm.RemoveAssemblyGroup.Execute(i);

            Assert.AreEqual(i-1, vm.AssemblyGroups.Count);
        }
    }
}
