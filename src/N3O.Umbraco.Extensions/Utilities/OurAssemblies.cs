using N3O.Umbraco.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace N3O.Umbraco.Utilities;

public static class OurAssemblies {
    private static readonly string[] DefaultPrefixes = ["N3O."];

    private static readonly ConcurrentDictionary<Type, Type[]> TypeArrayCache = new();

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

    public static bool IsOurAssemblyName(string assemblyName) {
        if (!assemblyName.HasValue()) {
            return false;
        }

        var prefixes = _ourPrefixes ?? DefaultPrefixes;

        return prefixes.Any(p => assemblyName.StartsWith(p, StringComparison.InvariantCultureIgnoreCase));
    }

    public static IReadOnlyList<Type> GetAllConcreteTypesImplementingInterface(Type interfaceType) {
        return TypeArrayCache.GetOrAdd(interfaceType, static itf => {
            var allMatchingTypes = new List<Type>();

            foreach (var assembly in _assemblies) {
                allMatchingTypes.AddRange(assembly.GetAllConcreteTypesInAssemblyImplementingInterface(itf));
            }

            return allMatchingTypes.ToArray();
        });
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
        processedReferencedAssemblies ??= [];

        var referencedAssemblies = new List<Assembly>();

        if (processedReferencedAssemblies.Contains(assembly)) {
            return referencedAssemblies;
        }

        referencedAssemblies.Add(assembly);
        processedReferencedAssemblies.Add(assembly);

        var ourReferencedAssemblies = assembly.GetReferencedAssemblies()
                                              .Where(IsOurAssembly)
                                              .Select(LoadOurReferencedAssembly)
                                              .Where(a => a != null)
                                              .ToList();

        referencedAssemblies.AddRange(ourReferencedAssemblies);

        foreach (var referencedAssembly in ourReferencedAssemblies) {
            referencedAssemblies.AddRange(LoadAllOurReferencedAssemblies(referencedAssembly,
                                                                         processedReferencedAssemblies));
        }

        return referencedAssemblies.Distinct().ToList();
    }

    // Sibling DLLs in the bin folder are loaded by file path in EnsureOurAssembliesAreLoaded().
    // After that step `Assembly.Load(AssemblyName)` with an exact version can still fail when a
    // NuGet-published consumer in the graph was compiled against a different timestamped version
    // than the local v1.0.0.0 build of the sibling abstractions. Fall back to the already-loaded
    // assembly by simple name so version skew across the package graph doesn't crash startup.
    private static Assembly LoadOurReferencedAssembly(AssemblyName reference) {
        var existing = AppDomain.CurrentDomain
                                .GetAssemblies()
                                .FirstOrDefault(a => a.GetName().Name.EqualsInvariant(reference.Name));

        if (existing != null) {
            return existing;
        }

        try {
            return Assembly.Load(reference);
        } catch (FileNotFoundException) {
            return null;
        }
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
