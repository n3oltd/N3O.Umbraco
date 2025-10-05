using N3O.Umbraco.Extensions;
using N3O.Umbraco.Sync.Extensions.Attributes;
using N3O.Umbraco.Sync.Extensions.Models;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace N3O.Umbraco.Sync.Extensions;

public static class DataSync {
    private static readonly Dictionary<string, SyncRegistration> Registrations = new();

    public static void OnDemand<T, TProducer, TConsumer>(string sharedSecret)
        where T : new()
        where TProducer : IDataSyncProducer<T>
        where TConsumer : IDataSyncConsumer<T> {
        Register<T, TProducer, TConsumer>(null, sharedSecret);
    }
    
    public static void Schedule<T, TProducer, TConsumer>(Duration interval, string sharedSecret)
        where T : new()
        where TProducer : IDataSyncProducer<T>
        where TConsumer : IDataSyncConsumer<T> {
        Register<T, TProducer, TConsumer>(interval, sharedSecret);
    }

    public static SyncRegistration GetRegistrationByProviderId(string providerId) {
        return Registrations.TryGetValue(providerId, out var registration) ? registration : null;
    }
    
    public static IEnumerable<SyncRegistration> GetAllRegistrations() {
        return Registrations.Select(x => x.Value);
    }
    
    private static void Register<T, TProducer, TConsumer>(Duration? interval, string sharedSecret)
        where T : new()
        where TProducer : IDataSyncProducer<T>
        where TConsumer : IDataSyncConsumer<T> {
        var providerId = GetProviderId<TProducer>();
        
        if (providerId != GetProviderId<TConsumer>()) {
            throw new Exception($"Types {typeof(TProducer).GetFriendlyName()} and {typeof(TConsumer).GetFriendlyName()} must have the same provider ID");
        }

        if (interval.HasValue() && interval < Duration.FromMinutes(5)) {
            throw new Exception("Interval must be greater than 5 minutes");
        }

        var registrations = new SyncRegistration(providerId,
                                                 sharedSecret,
                                                 interval,
                                                 typeof(TProducer),
                                                 typeof(TConsumer),
                                                 typeof(T),
                                                 null);
        
        Registrations[providerId] = registrations;
    }

    private static string GetProviderId<T>() {
        var providerAttribute = typeof(T).GetCustomAttribute<DataSyncProviderAttribute>();
        
        if (providerAttribute == null) {
            throw new Exception($"Type {typeof(T).GetFriendlyName()} is missing a required {nameof(DataSyncProviderAttribute)}");
        }

        return providerAttribute.Id;
    }
}