using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups;

[StaticLookups]
public abstract class TypesLookupsCollection<T> : LookupsCollection<T> where T : ILookup {
    private static readonly IReadOnlyList<T> All;

    static TypesLookupsCollection() {
        All = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                          t.IsSubclassOfType(typeof(T)) &&
                                          t.HasParameterlessConstructor())
                           .Select(t => (T) Activator.CreateInstance(t))
                           .ToList();
    }

    protected override Task<IReadOnlyList<T>> LoadAllAsync() {
        return Task.FromResult(All);
    }
}
