using N3O.Umbraco.Crm.Lookups;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderData : Value, ICrowdfunderData {
    [JsonConstructor]
    public CrowdfunderData(Guid id,
                           CrowdfunderType type,
                           string name,
                           string url,
                           string comment,
                           bool anonymous) {
        Id = id;
        Type = type;
        Name = name;
        Url = url;
        Comment = comment;
        Anonymous = anonymous;
    }

    public CrowdfunderData(ICrowdfunderData crowdfunderData)
        : this(crowdfunderData.Id,
               crowdfunderData.Type,
               crowdfunderData.Name,
               crowdfunderData.Url,
               crowdfunderData.Comment,
               crowdfunderData.Anonymous) { }

    public Guid Id { get; }
    public CrowdfunderType Type { get; }
    public string Name { get; }
    public string Url { get; }
    public string Comment { get; }
    public bool Anonymous { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
        yield return Type;
        yield return Name;
        yield return Url;
        yield return Comment;
        yield return Anonymous;
    }
}