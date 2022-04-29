namespace N3O.Umbraco.Storage.Extensions {
    public static class BlobExtensions {
        public static bool IsTempBlob(this Blob blob) {
            return blob?.StorageFolderName == StorageConstants.StorageFolders.Temp;
        }
    }
}