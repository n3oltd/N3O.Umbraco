using NodaTime;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Sync.Extensions.Models;

public class SyncRegistration : Value {
    public SyncRegistration(string providerId,
                            string sharedSecret,
                            Duration? autoSyncInterval,
                            Type producerType,
                            Type consumerType,
                            Type dataType) {
        ProviderId = providerId;
        SharedSecret = sharedSecret;
        AutoSyncInterval = autoSyncInterval;
        ProducerType = producerType;
        ConsumerType = consumerType;
        DataType = dataType;
    }

    public string ProviderId { get; }
    public string SharedSecret { get; }
    public Duration? AutoSyncInterval { get; }
    public Type ProducerType { get; }
    public Type ConsumerType { get; }
    public Type DataType { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return ProviderId;
        yield return SharedSecret;
        yield return AutoSyncInterval;
        yield return ProducerType;
        yield return ConsumerType;
        yield return DataType;
    }
}