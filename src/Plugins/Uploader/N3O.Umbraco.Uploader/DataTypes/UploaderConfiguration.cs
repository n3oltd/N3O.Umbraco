using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Uploader.DataTypes;

public class UploaderConfiguration {
    [ConfigurationField("allowedExtensions",
                        "Allowed Extensions",
                        "requiredfield",
                        Description = "Sets the comma-separated list of allowed file extensions (.ext1, .ext2 etc.)")]
    public string AllowedExtensions { get; set; }

    [ConfigurationField("altTextRequired",
                        "Alt Text Required",
                        "boolean",
                        Description = "Indicates if alt text is mandatory")]
    public bool AltTextRequired { get; set; }
    
    [ConfigurationField("imagesOnly",
                        "Images Only",
                        "boolean",
                        Description = "Tick to require all uploaded files are valid images and show alt text field and image preview in UI")]
    public bool ImagesOnly { get; set; }
    
    [ConfigurationField("maxFileSizeMb",
                        "Maximum File Size (MB)",
                        "requiredfield",
                        Description = "Sets the maximum allowed file size in megabytes")]
    public string MaxFileSizeMb { get; set; }
    
    [ConfigurationField("maxImageHeight",
                        "Maximum image height (optional)",
                        "textstring",
                        Description = "Sets the maximum height of uploaded images")]
    public string MaxImageHeight { get; set; }
    
    [ConfigurationField("maxImageWidth",
                        "Maximum image width (optional)",
                        "textstring",
                        Description = "Sets the maximum width of uploaded images")]
    public string MaxImageWidth { get; set; }

    [ConfigurationField("minImageHeight",
                        "Minimum image height (optional)",
                        "textstring",
                        Description = "Sets the minimum height of uploaded images")]
    public string MinImageHeight { get; set; }
    
    [ConfigurationField("minImageWidth",
                        "Minimum image width (optional)",
                        "textstring",
                        Description = "Sets the minimum width of uploaded images")]
    public string MinImageWidth { get; set; }
}
