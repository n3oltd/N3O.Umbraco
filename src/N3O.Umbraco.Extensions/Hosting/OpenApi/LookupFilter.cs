using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Utilities;
using NJsonSchema.Generation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Hosting;

public class LookupFilter : TypeTransformationFilter {
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<string>> LookupIds = new();

    protected override void DoProcess(SchemaProcessorContext context) {
        var type = context.ContextualType.Type;

        if (type.ImplementsInterface<ILookup>()) {
            var lookupIds = LookupIds.GetOrAdd(type, _ => GetLookupIds(type));

            ModelAsEnum(type, lookupIds, lookupIds?.FirstOrDefault());
        }
    }

    private IReadOnlyList<string> GetLookupIds(Type lookupType) {
        var lookupIds = new List<string>();
        var lookupCollectionTypes = GetLookupCollectionTypes(lookupType);
        var isStatic = lookupCollectionTypes.All(x => x.HasAttribute<StaticLookupsAttribute>());

        if (isStatic) {
            lookupCollectionTypes.SelectMany(StaticLookups.GetAll<ILookup>)
                                 .Do(x => lookupIds.Add(x.Id));
        }

        return lookupIds;
    }
    
    private IReadOnlyList<Type> GetLookupCollectionTypes(Type lookupType) {
        var interfaceType = typeof(ILookupsCollection<>).MakeGenericType(lookupType);
        
        var lookupCollectionTypes = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                                t.ImplementsInterface(interfaceType))
                                                 .ToList();

        return lookupCollectionTypes;
    }
}
