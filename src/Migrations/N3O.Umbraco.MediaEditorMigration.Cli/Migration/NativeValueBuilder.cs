using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace N3O.Umbraco.MediaEditorMigration.Cli;

// Builds the native Umbraco JSON shapes for both migration targets. Pure functions — no DB access.
//
//   --target inline      Umbraco.ImageCropper / Umbraco.UploadField. Both store the file path ON THE PROPERTY
//                        with no media library node, exactly as the retired N3O editors did, so nothing is
//                        created and every path carries over verbatim.
//   --target mediapicker Umbraco.MediaPicker3, which references the media library by GUID, so each file is
//                        first registered as a media node (see MediaNodeFactory) and the value holds its key.
public static class NativeValueBuilder {
    // The Umbraco.ImageCropper stored property value. Returns the serialised JSON plus what the caller must
    // flag for review — see BuildCrops for the two ways a crop can come out imperfect.
    //
    // Crop width/height are written for fidelity, but Umbraco's ImageCropperConfigurationExtensions
    // .ApplyConfiguration rebuilds the crop list from the DATA TYPE config on read, matching by alias and
    // keeping only the coordinates. So a crop alias that is not in the data type config is discarded on read,
    // which is why the same cropDefinitions must be written to both the value and the config.
    public static (string Json, CropOutcome Crops) BuildImageCropperValue(
        SourceFile file, IReadOnlyList<CropDefinition> cropDefinitions) {

        var (crops, outcome) = BuildCrops(file, cropDefinitions);

        // focalPoint is left null: the retired Cropper had no focal point, and a null one makes a crop without
        // coordinates fall back to a centre crop rather than to an invented focal point.
        var value = new JObject {
            ["src"] = file.Src,
            ["crops"] = crops,
            ["focalPoint"] = null
        };

        // Umbraco.ImageCropper has no alt-text slot, and with the file on the property there is no media node
        // whose name could carry it, so the original text is preserved as a non-standard member instead of
        // being thrown away. ImageCropperValue ignores members it does not know, so this is inert to Umbraco;
        // N3O.Umbraco.Extensions' IPublishedElement.AltText(alias) reads it back off the raw source value.
        //
        // This is migration continuity, not a permanent home: it survives publishing, but an editor saving the
        // property in the backoffice makes Umbraco re-serialise from ImageCropperValue and the text is lost.
        if (!string.IsNullOrWhiteSpace(file.AltText)) {
            value["altText"] = file.AltText;
        }

        return (JsonConvert.SerializeObject(value), outcome);
    }

    // The data type config when flipping to Umbraco.ImageCropper: ImageCropperConfiguration has a single
    // [ConfigurationField("crops")] of {alias, width, height}.
    public static string BuildImageCropperConfig(IReadOnlyList<CropDefinition> cropDefinitions) {
        var crops = new JArray();

        foreach (var def in cropDefinitions) {
            crops.Add(new JObject {
                ["alias"] = def.Alias,
                ["width"] = def.Width,
                ["height"] = def.Height
            });
        }

        return JsonConvert.SerializeObject(new JObject { ["crops"] = crops });
    }

    // The data type config when flipping to Umbraco.UploadField. The retired Uploader stored its restriction as
    // a comma-separated, dot-prefixed string (".png, .jpg"); the native editor wants a bare lower-case
    // array (["png","jpg"]), matching Umbraco's own built-in upload data types. An empty array means
    // "no restriction", which is what an unset allowedExtensions meant on the old editor.
    //
    // maxFileSizeMb, imagesOnly and altTextRequired have no equivalent on the native editor and are dropped.
    public static string BuildUploadFieldConfig(string allowedExtensions) {
        var extensions = new JArray();

        foreach (var extension in ParseExtensions(allowedExtensions)) {
            extensions.Add(extension);
        }

        return JsonConvert.SerializeObject(new JObject { ["fileExtensions"] = extensions });
    }

    // Shared by both targets: Umbraco.ImageCropper's crops and Umbraco.MediaPicker3's per-item local crops are
    // the same ImageCropperCrop shape, so one builder serves both.
    //
    // The retired Cropper stored its rectangles POSITIONALLY — the i-th rectangle belongs to the i-th entry of
    // the data type's cropDefinitions, with no alias on the rectangle itself. So the definitions drive the loop,
    // and a value holding more rectangles than the data type now defines has had crop definitions removed since
    // it was saved; those rectangles have no alias to be written under and are counted as dropped.
    private static (JArray Crops, CropOutcome Outcome) BuildCrops(
        SourceFile file, IReadOnlyList<CropDefinition> cropDefinitions) {

        var outcome = new CropOutcome();
        var crops = new JArray();

        // Map each positional crop rectangle to its crop definition (alias/size) and convert px → fractions.
        for (var i = 0; i < cropDefinitions.Count; i++) {
            var def = cropDefinitions[i];
            var crop = new JObject {
                ["alias"] = def.Alias,
                ["width"] = def.Width,
                ["height"] = def.Height
            };

            var rect = i < file.Crops.Count ? file.Crops[i] : null;
            var coordinates = ToCoordinates(rect, file.Width, file.Height);

            if (coordinates != null) {
                crop["coordinates"] = coordinates;
            } else {
                // No coordinates: the crop falls back to the focal point, or to a default centre crop when
                // there is none. Flag for a manual check only when there WAS a stored rectangle we couldn't
                // convert (missing image dimensions).
                crop["coordinates"] = null;

                if (rect != null && (rect.Width > 0 || rect.Height > 0)) {
                    outcome.WithoutCoordinates.Add(def.Alias);
                }
            }

            crops.Add(crop);
        }

        for (var i = cropDefinitions.Count; i < file.Crops.Count; i++) {
            var rect = file.Crops[i];

            if (rect.Width > 0 || rect.Height > 0) {
                outcome.DroppedRectangles++;
            }
        }

        return (crops, outcome);
    }

