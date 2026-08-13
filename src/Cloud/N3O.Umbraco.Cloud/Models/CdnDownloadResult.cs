using NodaTime;

namespace N3O.Umbraco.Cloud.Models;

public class CdnDownloadResult {
    private static readonly Duration MaxAge = Duration.FromMinutes(5);
    private static readonly Duration NotFoundRetryInterval = Duration.FromMinutes(15);
    private static readonly Duration ErrorRetryInterval = Duration.FromSeconds(10);

    private CdnDownloadResult(bool success, bool error, string content, Instant timestamp) {
        Success = success;
        Error = error;
        Content = content;
        Timestamp = timestamp;
    }

    public bool Success { get; }
    public bool Error { get; }
    public string Content { get; }
    public Instant Timestamp { get; }

    public bool CanRetry(IClock clock) {
        if (Success) {
            return false;
        } else {
            var age = clock.GetCurrentInstant() - Timestamp;

            return age > (Error ? ErrorRetryInterval : NotFoundRetryInterval);
        }
    }
    
    public bool IsExpired(IClock clock) {
        if (Success) {
            var age = clock.GetCurrentInstant() - Timestamp;

            return age > MaxAge;
        } else {
            return false;
        }
    }
    
    public bool WasInvalidated(Instant? invalidatedAt) {
        if (invalidatedAt == null) {
            return false;
        }
        
        return Timestamp <= invalidatedAt.Value;
    }

    public static CdnDownloadResult ForNotFound(Instant startedAt) {
        return new CdnDownloadResult(false, false, null, startedAt);
    }

    public static CdnDownloadResult ForError(Instant startedAt) {
        return new CdnDownloadResult(false, true, null, startedAt);
    }

    public static CdnDownloadResult ForSuccess(Instant startedAt, string content) {
        return new CdnDownloadResult(true, false, content, startedAt);
    }
}