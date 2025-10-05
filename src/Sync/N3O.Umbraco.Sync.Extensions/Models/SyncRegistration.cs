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
                            Type dataType,
                            DateTime? lastRunUtc) {
        ProviderId = providerId;
        SharedSecret = sharedSecret;
        AutoSyncInterval = autoSyncInterval;
        ProducerType = producerType;
        ConsumerType = consumerType;
        DataType = dataType;
        LastRunUtc = lastRunUtc;
    }

    public string ProviderId { get; }
    public string SharedSecret { get; }
    public Duration? AutoSyncInterval { get; }
    public Type ProducerType { get; }
    public Type ConsumerType { get; }
    public Type DataType { get; }
    public DateTime? LastRunUtc { get; private set; }
    
    public bool IsDue() {
        if (AutoSyncInterval == null) {
            return false;
        }

        if (LastRunUtc == null) {
            return true;
        }

        var nextRun = LastRunUtc.Value.Add(AutoSyncInterval.Value.ToTimeSpan());
        
        return DateTime.UtcNow >= nextRun;
    }

    public void MarkSynchronized() {
        LastRunUtc = DateTime.UtcNow;
    }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return ProviderId;
        yield return SharedSecret;
        yield return AutoSyncInterval;
        yield return ProducerType;
        yield return ConsumerType;
        yield return DataType;
        yield return LastRunUtc;
    }
}