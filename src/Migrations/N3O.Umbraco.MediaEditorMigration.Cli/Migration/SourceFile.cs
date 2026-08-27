using System.Collections.Generic;

namespace N3O.Umbraco.MediaEditorMigration.Cli;

// A file reference parsed from a stored N3O Cropper / Uploader property value. Both editors stored raw files on
// MediaFileManager.FileSystem at /media/{nodaTimeTicks}/{filename} with NO Umbraco media node — this captures
// everything the migration needs to register that file as a media node and rebuild the value natively.
public sealed class SourceFile {
    public string Src { get; set; }        // "/media/{ticks}/{filename}" — reused verbatim as the media file path
    public string MediaId { get; set; }    // "{ticks}" — the folder segment of Src
    public string Filename { get; set; }
    public string Extension { get; set; }  // lower-case, with leading dot (".jpg")
    public int? Width { get; set; }
    public int? Height { get; set; }
    public long? Bytes { get; set; }
    public string AltText { get; set; }
    public bool IsImage { get; set; }

    // Cropper only — the stored crop rectangles, in absolute pixels, positional (index i ⇒ the i-th crop definition).
    public List<CropRect> Crops { get; set; } = new();
}

public sealed class CropRect {
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

// A crop alias/size defined on the OLD Cropper data type config (cropDefinitions). Positionally aligned with the
// per-value Crops above, and carried onto the native MediaPicker3 data type config + each item's local crops.
public sealed class CropDefinition {
    public string Alias { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}
