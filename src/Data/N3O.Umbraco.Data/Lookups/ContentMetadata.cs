using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Data.Lookups {
    public class ContentMetadata : NamedLookup {
        public ContentMetadata(string id, string name, DataType dataType, int displayOrder, bool autoSelected)
            : base(id, name) {
            DataType = dataType;
            AutoSelected = autoSelected;
            DisplayOrder = displayOrder;
        }
        
        public DataType DataType { get; }
        public bool AutoSelected { get; }
        public int DisplayOrder { get; }
    }

    [StaticLookups]
    public class ContentMetadatas : StaticLookupsCollection<ContentMetadata> {
        public static readonly ContentMetadata CreatedAt = new("createdAt", "Created At", DataTypes.DateTime, 5, false);
        public static readonly ContentMetadata CreatedBy = new("createdBy", "Created By", DataTypes.String, 6, false);
        public static readonly ContentMetadata EditLink = new("editLink", "Edit Link", DataTypes.String, 4, false);
        public static readonly ContentMetadata HasUnpublishedChanges = new("hasUnpublishedChanges", "Has Unpublished Changes", DataTypes.Bool, 3, false);
        public static readonly ContentMetadata IsPublished = new("isPublished", "Is Published", DataTypes.Bool, 2, true);
        public static readonly ContentMetadata Name = new("name", "Name", DataTypes.String, 0, true);
        public static readonly ContentMetadata Path = new("path", "Path", DataTypes.String, 1, true);
        public static readonly ContentMetadata UpdatedAt = new("updatedAt", "Updated At", DataTypes.DateTime, 7, false);
        public static readonly ContentMetadata UpdatedBy = new("updatedBy", "Updated By", DataTypes.String, 8, false);
    }
}