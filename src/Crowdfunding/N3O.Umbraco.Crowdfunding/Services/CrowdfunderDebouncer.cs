using N3O.Umbraco.Crm.Lookups;
using System;
using System.Collections.Concurrent;
using System.Timers;

namespace N3O.Umbraco.Crowdfunding;

public static class CrowdfunderDebouncer {
    private static readonly ConcurrentDictionary<Guid, Timer> HeldCrowdfunders = new();
    
    public static void Enqueue(Guid id, CrowdfunderType crowdfunderType, Action<Guid, CrowdfunderType> enqueueCallback) {
        HeldCrowdfunders.AddOrUpdate(id,
                                     _ => CreateAndStartTimer(id, crowdfunderType, enqueueCallback),
                                     (_, existingTimer) => {
                                         existingTimer.Stop();
                                         existingTimer.Start();
                                
                                         return existingTimer;
                                     });
    }

    private static Timer CreateAndStartTimer(Guid id, CrowdfunderType crowdfunderType, Action<Guid, CrowdfunderType> enqueueCallback) {
        var timer = new Timer(60000);
        timer.AutoReset = false;
        
        timer.Elapsed += (_, _) => {
            try {
                enqueueCallback.Invoke(id, crowdfunderType);
            } 
            finally {
                HeldCrowdfunders.TryRemove(id, out _);
                
                timer.Dispose();
            }
        };
        
        timer.Start();
        
        return timer;
    }
}