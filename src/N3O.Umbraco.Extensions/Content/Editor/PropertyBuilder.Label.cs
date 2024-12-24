using System;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class LabelPropertyBuilder : PropertyBuilder {
    public LabelPropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }
    
    public void Set(DateTime? value) {
        Value = value;
    }

    public void Set(decimal? value) {
        Value = value;
    }

    public void Set(int? value) {
        Value = value;
    }

    public void Set(long? value) {
        Value = value;
    }

    public void Set(string value) {
        Value = value;
    }
}