    private static IReadOnlyList<string> ParseExtensions(string allowedExtensions) {
        if (string.IsNullOrWhiteSpace(allowedExtensions)) {
            return Array.Empty<string>();
        }

        return allowedExtensions.Split(',')
                                .Select(x => x.Trim().TrimStart('.').ToLowerInvariant())
                                .Where(x => x.Length > 0)
                                .Distinct(StringComparer.Ordinal)
                                .ToList();
    }

    // ---------------------------------------------------------------------------------------------------
    // --target mediapicker
    // ---------------------------------------------------------------------------------------------------

    // The MediaPicker3 stored property value: a JSON array of one media item (these editors were single-value).
    // crops/focalPoint are only populated for Cropper. Returns the serialised JSON plus what the caller must
    // flag for review — see BuildCrops.
    public static (string Json, CropOutcome Crops) BuildPickerValue(
        Guid mediaKey, SourceFile file, IReadOnlyList<CropDefinition> cropDefinitions) {

        var (crops, outcome) = BuildCrops(file, cropDefinitions);

        var item = new JObject {
            ["key"] = Guid.NewGuid().ToString(),
            ["mediaKey"] = mediaKey.ToString(),
            ["crops"] = crops,
            ["focalPoint"] = null
        };

        return (JsonConvert.SerializeObject(new JArray(item)), outcome);
    }

    // The data type config when flipping to Umbraco.MediaPicker3. Single item; crops carried over for Cropper.
    public static string BuildMediaPickerConfig(IReadOnlyList<CropDefinition> cropDefinitions,
                                                bool enableLocalFocalPoint) {
        var crops = new JArray();

        foreach (var def in cropDefinitions) {
            crops.Add(new JObject {
                ["alias"] = def.Alias,
                ["width"] = def.Width,
                ["height"] = def.Height
            });
        }

        var config = new JObject {
            ["filter"] = "",
            ["multiple"] = false,
            ["startNodeId"] = null,
            ["ignoreUserStartNodes"] = false,
            ["enableLocalFocalPoint"] = enableLocalFocalPoint,
            ["crops"] = crops,
            ["validationLimit"] = new JObject { ["min"] = 0, ["max"] = 1 }
        };

        return JsonConvert.SerializeObject(config);
    }

    // The umbracoFile value stored on a new media node. Image media uses the ImageCropper editor (JSON with a
    // default centre focal point); File media uses the Upload editor (a plain path string).
    public static string BuildUmbracoFileValue(SourceFile file) {
        if (!file.IsImage) {
            return file.Src;
        }

        var value = new JObject {
            ["src"] = file.Src,
            ["crops"] = new JArray(),
            ["focalPoint"] = new JObject { ["left"] = 0.5, ["top"] = 0.5 }
        };

        return JsonConvert.SerializeObject(value);
    }

    // ---------------------------------------------------------------------------------------------------

    // Absolute pixel rectangle → relative-fraction coordinates {x1,y1,x2,y2} in [0,1]. These are INSETS from
    // each edge, not corners: Umbraco passes them straight to ImageSharp's crop processor as
    // "left,top,right,bottom". Null when the rectangle or the source image dimensions are missing/zero (can't
    // convert without the original width/height).
    private static JObject ToCoordinates(CropRect rect, int? imageWidth, int? imageHeight) {
        if (rect == null || imageWidth is not > 0 || imageHeight is not > 0 || rect.Width <= 0 || rect.Height <= 0) {
            return null;
        }

        double w = imageWidth.Value;
        double h = imageHeight.Value;

        return new JObject {
            ["x1"] = Clamp(rect.X / w),
            ["y1"] = Clamp(rect.Y / h),
            ["x2"] = Clamp((double) (imageWidth.Value - (rect.X + rect.Width)) / w),
            ["y2"] = Clamp((double) (imageHeight.Value - (rect.Y + rect.Height)) / h)
        };
    }

    private static double Clamp(double value) {
        if (value < 0) return 0;
        if (value > 1) return 1;

        return Math.Round(value, 10);
    }
}
