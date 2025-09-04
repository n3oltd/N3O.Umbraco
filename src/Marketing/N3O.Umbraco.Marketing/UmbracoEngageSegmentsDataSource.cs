using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Community.Contentment.DataEditors;
using Umbraco.Engage.Infrastructure.Personalization.Segments;

namespace N3O.Umbraco.Marketing;

public class UmbracoEngageSegmentsDataSource : IContentmentDataSource  {
    private readonly ISegmentRepository _segmentRepository;

    public UmbracoEngageSegmentsDataSource(ISegmentRepository segmentRepository) {
        _segmentRepository = segmentRepository;
    }

    public string Name => "Umbraco Engage Segments";
    public string Description => "Data source for Umbraco Engage segments";
    public string Icon => "icon-people";
    public Dictionary<string, object> DefaultValues => null;
    public IEnumerable<ConfigurationField> Fields => null;
    public string Group => "N3O";
    public OverlaySize OverlaySize => OverlaySize.Small;

    public IEnumerable<DataListItem> GetItems(Dictionary<string, object> config) {
        return _segmentRepository.GetAll().Select(ToDataListItem).ToList();
    }

    public Type GetValueType(Dictionary<string, object> config) {
        return typeof(Segment);
    }

    public object ConvertValue(Type type, string value) {
        var id = long.Parse(value, CultureInfo.InvariantCulture);

        return _segmentRepository.GetById(id);
    }

    private DataListItem ToDataListItem(Segment segment) {
        var dataListItem = new DataListItem();
        dataListItem.Name = segment.Name;
        dataListItem.Description = segment.Description;
        dataListItem.Icon = "icon-male-and-female";
        dataListItem.Value = segment.Id.ToString(CultureInfo.InvariantCulture);
        dataListItem.Group = "N3O";

        return dataListItem;
    }
}