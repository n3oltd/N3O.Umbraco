using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using N3O.Umbraco.Search.Typesense.Commands;
using N3O.Umbraco.Search.Typesense.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Typesense;

namespace N3O.Umbraco.Search.Typesense.Handlers;

public class MigrateCollectionsHandler : IRequestHandler<MigrateCollectionsCommand, None, None> {
    private readonly ITypesenseClient _typesenseClient;
    private readonly IReadOnlyList<ISearchIndexer> _searchIndexers;
    private readonly IContentLocator _contentLocator;
    private readonly IBackgroundJob _backgroundJob;

    public MigrateCollectionsHandler(ITypesenseClient typesenseClient,
                                     IEnumerable<ISearchIndexer> searchIndexers,
                                     IContentLocator contentLocator,
                                     IBackgroundJob backgroundJob) {
        _typesenseClient = typesenseClient;
        _searchIndexers = searchIndexers.OrEmpty().ToList();
        _contentLocator = contentLocator;
        _backgroundJob = backgroundJob;
    }

    public async Task<None> Handle(MigrateCollectionsCommand req, CancellationToken cancellationToken) {
        if (_typesenseClient.HasValue()) {
            foreach (var collection in TypesenseHelper.GetAllCollections()) {
                await MigrateCollectionAsync(collection, cancellationToken);
            }
        }

        return None.Empty;
    }

    private async Task MigrateCollectionAsync(CollectionInfo collectionInfo, CancellationToken cancellationToken) {
        var aliasName = collectionInfo.Name.Resolve();
        var newPhysicalName = GetPhysicalCollectionName(aliasName, collectionInfo.Version);
        var existing = await TryGetCollectionAsync(aliasName, cancellationToken);

        if (existing == null) {
            cancellationToken.ThrowIfCancellationRequested();
            await CreateCollectionAsync(newPhysicalName, collectionInfo);

            cancellationToken.ThrowIfCancellationRequested();
            await _typesenseClient.UpsertCollectionAlias(aliasName, new CollectionAlias(newPhysicalName));

            EnqueueIndexing(collectionInfo);

            return;
        }

        if (!IsOldVersion(existing, collectionInfo)) {
            return;
        }

        var alias = await TryGetAliasAsync(aliasName, cancellationToken);

        // A site indexed before collections were versioned has a real collection, not an alias, at the alias name
        var isLegacyCollection = alias == null;
        var oldPhysicalName = isLegacyCollection ? aliasName : alias.CollectionName;

        var documentIds = await GetAllDocumentIdsAsync(oldPhysicalName, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
        await TryDeleteCollectionAsync(newPhysicalName);

        cancellationToken.ThrowIfCancellationRequested();
        await CreateCollectionAsync(newPhysicalName, collectionInfo);

        await ReindexAllAsync(collectionInfo, documentIds, newPhysicalName, cancellationToken);

        // Typesense cannot hold an alias and a collection under one name, so the legacy collection goes first
        if (isLegacyCollection) {
            cancellationToken.ThrowIfCancellationRequested();
            await TryDeleteCollectionAsync(oldPhysicalName);
        }

        cancellationToken.ThrowIfCancellationRequested();
        await _typesenseClient.UpsertCollectionAlias(aliasName, new CollectionAlias(newPhysicalName));

        if (!isLegacyCollection) {
            cancellationToken.ThrowIfCancellationRequested();
            await TryDeleteCollectionAsync(oldPhysicalName);
        }
    }

    private async Task ReindexAllAsync(CollectionInfo collectionInfo,
                                       IReadOnlyList<string> documentIds,
                                       string targetCollectionName,
                                       CancellationToken cancellationToken) {
        var indexers = _searchIndexers.Where(x => x.IsIndexerFor(collectionInfo.Name)).ToList();

        foreach (var documentId in documentIds) {
            cancellationToken.ThrowIfCancellationRequested();

            if (!SearchDocument.TryParseId(documentId, out var contentKey, out var culture)) {
                continue;
            }

            var publishedContent = _contentLocator.ById(contentKey);

            if (publishedContent == null) {
                continue;
            }

            foreach (var indexer in indexers.Where(x => x.CanIndex(publishedContent))) {
                await indexer.IndexAsync(publishedContent, culture, targetCollectionName);
            }
        }
    }

    private void EnqueueIndexing(CollectionInfo collectionInfo) {
        foreach (var contentType in collectionInfo.ContentTypeAliases.OrEmpty()) {
            _backgroundJob.EnqueueCommand<IndexContentsOfTypeCommand>(m => m.Add<ContentType>(contentType), contentType);
        }
    }

    private async Task<CollectionResponse> TryGetCollectionAsync(string collectionName,
                                                                 CancellationToken cancellationToken) {
        try {
            return await _typesenseClient.RetrieveCollection(collectionName, cancellationToken);
        } catch (TypesenseApiNotFoundException) {
            return null;
        }
    }

    private async Task<CollectionAliasResponse> TryGetAliasAsync(string aliasName, CancellationToken cancellationToken) {
        try {
            return await _typesenseClient.RetrieveCollectionAlias(aliasName, cancellationToken);
        } catch (TypesenseApiNotFoundException) {
            return null;
        }
    }

    private async Task TryDeleteCollectionAsync(string collectionName) {
        try {
            await _typesenseClient.DeleteCollection(collectionName);
        } catch (TypesenseApiNotFoundException) { }
    }

    private async Task CreateCollectionAsync(string physicalCollectionName, CollectionInfo collectionInfo) {
        var metadata = new Dictionary<string, object>();
        metadata.Add(TypesenseConstants.MetadataKeys.Version, collectionInfo.Version);

        // Schema is a third-party record with init-only properties, so an object initializer is the only way to set them
        var schema = new Schema(physicalCollectionName, collectionInfo.Fields) {
            EnableNestedFields = true,
            Metadata = metadata
        };

        await _typesenseClient.CreateCollection(schema);
    }

    private async Task<IReadOnlyList<string>> GetAllDocumentIdsAsync(string physicalCollectionName,
                                                                     CancellationToken cancellationToken) {
        var exportParameters = new ExportParameters();
        exportParameters.IncludeFields = "id";

        var documents = await _typesenseClient.ExportDocuments<ExportedDocument>(physicalCollectionName,
                                                                                 exportParameters,
                                                                                 cancellationToken);

        return documents.Select(x => x.Id).ToList();
    }

    private bool IsOldVersion(CollectionResponse collection, CollectionInfo collectionInfo) {
        var metadataVersion = GetVersionFromMetadata(collection);

        return metadataVersion != collectionInfo.Version;
    }

    private int? GetVersionFromMetadata(CollectionResponse collection) {
        var raw = collection?.Metadata?.TryGet(TypesenseConstants.MetadataKeys.Version);

        if (raw == null) {
            return null;
        } else if (raw is JsonElement element) {
            return element.GetInt32();
        } else if (raw is int i) {
            return i;
        } else {
            throw new Exception($"Version metadata has unrecognised format: {raw.GetType().FullName}");
        }
    }

    private static string GetPhysicalCollectionName(string aliasName, int version) {
        return $"{aliasName}_v{version}";
    }
}
