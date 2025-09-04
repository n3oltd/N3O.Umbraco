using N3O.Umbraco.Extensions;
using N3O.Umbraco.Parameters;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using N3O.Umbraco.Search.Typesense.Commands;
using N3O.Umbraco.Search.Typesense.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Typesense;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Search.Typesense.Notifications;

public class TypesenseStartupTasks : INotificationAsyncHandler<UmbracoApplicationStartedNotification> {
    private readonly ITypesenseClient _typesenseClient;
    private readonly IBackgroundJob _backgroundJob;

    public TypesenseStartupTasks(ITypesenseClient typesenseClient, IBackgroundJob backgroundJob) {
        _typesenseClient = typesenseClient;
        _backgroundJob = backgroundJob;
    }

    public async Task HandleAsync(UmbracoApplicationStartedNotification notification,
                                  CancellationToken cancellationToken) {
        foreach (var collection in TypesenseHelper.GetAllCollections()) {
            await MigrateCollectionAsync(collection);
        }
    }

    private async Task MigrateCollectionAsync(CollectionInfo collectionInfo) {
        var collection = await TryGetCollectionAsync(collectionInfo.Name);

        collection = await TryDropCollectionIfOldVersionAsync(collection, collectionInfo);
        
        if (collection == null) {
            await CreateCollectionAsync(collectionInfo);
            
            EnqueueIndexing(collectionInfo);
        }
    }

    private async Task<CollectionResponse> TryGetCollectionAsync(string collectionName) {
        try {
            return await _typesenseClient.RetrieveCollection(collectionName);
        } catch (TypesenseApiNotFoundException) {
            return null;
        }
    }
    
    private async Task<CollectionResponse> TryDropCollectionIfOldVersionAsync(CollectionResponse collection,
                                                                              CollectionInfo collectionInfo) {
        if (collection != null) {
            var metadataVersion = GetVersionFromMetadata(collection);

            if (metadataVersion != collectionInfo.Version) {
                await _typesenseClient.DeleteCollection(collectionInfo.Name);

                return null;
            }
        }

        return collection;
    }

    private async Task CreateCollectionAsync(CollectionInfo collectionInfo) {
        var schema = new Schema(collectionInfo.Name, collectionInfo.Fields) {
            Metadata = new Dictionary<string, object> {
                { TypesenseConstants.MetadataKeys.Version, collectionInfo.Version }
            }
        };

        await _typesenseClient.CreateCollection(schema);
    }

    private void EnqueueIndexing(CollectionInfo collection) {
        foreach (var contentType in collection.ContentTypeAliases.OrEmpty()) {
            _backgroundJob.EnqueueCommand<IndexContentsOfTypeCommand>(m => m.Add<ContentType>(contentType));   
        }
    }
    
    private int? GetVersionFromMetadata(CollectionResponse collection) {
        var collectionVersionElement = (JsonElement?) collection?.Metadata?.TryGet(TypesenseConstants.MetadataKeys.Version);

        return collectionVersionElement?.GetInt32();
    }
}