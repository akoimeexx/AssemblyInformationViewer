namespace com.akoimeexx.utilities.assemblyinformation {
    using System;
    using System.Diagnostics;
    
    public static partial class UriExtensions {
        public static Process OpenExternal(this Uri u) {
            return Process.Start(new ProcessStartInfo(u.AbsoluteUri));
        }
    }
}
