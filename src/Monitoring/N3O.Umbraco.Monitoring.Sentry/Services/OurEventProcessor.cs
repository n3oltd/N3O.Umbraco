using N3O.Umbraco.Extensions;
using N3O.Umbraco.Logging;
using Sentry;
using Sentry.Extensibility;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Monitoring.Sentry;

public class OurEventProcessor : ISentryEventProcessor {
    private readonly IReadOnlyList<ILogEnricher> _logEnrichers;

    public OurEventProcessor(IEnumerable<ILogEnricher> logEnrichers) {
        _logEnrichers = logEnrichers.OrEmpty().ToList();
    }
    
    public SentryEvent Process(SentryEvent sentryEvent) {
        foreach (var logEnricher in _logEnrichers) {
            foreach (var (key, value) in logEnricher.GetContextData()) {
                sentryEvent.Contexts.Add(key, value);
            }
            
            foreach (var (key, value) in logEnricher.GetTags()) {
                sentryEvent.Contexts.Add(key, value);
            }   
        }
        
        return sentryEvent;
    }
}