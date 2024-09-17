using N3O.Umbraco.Attributes;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Validation;
using Newtonsoft.Json;

namespace N3O.Umbraco.Data.Models; 

public class ContentPropertyReq {
    [Name("Alias")]
    public string Alias { get; set; }
    
    [Name("Type")]
    public PropertyType Type { get; set; }
    
    [Name("Boolean")]
    public BooleanValueReq Boolean { get; set; }
    
    [Name("Cropper")]
    public CropperValueReq Cropper { get; set; }
    
    [Name("DateTime")]
    public DateTimeValueReq DateTime { get; set; }

    [Name("Nested")]
    public NestedValueReq Nested { get; set; }
    
    [Name("Numeric")]
    public NumericValueReq Numeric { get; set; }
    
    [Name("Raw")]
    public RawValueReq Raw { get; set; }
    
    [Name("Textarea")]
    public TextareaValueReq Textarea { get; set; }
    
    [Name("TextBox")]
    public TextBoxValueReq TextBox { get; set; }
    
    [JsonIgnore]
    public AutoProperty<ValueReq> Value => new(this, x => x?.Type == Type);
}