using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Events;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CreateFundraiserResult : Value {
    private CreateFundraiserResult(bool success, string error, Guid? id, string url) {
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

    public static CreateFundraiserResult ForError(IEnumerable<EventMessage> eventMessages) {
        var error = string.Join("\n", eventMessages.Select(x => $"{x.MessageType}: {x.Message}"));
        
        return new CreateFundraiserResult(false, error, null, null);
    }
    
    public static CreateFundraiserResult ForSuccess(FundraiserContent fundraiser) {
        return new CreateFundraiserResult(true, null, fundraiser.Content().Key, fundraiser.Content().AbsoluteUrl());
    }
}