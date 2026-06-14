using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using System.Linq;
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
        // TODO Migration Review (CS0618): IAuditService.GetLogs(int) is obsolete (removed in Umbraco 19). The replacement
        // GetItemsByEntityAsync(...) is async, but this overrides the synchronous abstract
        // IContentMetadataConverter.GetValue(IContent) -> object, a public interface implemented by 11
        // converters and consumed by sync callers. Making it async would change that public interface and
        // ripple outside this project. Left as-is (warning, not error; scheduled for removal in v19).
        var latestAuditLog = _auditService.GetLogs(content.Id).OrderByDescending(x => x.CreateDate).FirstOrDefault();

        return latestAuditLog?.AuditType.ToString();
    }
    
    protected override string Title => "Latest State";
}
