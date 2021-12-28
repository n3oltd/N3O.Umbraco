using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Cropper.DataTypes;

public class CropperConfigurationEditor : ConfigurationEditor<CropperConfiguration> {
    public CropperConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }
}
