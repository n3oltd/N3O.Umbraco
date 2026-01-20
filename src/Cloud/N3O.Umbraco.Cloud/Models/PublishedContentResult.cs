using N3O.Umbraco.Cloud.Lookups;
using Newtonsoft.Json.Linq;
using System;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedContentResult {
    private PublishedContentResult(bool notFound, Guid? id, PublishedFileKind kind, string path, JObject content) {
        NotFound = notFound;
        Id = id;
        Kind = kind;
        Path = path;
        Content = content;
    }

    public bool NotFound { get; }
    public Guid? Id { get; }
    public PublishedFileKind Kind { get; }
    public string Path { get; }
    public JObject Content { get; }

    public static PublishedContentResult ForFound(Guid id, PublishedFileKind kind, string path, JObject content) {
        return new PublishedContentResult(false, id, kind, path, content);
    }
    
    public static PublishedContentResult ForNotFound(string path) {
        return new PublishedContentResult(true, null, null, path, null);
    }
}