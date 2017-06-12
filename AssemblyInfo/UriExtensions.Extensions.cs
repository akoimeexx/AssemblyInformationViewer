namespace com.akoimeexx.utilities.assemblyinformation {
    using System;
    using System.Diagnostics;
    
    public static partial class UriExtensions {
        public static void OpenExternal(this Uri u) {
            Process.Start(new ProcessStartInfo(u.AbsoluteUri));
        }
    }
}
