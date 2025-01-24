using NodaTime;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class JobResult : Value {
    public JobResult(Instant completedAt, bool success, JobError error, WebhookCrowdfunderInfo crowdfunderInfo) {
        CompletedAt = completedAt;
        Success = success;
        Error = error;
        CrowdfunderInfo = crowdfunderInfo;
    }
    
    public Instant CompletedAt { get; }
    public bool Success { get; }
    public JobError Error { get; }
    public WebhookCrowdfunderInfo CrowdfunderInfo { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return CompletedAt;
        yield return Success;
        yield return Error;
        yield return CrowdfunderInfo;
    }
}