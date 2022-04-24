using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Konstrukt;
using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Data.Handlers {
    public class ProcessImport : IRequestHandler<ProcessImportCommand, None, None> {
        private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
        private readonly IContentEditor _contentEditor;
        private readonly IReadOnlyList<PropertyConverter> _converters;

        public ProcessImport(IUmbracoDatabaseFactory umbracoDatabaseFactory,
                             IContentEditor contentEditor,
                             IEnumerable<PropertyConverter> converters) {
            _umbracoDatabaseFactory = umbracoDatabaseFactory;
            _contentEditor = contentEditor;
            _converters = converters.ToList();
        }
        
        public async Task<None> Handle(ProcessImportCommand req, CancellationToken cancellationToken) {
            using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                var import = await req.ImportId.RunAsync((id, _) => db.SingleByIdAsync<Import>(id),
                                                         true,
                                                         cancellationToken);

                if (import.CanProcess) {
                    try {
                        var contentPublisher = GetContentPublisher(import);

                        // Add method import called GetFields() where pass a JSON converter and let it deserialize
                        // the fields. Each field should have the propertyAlias, propertyType, and also the values,
                        // (an array of strings) + label etc. The field should be able to construct and UmbracoPropertyInfo
                        //var converter = _converters.Single(x => x.IsConverter(umbracoPropertyInfo));
                        //converter.Import(contentPublisher, umbracoPropertyInfo, values);

                        var publishResult = contentPublisher.SaveAndPublish();

                        // Add these methods to import to set its status and also to include the ID + reference was set to
                        if (publishResult.Success) {
                            //import.Published(id, reference);
                        } else {
                            //import.Saved(id, reference);
                        }
                    } catch {
                        // Catch specific exceptions that correspond to parsing errors, e.g. date is malformed
                        // This can be a generic message of the form, "Cannot convert value '[whatever text was' to
                        // type [dataType]'. This is enough for people to figure out what they need to do.
                    }

                    await db.UpdateAsync(import);
                }
            }

            return None.Empty;
        }

        private IContentPublisher GetContentPublisher(Import import) {
            if (import.Action == ImportActions.Create) {
                return _contentEditor.New("New", import.ParentId, import.ContentTypeAlias);
            } else if (import.Action == ImportActions.Update) {
                return _contentEditor.ForExisting(import.ReplacesId.Value);
            } else {
                throw UnrecognisedValueException.For(import.Action);
            }
        }
    }
}