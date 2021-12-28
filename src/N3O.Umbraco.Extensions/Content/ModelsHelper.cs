using Humanizer;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace N3O.Umbraco.Content; 

public class ModelsHelper {
    private static readonly ConcurrentDictionary<string, Type> TypeCache = new();
    
    public static Type GetOrCreateModelsBuilderType(string modelsNamespace, string contentType) {
        var typeName = $"{modelsNamespace}.{contentType.Pascalize()}";

        return GetOrCreateType(typeName);
    }
    
    private static Type GetOrCreateType(string typeName) {
        return TypeCache.GetOrAdd(typeName, () => {
            var type = OurAssemblies.GetTypes(t => t.FullName == typeName).SingleOrDefault();

            if (type == null) {
                var assemblyName = new AssemblyName($"assembly_{Guid.NewGuid()}");
                var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

                var moduleBuilder = assemblyBuilder.DefineDynamicModule($"module_{Guid.NewGuid()}");

                var typeAttributes = TypeAttributes.AnsiClass |
                                     TypeAttributes.AutoClass |
                                     TypeAttributes.Class |
                                     TypeAttributes.ExplicitLayout |
                                     TypeAttributes.Public;

                var typeBuilder = moduleBuilder.DefineType(typeName, typeAttributes, null);

                type = typeBuilder.CreateType();
            }

            return type;
        });
    }
}