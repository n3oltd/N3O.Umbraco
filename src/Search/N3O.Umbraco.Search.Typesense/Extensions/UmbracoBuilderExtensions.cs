using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using N3O.Umbraco.Search.Typesense.Attributes;
using N3O.Umbraco.Search.Typesense.Commands;
using N3O.Umbraco.Search.Typesense.NamedParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Typesense;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Search.Typesense.Extensions;

public static class UmbracoBuilderExtensions {
    public static async Task RegisterCollectionForAsync<TContent, TDocument>(this IUmbracoBuilder builder) 
        where TContent : IPublishedContent
        where TDocument : class {
        var typesenseClient = builder.Services.BuildServiceProvider().GetRequiredService<ITypesenseClient>();
        
        var collectionName = TypesenseHelper.GetCollectionName<TDocument>();
        var currentVersion = TypesenseHelper.GetCollectionVersion<TDocument>();

        if (!collectionName.HasValue()) {
            throw new Exception("Collection name and version must be specified");
        }
        
        var collection = await typesenseClient.RetrieveCollection(collectionName);

        if (!collection.HasValue()) {
            await CreateCollectionAsync<TDocument>(typesenseClient, collectionName, currentVersion);
            
            IndexDocuments<TContent>(builder);
        } else {
            var collectionVersion = GetCollectionVersion(collection);

            if (collectionVersion != currentVersion) {
                await typesenseClient.DeleteCollection(collectionName);
            
                await CreateCollectionAsync<TDocument>(typesenseClient, collectionName, currentVersion);

                IndexDocuments<TContent>(builder);
            }
        }
    }
    
    private static async Task CreateCollectionAsync<TDocument>(ITypesenseClient typesenseClient,
                                                               string collectionName,
                                                               int collectionVersion) {
        var fields = new List<Field>();

        foreach (var property in typeof(TDocument).GetProperties().Where(x => x.HasAttribute<SchemaFieldAttribute>())) {
            var schemaField = property.GetCustomAttribute<SchemaFieldAttribute>();
            
            if (!schemaField.Name.HasValue() || !schemaField.Type.HasValue()) {
                throw new Exception("field name and type must be specified");
            }

            var field = new Field(schemaField.Name,
                                  schemaField.Type,
                                  schemaField.Facet,
                                  schemaField.Optional,
                                  schemaField.Index,
                                  schemaField.Sort,
                                  schemaField.Infix,
                                  schemaField.Locale,
                                  schemaField.NumberOfDimensions,
                                  schemaField.Embed);

            fields.Add(field);
        }
        
        var schema = new Schema(collectionName, fields) { Metadata = new Dictionary<string, object>() };
        schema.Metadata.Add(nameof(CollectionAttribute.Version), collectionVersion);

        await typesenseClient.CreateCollection(schema);
    }

    private static int? GetCollectionVersion(CollectionResponse collection) {
        var collectionVersionElement = (JsonElement?) collection?.Metadata?.TryGet(nameof(CollectionAttribute.Version));

        return !collectionVersionElement.HasValue() ? null : collectionVersionElement?.GetInt32();
    }
    
    private static void IndexDocuments<TContent>(IUmbracoBuilder builder) where TContent : IPublishedContent {
        var backgroundJob = builder.Services.BuildServiceProvider().GetRequiredService<IBackgroundJob>();
        var contentType = AliasHelper<TContent>.ContentTypeAlias();
        
        backgroundJob.EnqueueCommand<IndexContentsOfTypeCommand>(m => m.Add<ContentType>(contentType));

    }
}