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


                    a = new AssemblyAnalysis() {
                        FullName = asm.FullName,
                        ImageRuntimeVersion = asm.ImageRuntimeVersion,
                        Location = asm.Location, 
                    };

                    foreach (Type t in asm.ExportedTypes) {
                        bool isStruct = (t.IsValueType && !t.IsEnum);
                        if (t.IsClass || isStruct) { // Is Class or Struct
                            InstanceAnalysis o = (isStruct) ?
                                new InstanceAnalysis() :
                                new ClassAnalysis();
                            // Set Name
                            o.Name = t.Name;
                            // Check if Abstract
                            if (!isStruct) {
                                ((ClassAnalysis)o).IsAbstract = (t.IsAbstract && !t.IsSealed);
                                ((ClassAnalysis)o).IsStatic = (t.IsAbstract && t.IsSealed);
                            }
                            // Set Ancestors, if any
                            List<KeyValuePair<string, string[]>> ancestors =
                                new List<KeyValuePair<string, string[]>>();
                            if (
                                !isStruct &&
                                t.BaseType != null && 
                                t.BaseType != typeof(Object)
                            ) ancestors.Add(
                                new KeyValuePair<string, string[]>(
                                    "Base Class",
                                    new string[] { t.BaseType.Name }
                                )
                            );
                            Type[] interfaces = t.GetInterfaces();
                            if (interfaces.Length > 0) {
                                List<string> names = new List<string>();
                                foreach (Type type in interfaces) {
                                    names.Add(type.Name);
                                }
                                ancestors.Add(
                                    new KeyValuePair<string, string[]>(
                                        "Interfaces",
                                        names.ToArray()
                                    )
                                );
                            }
                            if (ancestors.Count > 0)
                                o.Ancestors = ancestors.ToArray();
                            // Set Constructors, if any
                            List<string> constructors = new List<string>();
                            foreach (var c in t.GetConstructors(
                                BindingFlags.Public | 
                                BindingFlags.Instance | 
                                BindingFlags.Static
                            )) {
                                List<string> parameters = new List<string>();
                                foreach (var p in c.GetParameters()) {
                                    parameters.Add(String.Format(
                                        "{0}{1}", 
                                        p.ParameterType.Name, 
                                        p.HasDefaultValue ? 
                                            String.Format(
                                                " [{0}]", 
                                                p.DefaultValue
                                            ) : 
                                            ""
                                    ));
                                }
                                constructors.Add(String.Format(
                                    "{0}{1}", 
                                    c.Name, 
                                    parameters.Count > 0 ? 
                                        String.Format(
                                            " ({0})", 
                                            String.Join(", ", parameters)
                                        ) : 
                                        ""
                                ));
                            }
                            if (constructors.Count > 0)
                                o.Constructors = constructors.ToArray();
                            // Set Events, if any
                            List<string> events = new List<string>();
                            foreach (var e in t.GetEvents(
                                BindingFlags.FlattenHierarchy |
                                BindingFlags.Instance |
                                BindingFlags.Public |
                                BindingFlags.Static                                
                            )) events.Add(String.Format(
                                "<{0}> {1}",
                                e.EventHandlerType.Name, 
                                e.Name
                            ));
                            if (events.Count > 0)
                                o.Events = events.ToArray();
                            // Set Methods, if any
                            List<string> methods = new List<string>();
                            foreach (var m in t.GetMethods(
                                BindingFlags.FlattenHierarchy |
                                BindingFlags.Instance |
                                BindingFlags.Public |
                                BindingFlags.Static 
                            )) {
                                if (!m.IsSpecialName) {
                                    List<string> parameters = new List<string>();
                                    foreach (var p in m.GetParameters())
                                    {
                                        parameters.Add(String.Format(
                                            "{0}{1}",
                                            p.ParameterType.Name,
                                            p.HasDefaultValue ?
                                                String.Format(
                                                    " [{0}]",
                                                    p.DefaultValue
                                                ) :
                                                ""
                                        ));
                                    }
                                    methods.Add(String.Format(
                                        "{0}{1}", 
                                        m.Name, 
                                        parameters.Count > 0 ?
                                            String.Format(
                                                " ({0})",
                                                String.Join(", ", parameters)
                                            ) :
                                            ""
                                    ));
                                }
                            }
                            if (methods.Count > 0)
                                o.Methods = methods.ToArray();
                            // Set Properties, if any
                            List<string> properties = new List<string>();
                            foreach (var p in t.GetProperties(
                                BindingFlags.FlattenHierarchy |
                                BindingFlags.Instance |
                                BindingFlags.Public |
                                BindingFlags.Static
                            )) properties.Add(String.Format(
                                "<{0}> {1}{2}", 
                                p.PropertyType.Name, 
                                p.Name, 
                                p.CanRead || p.CanWrite ?
                                    String.Format(" [{0}{1} ]", 
                                        p.CanRead ? " get;" : "", 
                                        p.CanWrite ? " set;" : ""
                                    ) : 
                                    ""
                            ));
                            if (properties.Count > 0)
                                o.Properties = properties.ToArray();
                            if (isStruct) {
                                List<InstanceAnalysis> instances =
                                    new List<InstanceAnalysis>(
                                        a.Structs ?? new InstanceAnalysis[] { }
                                    );
                                instances.Add(o);
                                a.Structs = instances.ToArray();
                            } else {
                                List<ClassAnalysis> instances =
                                    new List<ClassAnalysis>(
                                        a.Classes ?? new ClassAnalysis[] { }
                                    );
                                instances.Add((ClassAnalysis)o);
                                a.Classes = instances.ToArray();
                            }
                        } else if (t.IsEnum) { // Is Enum
                            EnumAnalysis e = new EnumAnalysis();
                            e.Name = t.Name;

                            List<string> names = new List<string>(Enum.GetNames(t));
                            List<string> values = new List<string>();
                            List<KeyValuePair<string, string>> results = 
                                new List<KeyValuePair<string, string>>();
                            foreach (var v in Enum.GetValues(t))
                                values.Add(((int)v).ToString());
                            for (int i = 0; i < names.Count; i++)
                                results.Add(new KeyValuePair<string, string>(
                                    names[i],
                                    values[i]
                                ));
                            e.Values = results.ToArray();

                            List<EnumAnalysis> instances = 
                                new List<EnumAnalysis>(
                                    a.Enums ?? new EnumAnalysis[] { }
                                );
                            instances.Add(e);
                            a.Enums = instances.ToArray();
                        } else if (t.IsInterface) { // Is Interface
                            ObjectAnalysis i = new ObjectAnalysis();
                            i.Name = t.Name;
                            // Set Ancestors, if any
                            List<KeyValuePair<string, string[]>> ancestors =
                                new List<KeyValuePair<string, string[]>>();
                            Type[] interfaces = t.GetInterfaces();
                            if (interfaces.Length > 0) {
                                List<string> names = new List<string>();
                                foreach (Type type in interfaces) {
                                    names.Add(type.Name);
                                }
                                ancestors.Add(
                                    new KeyValuePair<string, string[]>(
                                        "Interfaces",
                                        names.ToArray()
                                    )
                                );
                            }
                            if (ancestors.Count > 0)
                                i.Ancestors = ancestors.ToArray();
                            // Set Events, if any
                            List<string> events = new List<string>();
                            foreach (var e in t.GetEvents(
                                BindingFlags.FlattenHierarchy |
                                BindingFlags.Instance |
                                BindingFlags.Public |
                                BindingFlags.Static                                
                            )) events.Add(String.Format(
                                "<{0}> {1}",
                                e.EventHandlerType.Name, 
                                e.Name
                            ));
                            if (events.Count > 0)
                                i.Events = events.ToArray();
                            // Set Methods, if any
                            List<string> methods = new List<string>();
                            foreach (var m in t.GetMethods(
                                BindingFlags.FlattenHierarchy |
                                BindingFlags.Instance |
                                BindingFlags.Public |
                                BindingFlags.Static
                            )) {
                                if (!m.IsSpecialName) {
                                    List<string> parameters = new List<string>();
                                    foreach (var p in m.GetParameters()) {
                                        parameters.Add(String.Format(
                                            "{0}{1}",
                                            p.ParameterType.Name,
                                            p.HasDefaultValue ?
                                                String.Format(
                                                    " [{0}]",
                                                    p.DefaultValue
                                                ) :
                                                ""
                                        ));
                                    }
                                    methods.Add(String.Format(
                                        "{0}{1}",
                                        m.Name,
                                        parameters.Count > 0 ?
                                            String.Format(
                                                " ({0})",
                                                String.Join(", ", parameters)
                                            ) :
                                            ""
                                    ));
                                }
                            }
                            if (methods.Count > 0)
                                i.Methods = methods.ToArray();
                            // Set Properties, if any
                            List<string> properties = new List<string>();
                            foreach (var p in t.GetProperties(
                                BindingFlags.FlattenHierarchy |
                                BindingFlags.Instance |
                                BindingFlags.Public |
                                BindingFlags.Static
                            )) properties.Add(String.Format(
                                "<{0}> {1}{2}", 
                                p.PropertyType.Name, 
                                p.Name, 
                                p.CanRead || p.CanWrite ?
                                    String.Format(" [{0}{1} ]", 
                                        p.CanRead ? " get;" : "", 
                                        p.CanWrite ? " set;" : ""
                                    ) : 
                                    ""
                            ));
                            if (properties.Count > 0)
                                i.Properties = properties.ToArray();
                            List<ObjectAnalysis> instances =
                                new List<ObjectAnalysis>(
                                    a.Interfaces ?? new ObjectAnalysis[] { }
                                );
                            instances.Add(i);
                            a.Interfaces = instances.ToArray();
                        }
                    }
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
    }
    [Serializable]
    public struct AssemblyAnalysis {
        public string FullName { get; set; }
        public string ImageRuntimeVersion { get; set; }
        public string Location { get; set; }
        public ClassAnalysis[] Classes { get; set; }
        public EnumAnalysis[] Enums { get; set; }
        public ObjectAnalysis[] Interfaces { get; set; }
        public InstanceAnalysis[] Structs { get; set; }
    }
    [Serializable]
    public abstract class BaseAnalysis {
        public string Name { get; set; }
    }
    [Serializable] // Enums
    public class EnumAnalysis : BaseAnalysis {
        public KeyValuePair<string, string>[] Values { get; set; }
    }
    [Serializable] // Interfaces
    public class ObjectAnalysis : BaseAnalysis {
        public KeyValuePair<string, string[]>[] Ancestors { get; set; }
        public string[] Events { get; set; }
        public string[] Methods { get; set; }
        public string[] Properties { get; set; }

    }
    [Serializable] // Structs
    public class InstanceAnalysis : ObjectAnalysis {
        public string[] Constructors { get; set; }
    }
    [Serializable] // Classes
    public class ClassAnalysis : InstanceAnalysis {
        public bool IsAbstract { get; set; }
        public bool IsStatic { get; set; }
    }
}
