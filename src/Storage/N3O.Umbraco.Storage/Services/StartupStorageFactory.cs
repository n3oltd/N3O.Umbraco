using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Linq;

namespace N3O.Umbraco.Storage;

public class StartupStorageFactory {
    public IStartupStorage Create() {
        var type = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                               t.ImplementsInterface<IStartupStorage>() &&
                                               t.HasParameterlessConstructor())
                                .ApplyAttributeOrdering()
                                .First();

        return (IStartupStorage) Activator.CreateInstance(type);
    }
}