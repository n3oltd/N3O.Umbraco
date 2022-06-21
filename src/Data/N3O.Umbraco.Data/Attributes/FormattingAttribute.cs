using N3O.Umbraco.Data.Models;
using System;

namespace N3O.Umbraco.Data.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public abstract class FormattingAttribute : Attribute {
    public abstract void ApplyFormatting(ExcelFormatting formatting);
}
