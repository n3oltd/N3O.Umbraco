namespace N3O.Umbraco.MediaEditorMigration.Cli;

// Aggregate counters for a migration run, surfaced in the summary and used to decide the exit code.
public sealed class RunTotals {
    public int DataTypesConverted { get; set; }
    public int ValuesConverted { get; set; }
    public int ValuesUnchanged { get; set; }
    public int ValuesFailed { get; set; }

    // --target mediapicker only: media library nodes created for the files the retired editors stored without one.
    public int MediaNodesCreated { get; set; }

    // Values that carried alt text. No native editor has a slot for it, so it is carried as far as each target
    // allows: the media node's name under --target mediapicker, and a non-standard "altText" member of the
    // Umbraco.ImageCropper JSON under --target inline (read back via IPublishedElement.AltText(alias)).
    public int AltTextPreserved { get; set; }

    // Values whose alt text could not be carried anywhere: Umbraco.UploadField stores a bare path string, so
    // there is nowhere to put it.
    public int AltTextDropped { get; set; }

    public int CropsWithoutCoordinates { get; set; }

    // Stored crop rectangles that had no crop definition left to name them, so they could not be carried over.
    public int CropRectanglesDropped { get; set; }

    // True when Umbraco's published-cache serializer marker was cleared, which makes Umbraco rebuild the
    // whole cache on next start. Without it the site keeps serving the pre-migration values.
    public bool PublishedCacheInvalidated { get; set; }

    // Cropper/Uploader values found nested inside a Block List / Block Grid / Perplex block rather than
    // directly in umbracoPropertyData. AliasesFixed counts entries that had no value but whose stale
    // editorAlias still had to stop naming a retired editor.
    public int NestedValuesConverted { get; set; }
    public int NestedAliasesFixed { get; set; }

    // Block List / Block Grid values still in the Umbraco 13 udi shape (properties keyed by alias, lower-case
    // "layout", "udi" instead of "key") that were rewritten to the v14+ key-based shape. These only survive
    // nested inside another editor's value, which Umbraco's own 13->17 upgrade never traverses.
    public int LegacyBlockShapesNormalized { get; set; }
}
