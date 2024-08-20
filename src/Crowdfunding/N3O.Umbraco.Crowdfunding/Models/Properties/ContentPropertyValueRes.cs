﻿namespace N3O.Umbraco.Crowdfunding.Models; 

public class ContentPropertyValueRes {
    public BooleanValueRes Boolean { get; set; }
    public CropperValueRes Cropper { get; set; }
    public DateTimeValueRes DateTime { get; set; }
    public NestedValueRes Nested { get; set; }
    public NumericValueRes Numeric { get; set; }
    public RawValueRes Raw { get; set; }
    public TextareaValueRes Textarea { get; set; }
    public TextBoxValueRes TextBox { get; set; }
}