using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Entities;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data.Handlers;

public class GetExportFileHandler : IRequestHandler<GetExportFileQuery, None, ExportFile> {
    private readonly IRepository<Export> _repository;
    private readonly IVolume _volume;

    public GetExportFileHandler(IRepository<Export> repository, IVolume volume) {
        _repository = repository;
        _volume = volume;
    }

    public async Task<ExportFile> Handle(GetExportFileQuery req, CancellationToken cancellationToken) {
        var export = await req.ExportId.RunAsync(_repository.GetAsync, true, cancellationToken);
        var storageFolder = await _volume.GetStorageFolderAsync(export.StorageFolderName);
        var blob = await storageFolder.GetFileAsync(export.Filename, cancellationToken);

        return new ExportFile(blob.Filename, blob.ContentType, blob.Stream);
    }
}