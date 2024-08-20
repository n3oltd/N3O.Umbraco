using N3O.Umbraco.Crowdfunding.Lookups;

namespace N3O.Umbraco.Crowdfunding.Models; 

public class ContentPropertyRes {
    public string Alias { get; set; }
    public PropertyType Type { get; set; }
    public ContentPropertyValueRes PropertyValue { get; set; }
    public ContentPropertyCriteriaRes PropertyCriteria { get; set; }
}