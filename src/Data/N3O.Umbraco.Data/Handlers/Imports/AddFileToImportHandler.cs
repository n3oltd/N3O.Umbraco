using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Konstrukt;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Storage.Extensions;
using N3O.Umbraco.Storage.Services;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Scoping;

namespace N3O.Umbraco.Data.Handlers {
    public class AddFileToImportHandler : IRequestHandler<AddFileToImportCommand, AddFileToImportReq, None> {
        private readonly IJsonProvider _jsonProvider;
        private readonly IScopeProvider _scopeProvider;
        private readonly IVolume _volume;

        public AddFileToImportHandler(IJsonProvider jsonProvider, IScopeProvider scopeProvider, IVolume volume) {
            _jsonProvider = jsonProvider;
            _scopeProvider = scopeProvider;
            _volume = volume;
        }

        public async Task<None> Handle(AddFileToImportCommand req, CancellationToken cancellationToken) {
            using (var scope = _scopeProvider.CreateScope()) {
                var import = await scope.Database
                                        .Query<Import>()
                                        .FirstOrDefaultAsync(x => x.Reference == req.ReferenceId);

                await _volume.MoveFileAsync(req.Model.File.Filename,
                                            req.Model.File.StorageFolderName,
                                            import.GetStorageFolderName(_jsonProvider));

                return None.Empty;
            }
        }
    }
}