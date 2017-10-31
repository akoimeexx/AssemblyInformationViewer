using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.akoimeexx.utilities.assemblyinformation {
    public static partial class NumericExtensions {
        private static Dictionary<int, string> NumericalUnits {
            get {
                return new Dictionary<int, string>() {
                    { 0, "zero" },
                    { 1, "one" }, 
                    { 2, "two" }, 
                    { 3, "three" }, 
                    { 4, "four" }, 
                    { 5, "five" }, 
                    { 6, "six" }, 
                    { 7, "seven" }, 
                    { 8, "eight" }, 
                    { 9, "nine" }, 
                    { 10, "ten" }, 
                    { 11, "eleven" }, 
                    { 12, "twelve" }, 
                    { 13, "thirteen" }, 
                    { 14, "fourteen" }, 
                    { 15, "fifteen" }, 
                    { 16, "sixteen" }, 
                    { 17, "seventeen" }, 
                    { 18, "eighteen" }, 
                    { 19, "nineteen" }, 
                    { 20, "twenty" }, 
                    { 30, "thirty" }, 
                    { 40, "forty" }, 
                    { 50, "fifty" }, 
                    { 60, "sixty" }, 
                    { 70, "seventy" }, 
                    { 80, "eighty" }, 
                    { 90, "ninety" }
                };
            }
        }
        private static string[] Placements {
            get {
                return new string[] {
                    "hundred",           // 100
                    "thousand",          // 1,000
                    "million",           // 1,000,000
                    "billion",           // 1,000,000,000
                    "trillion",          // 1,000,000,000,000
                    "quadrillion",       // 1,000,000,000,000,000
                    "quintillion",       // 1,000,000,000,000,000,000
                    "sextillion",        // 1,000,000,000,000,000,000,000
                    "septillion",        // 1,000,000,000,000,000,000,000,000
                    "octillion",         // 1,000,000,000,000,000,000,000,000,000
                    "nonillion",         // 1,000,000,000,000,000,000,000,000,000,000
                    "decillion",         // 1,000,000,000,000,000,000,000,000,000,000,000
                    "undecillion",       // 1,000,000,000,000,000,000,000,000,000,000,000,000
                    "duodecillion",      // 1,000,000,000,000,000,000,000,000,000,000,000,000,000
                    "tredecillion",      // 1,000,000,000,000,000,000,000,000,000,000,000,000,000,000
                    "quattuordecillion", // 1,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000
                    "quindecillion",     // 1,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000
                    "sexdecillion",      // 1,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000
                    "septendecillion",   // 1,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000
                    "octodecillion",     // 1,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000
                    "novemdecillion",    // 1,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000 
                    "vigintillion"       // 1,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000,000
                };
            }
        }
        private static string[] SizeExtensions {
            get {
                return new string[] {
                    "B", "KB", "MB", "GB", "TB", "PB", "EB"
                };
            }
        }
        public static string FileSize(this byte bytes) {
            return FileSize(Convert.ToInt64(bytes));
        }
        public static string FileSize(this int bytes) {
            return FileSize(Convert.ToInt64(bytes));
        }
        public static string FileSize(this long bytes) {
            string s = default(string);
            try {
                int exponent = Convert.ToInt32(
                    Math.Floor(
                        Math.Log(bytes, 1024)
                    )
                );
                s = String.Format(
                    "{0:0.00}{1}", 
                    (Math.Abs(bytes) != 0) ?
                        (Math.Sign(
                            bytes
                        ) * Math.Round(
                            bytes / Math.Pow(
                                1024, exponent
                            ), 
                            1)
                        ) : 0,
                    SizeExtensions[exponent]
                );
            } finally { }
            return s;
        }
        public static string ToWord(this byte value) {
            return ToWord(Convert.ToInt64(value));
        }
        public static string ToWord(this int value) {
            return ToWord(Convert.ToInt64(value));
        }
        public static string ToWord(this long value) {
            string s = value.ToString();
            try {
                List<string> components = new List<string>();
                if (value < 0)
                    components.Add("negative");
            } finally { }
            return s;
        }
    }
}