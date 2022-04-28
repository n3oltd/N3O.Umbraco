using System;

namespace N3O.Umbraco.Data.Konstrukt {
    public partial class Import {
        public void PublishingFailed(Guid id, string reference) {
            ImportedContentId = id;
            Reference = reference;
            Errors = null;
            Status = ImportStatuses.ImportedWithWarning;
        }
    }
}