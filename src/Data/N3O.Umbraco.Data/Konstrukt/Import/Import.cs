using N3O.Umbraco.Extensions;
using NPoco;
using System;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace N3O.Umbraco.Data.Konstrukt {
    [TableName(DataConstants.Tables.Imports.Name)]
    [PrimaryKey("Id")]
    public class Import {
        [PrimaryKeyColumn(Name = DataConstants.Tables.Imports.PrimaryKey)]
        public int Id { get; set; }
        
        [Column(nameof(Reference))]
        public string Reference { get; set; }
        
        [Column(nameof(QueuedAt))]
        public DateTime QueuedAt { get; set; }

        [Column(nameof(QueuedByUser))]
        public Guid QueuedByUser { get; set; }
        
        [Column(nameof(QueuedByName))]
        public string QueuedByName { get; set; }

        [Column(nameof(Action))]
        public string Action { get; set; }
        
        [Column(nameof(BatchReference))]
        public string BatchReference { get; set; }

        [Column(nameof(BatchFilename))]
        public string BatchFilename { get; set; }
        
        [Column(nameof(FileRowNumber))]
        public int FileRowNumber { get; set; }

        [Column(nameof(ContentTypeAlias))]
        public string ContentTypeAlias { get; set; }
        
        [Column(nameof(ContentTypeName))]
        public string ContentTypeName { get; set; }
        
        [Column(nameof(ReplacesId))]
        public Guid? ReplacesId { get; set; }
        
        [Column(nameof(ParentId))]
        public Guid ParentId { get; set; }
        
        [Column(nameof(Fields))]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string Fields { get; set; }

        [Column(nameof(Errors))]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Errors { get; set; }
        
        [Column(nameof(Status))]
        public string Status { get; set; }
        
        [Column(nameof(ImportedAt))]
        public DateTime ImportedAt { get; set; }
        
        [Column(nameof(ImportedContentId))]
        [NullSetting(NullSetting = NullSettings.Null)]
        public Guid? ImportedContentId { get; set; }

        [Column(nameof(ImportedContentReference))]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string ImportedContentReference { get; set; }

        [Ignore]
        public bool CanProcess => Status.IsAnyOf(ImportStatuses.Abandoned, ImportStatuses.Queued, ImportStatuses.Error);
    }
}