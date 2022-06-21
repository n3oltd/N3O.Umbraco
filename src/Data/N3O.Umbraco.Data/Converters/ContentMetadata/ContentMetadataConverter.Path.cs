using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Converters;

public class PathContentMetadataConverter : ContentMetadataConverter<string> {
    private readonly Dictionary<int, string> _pathCache = new();
    private readonly IContentService _contentService;

    public PathContentMetadataConverter(IColumnRangeBuilder columnRangeBuilder, IContentService contentService)
        : base(columnRangeBuilder, ContentMetadatas.Path) {
        _contentService = contentService;
    }

    public override object GetValue(IContent content) {
        var parentId = content.ParentId;
        
        return _pathCache.GetOrAdd(parentId, () => {
            var segments = new List<string>();

            while (parentId != -1) {
                var parent = _contentService.GetById(parentId);

                segments.Add(parent.Name);
        
                parentId = parent.ParentId;
            }

            segments.Reverse();
    
            return string.Join(" // ", segments);
        });
    }
    
    protected override string Title => "Path";
}
