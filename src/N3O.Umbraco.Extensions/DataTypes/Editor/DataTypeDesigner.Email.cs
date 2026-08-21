using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.DataTypes;

public class EmailDataTypeDesigner : DataTypeDesigner {
    public EmailDataTypeDesigner(IDataTypeService dataTypeService,
                                 PropertyEditorCollection propertyEditors,
                                 IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    protected override object BuildConfiguration(IDataType existing) {
        return new EmailAddressConfiguration();
    }

    protected override string EditorAlias => UmbracoPropertyEditors.Aliases.EmailAddress;
}
