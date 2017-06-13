using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.akoimeexx.utilities.assemblyinformation {
    public static partial class StringExtensions {
        public static string ToDiff(this string comparator, string comparison) {
            //See https://www.codeproject.com/Articles/13326/An-O-ND-Difference-Algorithm-for-C for examples on how to build a diff algorithm
            List<string> diffCollection = new List<string>();
            try {

            } finally { }
            return String.Join(Environment.NewLine, diffCollection);
        }
    }
}
