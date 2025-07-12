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
        var latestAuditLog = _auditService.GetLogs(content.Id).OrderByDescending(x => x.CreateDate).FirstOrDefault();

        return latestAuditLog?.AuditType.ToString();
    }
    
    protected override string Title => "Latest State";
}
