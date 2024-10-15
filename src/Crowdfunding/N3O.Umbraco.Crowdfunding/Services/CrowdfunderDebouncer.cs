using N3O.Umbraco.Crm.Lookups;
using System;
using System.Collections.Concurrent;
using System.Timers;

namespace N3O.Umbraco.Crowdfunding;

public static class CrowdfunderDebouncer {
    private static readonly ConcurrentDictionary<Guid, Timer> PendingTriggers = new();
    
    public static void Debounce(Guid id, CrowdfunderType type, Action<Guid, CrowdfunderType> callback) {
        PendingTriggers.AddOrUpdate(id,
                                    _ => CreateAndStartTimer(id, type, callback),
                                    (_, existingTimer) => {
                                        existingTimer.Stop();
                                        existingTimer.Start();
                                
                                        return existingTimer;
                                    });
    }

    private static Timer CreateAndStartTimer(Guid id, CrowdfunderType type, Action<Guid, CrowdfunderType> callback) {
        var timer = new Timer(60000);
        timer.AutoReset = false;
        
        timer.Elapsed += (_, _) => {
            try {
                callback.Invoke(id, type);
            } finally {
                PendingTriggers.TryRemove(id, out _);
                
                timer.Dispose();
            }
        };
        
        timer.Start();
        
        return timer;
    }
}