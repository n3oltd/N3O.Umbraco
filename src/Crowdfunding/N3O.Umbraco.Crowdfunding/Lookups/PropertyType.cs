using N3O.Umbraco.Content;
using N3O.Umbraco.Lookups;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public abstract class PropertyType : Lookup {
    protected PropertyType(string id) : base(id) { }

    public abstract Task UpdatePropertyAsync(IContentPublisher contentPublisher, string alias, object data);
}

public abstract class PropertyType<T> : PropertyType {
    protected PropertyType(string id) : base(id) { }

    public override async Task UpdatePropertyAsync(IContentPublisher contentPublisher, string alias, object data) {
        await UpdatePropertyAsync(contentPublisher, alias, (T) data);
    }

    protected abstract Task UpdatePropertyAsync(IContentPublisher contentPublisher, string alias, T data);
}

public class PropertyTypes : StaticLookupsCollection<PropertyType> {
    public static readonly PropertyType Boolean = new BooleanPropertyType();
    public static readonly PropertyType Cropper = new CropperPropertyType();
    public static readonly PropertyType DateTime = new DateTimePropertyType();
    public static readonly PropertyType Numeric = new NumericPropertyType();
    public static readonly PropertyType Raw = new RawPropertyType();
    public static readonly PropertyType Textarea = new TextareaPropertyType();
    public static readonly PropertyType TextBox = new TextBoxPropertyType();
}