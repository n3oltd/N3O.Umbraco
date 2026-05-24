using N3O.Umbraco.Mediator;
using System;

namespace N3O.Umbraco.Scheduler;

public static class TriggerKey {
    private static readonly string Separator = "|";
    
    public static readonly string ApiSecurityKey = Guid.NewGuid().ToString();

    public static string Generate<TRequest, TModel>() where TRequest : Request<TModel, None> {
        return Generate(typeof(TRequest), typeof(TModel));
    }

    public static string Generate(Type requestType, Type modelType) {
        return $"{requestType.AssemblyQualifiedName}{Separator}{modelType.AssemblyQualifiedName}";
    }

    public static Type ParseRequestType(string triggerId) {
        return Parse(triggerId).RequestType;
    }

    public static Type ParseModelType(string triggerId) {
        return Parse(triggerId).ModelType;
    }

    public static (Type RequestType, Type ModelType) Parse(string triggerKey) {
        var bits = triggerKey.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
        var requestType = ResolveType(bits[0]);
        var modelType = ResolveType(bits[1]);

        return (requestType, modelType);
    }

    private static Type ResolveType(string assemblyQualifiedName) {
        var type = Type.GetType(assemblyQualifiedName);

        if (type == null) {
            throw new InvalidOperationException(
                $"Could not resolve type '{assemblyQualifiedName}' referenced by a scheduled trigger. " +
                $"This usually means a stale recurring job — the assembly is no longer deployed. " +
                $"Remove the orphaned job from Hangfire (dashboard or [HangFire].[Set]/[Hash] tables).");
        }

        return type;
    }
}
