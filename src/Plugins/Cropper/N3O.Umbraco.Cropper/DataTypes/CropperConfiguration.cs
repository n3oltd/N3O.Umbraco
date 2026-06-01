using System.Collections.Generic;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Cropper.DataTypes;

public class CropperConfiguration {
    [ConfigurationField("altText")]
    public bool AltText { get; set; }

    [ConfigurationField("cropDefinitions")]
    public IEnumerable<CropDefinition> CropDefinitions { get; set; }
}
