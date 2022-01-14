using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups {
    public class LookupInfo : Lookup {
        public LookupInfo(string id, Type lookupType) : base(id) {
            LookupType = lookupType;
        }

        public Type LookupType { get; }
    }

    [StaticLookups]
    public class LookupTypes : LookupsCollection<LookupInfo> {
        private static readonly IReadOnlyList<LookupInfo> All;

        static LookupTypes() {
            var lookupTypesSetTypes = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                                  t.ImplementsInterface<ILookupTypesSet>())
                                                   .ToList();
            
            var list = new List<LookupInfo>();
            
            foreach (var lookupTypesSetType in lookupTypesSetTypes) {
                var fields = lookupTypesSetType.GetConstantOrStaticFields();

                foreach (var field in fields) {
                    var fieldValue = GetValue(field);

                    if (fieldValue == null) {
                        continue;
                    }
                    
                    var attribute = field.GetCustomAttribute<LookupInfoAttribute>();

                    if (attribute == null) {
                        throw new Exception($"Field {field.Name} on type {lookupTypesSetType.GetFriendlyName()} is missing the required {nameof(LookupInfoAttribute)}");
                    }
                    
                    list.Add(new LookupInfo(fieldValue, attribute.LookupType));
                }
            }

            All = list;
        }

        public override Task<IReadOnlyList<LookupInfo>> GetAllAsync() {
            return Task.FromResult(All);
        }

        private static string GetValue(FieldInfo field) {
            if (field.IsLiteral) {
                if (field.GetRawConstantValue() is string constantValue) {
                    return constantValue;
                }
            } else if (field.GetValue(null) is string staticValue) {
                return staticValue;
            }
            
            return null;
        }
    }
}