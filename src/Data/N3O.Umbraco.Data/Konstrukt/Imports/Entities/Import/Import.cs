using N3O.Umbraco.Extensions;
using NPoco;
using System;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace N3O.Umbraco.Data.Konstrukt;

[TableName(DataConstants.Tables.Imports.Name)]
[PrimaryKey("Id")]
public partial class Import {
    [PrimaryKeyColumn(Name = DataConstants.Tables.Imports.PrimaryKey)]
    public int Id { get; set; }
    
    [Column(nameof(Reference))]
    public string Reference { get; set; }

    [Column(nameof(QueuedAt))]
    public DateTime QueuedAt { get; set; }

    [Column(nameof(QueuedBy))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public string QueuedBy { get; set; }

    [Column(nameof(Action))]
    public string Action { get; set; }

    [Column(nameof(Filename))]
    public string Filename { get; set; }
    
    [Column(nameof(Row))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public int? Row { get; set; }
    
    [Column(nameof(ParserSettings))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public string ParserSettings { get; set; }

    [Column(nameof(ContentTypeAlias))]
    public string ContentTypeAlias { get; set; }
    
    [Column(nameof(ContentTypeName))]
    public string ContentTypeName { get; set; }

    [Column(nameof(ReplacesId))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public Guid? ReplacesId { get; set; }
    
    [Column(nameof(ContainerId))]
    public Guid ContainerId { get; set; }
    
    [Column(nameof(Name))]
    public string Name { get; set; }
    
    [Column(nameof(Data))]
    [SpecialDbType(SpecialDbTypes.NTEXT)]
    public string Data { get; set; }

    [Column(nameof(Notices))]
    [NullSetting(NullSetting = NullSettings.Null)]
    [SpecialDbType(SpecialDbTypes.NTEXT)]
    public string Notices { get; set; }
    
    [Column(nameof(Status))]
    public string Status { get; set; }
    
    [Column(nameof(ImportedAt))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public DateTime? ImportedAt { get; set; }
    
    [Column(nameof(ImportedContentId))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public Guid? ImportedContentId { get; set; }

    [Column(nameof(ImportedContentSummary))]
    [NullSetting(NullSetting = NullSettings.Null)]
    public string ImportedContentSummary { get; set; }

    [Ignore]
    public bool CanProcess => Status.IsAnyOf(ImportStatuses.Abandoned, ImportStatuses.Queued, ImportStatuses.Error);
}
