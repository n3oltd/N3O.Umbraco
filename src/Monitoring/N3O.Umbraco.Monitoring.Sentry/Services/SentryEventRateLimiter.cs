using Sentry;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace N3O.Umbraco.Monitoring.Sentry;

public static class SentryEventRateLimiter {
    private static readonly ConcurrentDictionary<string, Bucket> _buckets = new();
    private static int _maxEventsPerFingerprint = 50;
    private static TimeSpan _windowDuration = TimeSpan.FromMinutes(10);

    public static void Configure(int maxEventsPerFingerprint, TimeSpan windowDuration) {
        if (maxEventsPerFingerprint > 0) {
            _maxEventsPerFingerprint = maxEventsPerFingerprint;
        }

        if (windowDuration > TimeSpan.Zero) {
            _windowDuration = windowDuration;
        }
    }

    public static SentryEvent BeforeSend(SentryEvent e, SentryHint hint) {
        var key = GetFingerprintKey(e);

        if (string.IsNullOrEmpty(key)) {
            return e;
        }

        var now = DateTime.UtcNow;

        var bucket = _buckets.AddOrUpdate(key,
                                          _ => new Bucket(now, 1),
                                          (_, existing) => (now - existing.WindowStart) > _windowDuration
                                              ? new Bucket(now, 1)
                                              : new Bucket(existing.WindowStart, existing.Count + 1));

        if (bucket.Count > _maxEventsPerFingerprint) {
            return null;
        }

        if (bucket.Count == _maxEventsPerFingerprint) {
            e.SetTag("rate_limited", "true");
            e.SetTag("rate_limit_threshold", _maxEventsPerFingerprint.ToString());
            e.SetTag("rate_limit_window_seconds", ((int) _windowDuration.TotalSeconds).ToString());
        }

        return e;
    }

    private static string GetFingerprintKey(SentryEvent e) {
        if (e.Fingerprint != null && e.Fingerprint.Any()) {
            return string.Join("|", e.Fingerprint);
        }

        var firstException = e.SentryExceptions?.FirstOrDefault();

        if (firstException != null) {
            var value = firstException.Value ?? "";

            if (value.Length > 200) {
                value = value.Substring(0, 200);
            }

            return $"{firstException.Type}|{value}";
        }

        return e.Message?.Formatted;
    }

    private readonly record struct Bucket(DateTime WindowStart, int Count);
}
