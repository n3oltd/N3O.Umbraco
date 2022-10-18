using System;

namespace N3O.Umbraco.Content;

public class LabelPropertyBuilder : PropertyBuilder {
    public void Set(string value) {
        Value = value;
    }
    
    public void Set(DateTime? value) {
        Value = value;
    }
    
    public void Set(decimal? value) {
        Value = value;
    }
}
