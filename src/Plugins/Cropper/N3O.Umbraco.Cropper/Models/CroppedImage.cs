using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace N3O.Umbraco.Cropper.Models;

public class CroppedImage : DynamicObject, IEnumerable<ImageCrop> {
    private readonly Dictionary<string, ImageCrop> _crops = new(StringComparer.InvariantCultureIgnoreCase);

    public CroppedImage(CropperConfiguration configuration, CropperSource source) {
        AltText = source.AltText;
        
        foreach (var (crop, index) in source.Crops.SelectWithIndex()) {
            var definition = configuration.CropDefinitions.ElementAtOrDefault(index);

            if (definition != null) {
                AddImageCrop(definition, source.MediaId, source.Filename, crop);
            }
        }
    }

    public bool HasCrop(string alias) {
        return _crops.ContainsKey(alias);
    }
    
    public string AltText { get; }
    public ImageCrop Crop => this.Single();

    public override bool TryGetMember(GetMemberBinder binder, out object result) {
        var alias = binder.Name;
        var exists = _crops.ContainsKey(alias);
        var crop = exists ? _crops[alias] : null;

        result = crop;

        return exists;
    }

    public IEnumerator<ImageCrop> GetEnumerator() {
        return _crops.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return _crops.Values.GetEnumerator();
    }

    public ImageCrop this[string alias] {
        get => _crops[alias];

        set => _crops[alias] = value;
    }
    
    private void AddImageCrop(CropDefinition definition, string mediaId, string sourceFile, CropperSource.Crop crop) {
        var imageCrop = new ImageCrop(definition.Alias,
                                      ImagePath.Get(mediaId, sourceFile, definition, crop),
                                      definition.Height,
                                      definition.Width);

        _crops[definition.Alias] = imageCrop;
    }
}
