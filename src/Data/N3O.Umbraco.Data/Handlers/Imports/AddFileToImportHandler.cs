using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Konstrukt;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Storage.Extensions;
using N3O.Umbraco.Storage.Services;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Data.Handlers {
    public class AddFileToImportHandler : IRequestHandler<AddFileToImportCommand, AddFileToImportReq, None> {
        private readonly IJsonProvider _jsonProvider;
        private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
        private readonly IVolume _volume;

        public AddFileToImportHandler(IJsonProvider jsonProvider,
                                      IUmbracoDatabaseFactory umbracoDatabaseFactory,
                                      IVolume volume) {
            _jsonProvider = jsonProvider;
            _umbracoDatabaseFactory = umbracoDatabaseFactory;
            _volume = volume;
        }

        public async Task<None> Handle(AddFileToImportCommand req, CancellationToken cancellationToken) {
            using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                var import = await db.Query<Import>().FirstOrDefaultAsync(x => x.Reference == req.ReferenceId);

                await _volume.MoveFileAsync(req.Model.File.Filename,
                                            req.Model.File.StorageFolderName,
                                            import.GetStorageFolderName(_jsonProvider));

                return None.Empty;
            }
        }
    }
}