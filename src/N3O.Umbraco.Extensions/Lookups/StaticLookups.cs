using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace N3O.Umbraco.Lookups {
    public static class StaticLookups {
        public static IReadOnlyList<TLookup> GetAll<T, TLookup>() where TLookup : ILookup {
            return GetAll<TLookup>(typeof(T));
        }

        public static IReadOnlyList<TLookup> GetAll<TLookup>() where TLookup : ILookup {
            var staticLookupsType = typeof(StaticLookupsCollection<>).MakeGenericType(typeof(TLookup));
        
            var staticType = OurAssemblies.GetTypes(t => t.IsConcreteClass() && t.IsSubclassOf(staticLookupsType))
                                          .SingleOrDefault();

            if (staticType == null) {
                throw new Exception($"Could not find a single class inheriting from {staticLookupsType.FullName}");
            }

            return GetAll<TLookup>(staticType);
        }
    
        public static IReadOnlyList<TLookup> GetAll<TLookup>(Type staticType)
            where TLookup : ILookup {
            var fields = staticType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                                   .Select(f => f.GetValue(null))
                                   .ToList();

            var list = fields.Where(f => f is TLookup).Cast<TLookup>().ToList();

            return list;
        }
    }
}
