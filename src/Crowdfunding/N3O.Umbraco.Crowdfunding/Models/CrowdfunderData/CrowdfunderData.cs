using N3O.Umbraco.Crm.Lookups;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderData : Value, ICrowdfunderData {
    [JsonConstructor]
    public CrowdfunderData(Guid crowdfunderId, CrowdfunderType crowdfunderType, string comment, bool anonymous) {
        CrowdfunderId = crowdfunderId;
        CrowdfunderType = crowdfunderType;
        Comment = comment;
        Anonymous = anonymous;
    }

    public CrowdfunderData(ICrowdfunderData crowdfunderData)
        : this(crowdfunderData.CrowdfunderId,
               crowdfunderData.CrowdfunderType,
               crowdfunderData.Comment,
               crowdfunderData.Anonymous) { }

    public Guid CrowdfunderId { get; }
    public CrowdfunderType CrowdfunderType { get; }
    public string Comment { get; }
    public bool Anonymous { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return CrowdfunderId;
        yield return CrowdfunderType;
        yield return Comment;
        yield return Anonymous;
    }
}