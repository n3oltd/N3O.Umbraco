using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Search.Typesense;

public static class TypesenseConverterRegistry {
    private static readonly Lazy<IReadOnlyList<ITypesenseConverter>> All = new(LoadAll);

    public static ITypesenseConverter GetConverter(Type type) {
        return All.Value.FirstOrDefault(x => x.CanConvert(type));
    }

    public static bool HasConverterFor(Type type) {
        return All.Value.Any(x => x.CanConvert(type));
    }

    private static IReadOnlyList<ITypesenseConverter> LoadAll() {
        return OurAssemblies.GetTypes(x => x.IsConcreteClass() &&
                                           x.ImplementsInterface<ITypesenseConverter>() &&
                                           x.HasParameterlessConstructor())
                            .Select(x => (ITypesenseConverter) Activator.CreateInstance(x))
                            .ToList();
    }
}
