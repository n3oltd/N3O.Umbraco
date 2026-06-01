using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Uploader.DataTypes;

public class UploaderConfiguration {
    [ConfigurationField("allowedExtensions")]
    public string AllowedExtensions { get; set; }

    [ConfigurationField("altTextRequired")]
    public bool AltTextRequired { get; set; }

    [ConfigurationField("imagesOnly")]
    public bool ImagesOnly { get; set; }

    [ConfigurationField("maxFileSizeMb")]
    public string MaxFileSizeMb { get; set; }

    [ConfigurationField("maxImageHeight")]
    public string MaxImageHeight { get; set; }

    [ConfigurationField("maxImageWidth")]
    public string MaxImageWidth { get; set; }

    [ConfigurationField("minImageHeight")]
    public string MinImageHeight { get; set; }

    [ConfigurationField("minImageWidth")]
    public string MinImageWidth { get; set; }
}
