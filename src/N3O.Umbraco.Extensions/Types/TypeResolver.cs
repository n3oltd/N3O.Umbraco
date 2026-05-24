using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace N3O.Umbraco.Types;

public static class TypeResolver {
    private const string OurPinnedVersion = "1.0.0.0";

    private static readonly Regex VersionToken = new(
        @"(?<name>[A-Za-z0-9._-]+),\s*Version=(?<version>\d+\.\d+\.\d+\.\d+)",
        RegexOptions.Compiled);

    private static readonly Regex AssemblyMetadata = new(
        @",\s*(Version=[\d.]+|Culture=[^,\]]*|PublicKeyToken=[^,\]]*)",
        RegexOptions.Compiled);

    public static Type Resolve(string name) {
        if (!name.HasValue()) {
            return null;
        }

        var type = TryGetType(name);

        if (type == null) {
            var pinned = PersistedName(name);

            if (!string.Equals(pinned, name, StringComparison.Ordinal)) {
                type = TryGetType(pinned);
            }
        }

        if (type == null) {
            var stripped = AssemblyMetadata.Replace(name, "");

            if (!stripped.EqualsInvariant(name)) {
                type = TryGetType(stripped);
            }
        }

        if (type == null) {
            type = OurAssemblies.GetTypes(x => x.FullName.EqualsInvariant(name)).SingleOrDefault();
        }

        return type;
    }

    public static string PersistedName(Type type) {
        return type == null ? null : PersistedName(type.AssemblyQualifiedName);
    }

    public static string PersistedName(string assemblyQualifiedName) {
        if (!assemblyQualifiedName.HasValue()) {
            return assemblyQualifiedName;
        }

        return VersionToken.Replace(assemblyQualifiedName, match => {
            var name = match.Groups["name"].Value;
            var pinned = PinnedVersionFor(name);

            return pinned == null
                ? match.Value
                : $"{name}, Version={pinned}";
        });
    }

    private static string PinnedVersionFor(string assemblyName) {
        return OurAssemblies.IsOurAssemblyName(assemblyName) ? OurPinnedVersion : null;
    }

    private static Type TryGetType(string name) {
        return Type.GetType(name);
    }
}
