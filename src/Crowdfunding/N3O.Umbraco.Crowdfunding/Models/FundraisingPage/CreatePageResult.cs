using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Events;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CreatePageResult : Value {
    public CreatePageResult(bool success, string error, Guid? id, string url) {
        Success = success;
        Error = error;
        Id = id;
        Url = url;
    }

    public bool Success { get; }
    public string Error { get; }
    public Guid? Id { get; }
    public string Url { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Success;
        yield return Error;
        yield return Id;
        yield return Url;
    }

    public static CreatePageResult ForSuccess(CrowdfundingPageContent crowdfundingPage) {
        return new CreatePageResult(true,
                                    null,
                                    crowdfundingPage.Content().Key,
                                    crowdfundingPage.Content().AbsoluteUrl());
    }

    public static CreatePageResult ForError(IEnumerable<EventMessage> eventMessages) {
        var error = string.Join("\n", eventMessages.Select(x => $"{x.MessageType}: {x.Message}"));
        
        return new CreatePageResult(false, error, null, null);
    }
}