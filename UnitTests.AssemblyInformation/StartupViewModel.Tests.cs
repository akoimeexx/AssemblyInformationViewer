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
        public void SuccessfulViewModelInitialization() {
            ai.ViewModels.StartupViewModel vm = 
                new ai.ViewModels.StartupViewModel();

            Assert.AreEqual(true, true);
        }
    }
}
