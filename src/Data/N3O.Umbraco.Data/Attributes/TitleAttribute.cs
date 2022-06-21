using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Data.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class TitleAttribute : Attribute {
    public TitleAttribute(string text, bool doNotLocalize = false) {
        Text = text;
        DoNotLocalize = doNotLocalize;
    }

    public string Text { get; }
    public bool DoNotLocalize { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public class TitleFromMetadataAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property)]
public class TitleFromValueAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property)]
public class CustomTitleAttribute : Attribute {
    public CustomTitleAttribute(Type type) {
        if (type.ImplementsInterface(typeof(IColumnHeading))) {
            throw new Exception($"{type.FullName.Quote()} must implement {nameof(IColumnHeading)}");
        }

        Type = type;
    }

    public Type Type { get; }
}
