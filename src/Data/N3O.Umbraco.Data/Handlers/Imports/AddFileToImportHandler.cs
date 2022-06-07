using N3O.Umbraco.Mediator;
using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Konstrukt;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Storage.Extensions;
using N3O.Umbraco.Storage.Services;
using System;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Scoping;

namespace N3O.Umbraco.Data.Handlers {
    public class AddFileToImportHandler : IRequestHandler<AddFileToImportCommand, AddFileToImportReq, None> {
        private readonly IJsonProvider _jsonProvider;
        private readonly Lazy<IVolume> _volume;
        private readonly IScopeProvider _scopeProvider;

        public AddFileToImportHandler(IJsonProvider jsonProvider,
                                      IScopeProvider scopeProvider,
                                      Lazy<IVolume> volume) {
            _jsonProvider = jsonProvider;
            _volume = volume;
            _scopeProvider = scopeProvider;
        }

        public async Task<None> Handle(AddFileToImportCommand req, CancellationToken cancellationToken) {
            Import import;

            using (var scope = _scopeProvider.CreateScope()) {
                var query = scope.Database.Query<Import>().Where(x => x.Reference == req.ReferenceId);
                import = await query.FirstOrDefaultAsync();
                scope.Complete();
            }

            var storageFolderName = _jsonProvider.DeserializeObject<ParserSettings>(import.ParserSettings).StorageFolderName;

            if (req.Model.ZipFile != null) {
                await ExtractToStorageFolderAsync(req.Model.ZipFile, storageFolderName);
            }

            return None.Empty;
        }

        private async Task ExtractToStorageFolderAsync(StorageToken zipStorageToken, string storageFolderName) {
            var tempStorage = await _volume.Value.GetTempFolderAsync();
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
    }
}