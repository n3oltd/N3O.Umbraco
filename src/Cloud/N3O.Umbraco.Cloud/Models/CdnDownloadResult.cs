using NodaTime;

namespace N3O.Umbraco.Cloud.Models;

public class CdnDownloadResult {
    private static readonly Duration MaxAge = Duration.FromMinutes(5);
    private static readonly Duration RetryInterval = Duration.FromMinutes(1);
    
    private CdnDownloadResult(bool success, string content, Instant timestamp) {
        Success = success;
        Content = content;
        Timestamp = timestamp;
    }

    public bool Success { get; }
    public string Content { get; }
    public Instant Timestamp { get; }

    public bool CanRetry(IClock clock) {
        if (Success) {
            return false;
        } else {
            var age = clock.GetCurrentInstant() - Timestamp;

            return age > RetryInterval;
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

    public static CdnDownloadResult ForFailure(IClock clock) {
        return new CdnDownloadResult(false, null, clock.GetCurrentInstant());
    }
    
    public static CdnDownloadResult ForSuccess(IClock clock, string content) {
        return new CdnDownloadResult(true, content, clock.GetCurrentInstant());
    }
}