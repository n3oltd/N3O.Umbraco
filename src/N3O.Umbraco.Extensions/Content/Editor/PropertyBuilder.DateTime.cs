using NodaTime;
using System;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class DateTimePropertyBuilder : PropertyBuilder {
    public DateTimePropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }
    
    public void SetDate(DateTime? value) {
        Value = value?.Date;
    }
    
    public void SetDate(LocalDate? value) {
        SetDate(value?.ToDateTimeUnspecified());
    }

    public void SetDate(LocalDateTime? value) {
        SetDate(value?.Date);
    }
    
    public void SetDateTime(DateTime? value) {
        Value = value;
    }
    
    public void SetDateTime(LocalDateTime? value) {
        SetDate(value?.ToDateTimeUnspecified());
    }
}
