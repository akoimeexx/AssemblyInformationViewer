
namespace com.akoimeexx.utilities.assemblyinformation.Models {
    using System;

    public struct AssemblyInfo {
        public string Name { get; set; }
        public Version Version { get; set; }
        public string Path { get; set; }
        public override string ToString() {
            return String.Format("[\"{0}\", {1}]", Name, Version);
        }
    }
}
