using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace com.akoimeexx.utilities.assemblyinformation {
    /// <summary>
    /// CAUTION! VOODOO MADJICKS ABOUND HERE:
    /// I understand at a high level how and why this works, but not at a low level.
    /// </summary>
    public static class ReflectionAnalyzer {
        /// <summary>
        /// Loaded in a separate AppDomain context, allowing loaded assemblies 
        /// to be successfully unloaded after retrieving metadata
        /// </summary>
        private class AnalysisProxy : MarshalByRefObject {
            public AssemblyAnalysis Analyze(string path) {
                AssemblyAnalysis a = default(AssemblyAnalysis);
                try {
                    Assembly asm = Assembly.LoadFile(path);
                    AppDomain.CurrentDomain.AssemblyResolve += (o, args) => {
                        string root = Path.GetDirectoryName(path);
                        string assemblyName = new AssemblyName(args.Name).Name;

                        List<string> files = new List<string>();

                        files.AddRange(
                            Directory.GetFiles(root, "*.dll", SearchOption.AllDirectories)
                        );
                        files.AddRange(
                            Directory.GetFiles(root, "*.exe", SearchOption.AllDirectories)
                        );
                        string needle = files.Find(_ => {
                            return (
                                _.Contains(assemblyName + ".dll") ||
                                _.Contains(assemblyName + ".exe")
                            );
                        });
                        if (needle != default(string))
                            return Assembly.LoadFrom(needle);
                        return null;
                    };

                    List<KeyValuePair<string, string[]>> classes = 
                        new List<KeyValuePair<string, string[]>>();
                    foreach (Type classType in asm.ExportedTypes) {
                        List<string> methods = new List<string>();
                        foreach (MethodInfo method in classType.GetMethods(
                            BindingFlags.Public | 
                            BindingFlags.Static | 
                            BindingFlags.Instance
                        )) {
                            methods.Add(method.Name);
                        }
                        classes.Add(new KeyValuePair<string, string[]>(
                            classType.Name,
                            methods.ToArray()
                        ));
                    }
                    a = new AssemblyAnalysis() {
                        FullName = asm.FullName,
                        ImageRuntimeVersion = asm.ImageRuntimeVersion,
                        Location = asm.Location,
                        ExportedTypes = classes.ToArray()
                    };
                } catch (Exception e) {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(e.Message);
#endif
                }
                return a;
            }
        }
        public static AssemblyAnalysis ExamineAssembly(string path) {
            AssemblyAnalysis a = default(AssemblyAnalysis);
            AppDomain domain = default(AppDomain);
            try {
                domain = AppDomain.CreateDomain(
                    "AnalysisDomain", 
                    AppDomain.CurrentDomain.Evidence, 
                    new AppDomainSetup() {
                        ApplicationBase = Environment.CurrentDirectory,
                        PrivateBinPath = System.IO.Path.GetDirectoryName(path)
                    }
                );

                Type t = typeof(AnalysisProxy);
                var v = (AnalysisProxy)domain.CreateInstanceAndUnwrap(
                    t.Assembly.FullName, 
                    t.FullName
                );
                a = v.Analyze(path);
            } catch (Exception e) {
#if DEBUG
        System.Diagnostics.Debug.WriteLine(e.Message);
#endif
            } finally { 
                AppDomain.Unload(domain);
            }
            return a;
        }
        [Serializable]
        public struct AssemblyAnalysis {
            public string FullName { get; set; }
            public string ImageRuntimeVersion { get; set; }
            public string Location { get; set; }
            public KeyValuePair<string, string[]>[] ExportedTypes { get; set; }
        }
    }
}
