using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace N3O.Umbraco.Utilities {
    public static class OurAssemblies {
        private static IReadOnlyList<string> _ourPrefixes;
        private static IReadOnlyList<Assembly> _assemblies;
        private static IReadOnlyList<Type> _exportedTypes;
    
        public static void Configure(params string[] prefixes) {
            _ourPrefixes = prefixes.OrEmpty().Concat("N3O.").ToList();
        
            EnsureOurAssembliesAreLoaded();

            _assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(IsOurAssembly).ToList();
            _exportedTypes = _assemblies.SelectMany(a => a.GetExportedTypes()).ToList();
        }

        public static IReadOnlyList<Assembly> GetAllAssemblies() => _assemblies;

        public static IReadOnlyList<Type> GetTypes(Func<Type, bool> predicate = null) {
            var types = _exportedTypes.Where(t => predicate?.Invoke(t) ?? true).ToList();

            return types;
        }

        private static bool IsOurAssembly(string file) {
            try {
                var assemblyName = AssemblyName.GetAssemblyName(file);

                return IsOurAssembly(assemblyName);
            } catch {
                return false;
            }
        }

        public static bool IsOurAssembly(Assembly assembly) {
            var assemblyName = assembly.GetName();

            return IsOurAssembly(assemblyName);
        }

        private static bool IsOurAssembly(AssemblyName assemblyName) {
            var fullName = assemblyName.FullName;

            var result = _ourPrefixes.Any(x => fullName.StartsWith(x, StringComparison.InvariantCultureIgnoreCase));

            return result;
        }

        private static IReadOnlyList<Assembly> LoadAllOurReferencedAssemblies(Assembly assembly,
                                                                              List<Assembly> processedReferencedAssemblies = null) {
            processedReferencedAssemblies ??= new List<Assembly>();

            var referencedAssemblies = new List<Assembly>();

            if (processedReferencedAssemblies.Contains(assembly)) {
                return referencedAssemblies;
            }

            referencedAssemblies.Add(assembly);
            processedReferencedAssemblies.Add(assembly);

            var ourReferencedAssemblies = assembly.GetReferencedAssemblies()
                                                  .Where(IsOurAssembly)
                                                  .Select(Assembly.Load)
                                                  .ToList();

            referencedAssemblies.AddRange(ourReferencedAssemblies);

            foreach (var referencedAssembly in ourReferencedAssemblies) {
                referencedAssemblies.AddRange(LoadAllOurReferencedAssemblies(referencedAssembly,
                                                                             processedReferencedAssemblies));
            }

            return referencedAssemblies.Distinct().ToList();
        }

        private static void EnsureOurAssembliesAreLoaded() {
            var binFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var binAssemblyFiles = Directory.GetFiles(binFolder)
                                            .Where(f => f.ToLowerInvariant().EndsWith(".dll"))
                                            .ToList();

            foreach (var file in binAssemblyFiles) {
                if (IsOurAssembly(file)) {
                    Assembly.LoadFrom(file);
                }
            }

            var referencedAssemblies = LoadAllOurReferencedAssemblies(Assembly.GetExecutingAssembly());
            foreach (var assembly in referencedAssemblies) {
                if (IsOurAssembly(assembly)) {
                    Assembly.Load(assembly.GetName());
                }
            }
        }
    }
}
