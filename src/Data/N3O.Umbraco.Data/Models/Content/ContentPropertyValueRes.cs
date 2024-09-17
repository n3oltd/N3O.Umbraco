using N3O.Umbraco.Data.Lookups;

namespace N3O.Umbraco.Data.Models; 

public class ContentPropertyValueRes {
    public string Alias { get; set; }
    public PropertyType Type { get; set; }
    public BooleanValueRes Boolean { get; set; }
    public CropperValueRes Cropper { get; set; }
    public DateTimeValueRes DateTime { get; set; }
    public NestedValueRes Nested { get; set; }
    public NumericValueRes Numeric { get; set; }
    public RawValueRes Raw { get; set; }
    public TextareaValueRes Textarea { get; set; }
    public TextBoxValueRes TextBox { get; set; }
}