using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Storage;
using N3O.Umbraco.Storage.Extensions;
using NodaTime;
using System;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Handlers;

public class QueueImportsHandler : IRequestHandler<QueueImportsCommand, QueueImportsReq, QueueImportsRes> {
    private const int MaxRowsCount = 5_000;

    private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;
    private readonly IWorkspace _workspace;
    private readonly IClock _clock;
    private readonly IContentTypeService _contentTypeService;
    private readonly IImportQueue _importQueue;
    private readonly Lazy<IVolume> _volume;
    private readonly ErrorLog _errorLog;
    private readonly string _nameColumnTitle;
    private readonly string _replacesColumnTitle;
    private readonly string _contentIdColumnTitle;

    public QueueImportsHandler(IBackOfficeSecurityAccessor backOfficeSecurityAccessor,
                               IWorkspace workspace,
                               IClock clock,
                               IContentTypeService contentTypeService,
                               IDataTypeService dataTypeService,
                               IImportQueue importQueue,
                               Lazy<IVolume> volume,
                               IFormatter formatter) {
        _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
        _workspace = workspace;
        _clock = clock;
        _contentTypeService = contentTypeService;
        _importQueue = importQueue;
        _volume = volume;
        _errorLog = new ErrorLog(formatter);

        _nameColumnTitle = formatter.Text.Format<DataStrings>(s => s.NameColumnTitle);
        _replacesColumnTitle = formatter.Text.Format<DataStrings>(s => s.ReplacesColumnTitle);
        _contentIdColumnTitle = formatter.Text.Format<DataStrings>(s => s.ContentIdColumnTitle);
    }
    
    public async Task<QueueImportsRes> Handle(QueueImportsCommand req, CancellationToken cancellationToken) {
        var instant = _clock.GetCurrentInstant();
        var storageFolderName = instant.ToUnixTimeMilliseconds().ToString();
        var csvBlob = await _volume.Value.MoveFileAsync(req.Model.CsvFile.Filename,
                                                        req.Model.CsvFile.StorageFolderName,
                                                        storageFolderName);
        
        using (csvBlob.Stream) {
            var contentType = _contentTypeService.Get(req.ContentType);

            var csvReader = _workspace.GetCsvReader(req.Model.DatePattern,
                                                    DecimalSeparators.Point,
                                                    BlobResolvers.Url(),
                                                    TextEncodings.Utf8,
                                                    csvBlob.Stream,
                                                    true);

            var columnHeadings = csvReader.GetColumnHeadings();

            if (req.Model.ZipFile != null) {
                await ExtractToStorageFolderAsync(req.Model.ZipFile, storageFolderName);
            }
            
            var currentUser = _backOfficeSecurityAccessor.BackOfficeSecurity.CurrentUser;
            var hasReplaceColumn = columnHeadings.Contains(_replacesColumnTitle, true);
            var hasNameColumn = columnHeadings.Contains(_nameColumnTitle, true);
            var hasContentIdColumn = columnHeadings.Contains(_contentIdColumnTitle, true);
            var rowNumber = 1;
            
            while (csvReader.ReadRow()) {
                if (rowNumber > MaxRowsCount) {
                    _errorLog.AddError<Strings>(s => s.MaxRowsExceeded_1, MaxRowsCount);
                    _errorLog.ThrowIfHasErrors();
                }

                var contentId = default(Guid?);

                if (hasContentIdColumn) {
                    var contentIdField = csvReader.Row.GetRawField(_contentIdColumnTitle);

                    if (contentIdField.HasValue()) {
                        contentId = Guid.Parse(contentIdField);
                    }
                }

                var name = hasNameColumn ? csvReader.Row.GetRawField(_nameColumnTitle) : null;
                var replacesCriteria = hasReplaceColumn ? csvReader.Row.GetRawField(_replacesColumnTitle) : null;
                var sourceValues = columnHeadings.ToDictionary(x => x, x => csvReader.Row.GetRawField(x));
                
                if (!replacesCriteria.HasValue() && !name.HasValue()) {
                    _errorLog.AddError<Strings>(s => s.MissingName);
                    _errorLog.ThrowIfHasErrors();
                }

                await _importQueue.AppendAsync(req.ContainerId.Value,
                                               contentType.Alias,
                                               csvBlob.Filename,
                                               rowNumber,
                                               instant,
                                               currentUser.Name,
                                               req.Model.DatePattern,
                                               storageFolderName,
                                               contentId,
                                               replacesCriteria,
                                               name,
                                               req.Model.MoveUpdatedContentToCurrentLocation.GetValueOrThrow(),
                                               sourceValues,
                                               cancellationToken);

                rowNumber++;
            }

            var count = await _importQueue.CommitAsync();

            return new QueueImportsRes {
               Count = count
           };
        }
    }
    
    private async Task ExtractToStorageFolderAsync(StorageToken zipStorageToken, string storageFolderName) {
        var tempStorage = await _volume.Value.GetStorageFolderAsync(zipStorageToken.StorageFolderName);
        var storageFolder = await _volume.Value.GetStorageFolderAsync(storageFolderName);
        var zipBlob = await tempStorage.GetFileAsync(zipStorageToken.Filename);

        try {
            using (zipBlob.Stream) {
                var zipArchive = new ZipArchive(zipBlob.Stream, ZipArchiveMode.Read);
                
                await zipArchive.ExtractToStorageFolderAsync(storageFolder);
            }
        } finally {
            await tempStorage.DeleteFileAsync(zipBlob.Filename);
        }
    }
    
    public class Strings : CodeStrings {
        public string MaxRowsExceeded_1 => $"The CSV file contains more than the maximum allowed {0} rows";
        public string MissingName => "Name is required to import new content";
    }
}
