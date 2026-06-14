using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Converters;

public class LatestStateContentMetadataConverter : ContentMetadataConverter<string> {
    private readonly IAuditService _auditService;

    public LatestStateContentMetadataConverter(IColumnRangeBuilder columnRangeBuilder, IAuditService auditService)
        : base(columnRangeBuilder, ContentMetadatas.LatestState) {
        _auditService = auditService;
    }

    public override object GetValue(IContent content) {
        // GetItemsByEntityAsync replaces the deprecated GetLogs(int) (removed in Umbraco 19).
        // skip=0, take=1, Descending returns the most recent entry directly.
        // GetAwaiter().GetResult() is safe: no SynchronizationContext on ASP.NET Core thread pool.
        var latestAuditLog = _auditService.GetItemsByEntityAsync(content.Id, 0, 1, Direction.Descending, null, null)
                                           .GetAwaiter()
                                           .GetResult()
                                           .Items
                                           .FirstOrDefault();

        return latestAuditLog?.AuditType.ToString();
    }
    
    protected override string Title => "Latest State";
}
