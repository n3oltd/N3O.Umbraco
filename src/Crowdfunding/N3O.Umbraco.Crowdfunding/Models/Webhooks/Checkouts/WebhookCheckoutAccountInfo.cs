using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Webhooks.Models;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookCheckoutAccountInfo : Value {
    public WebhookCheckoutAccountInfo(Guid id,
                                      WebhookReference reference,
                                      WebhookLookup type,
                                      WebhookElementsIndividual individual,
                                      WebhookElementsOrganization organization,
                                      WebhookElementsAddress address,
                                      WebhookElementsEmail email,
                                      WebhookElementsTelephone telephone,
                                      WebhookElementsPreferences preferences,
                                      WebhookElementsTaxStatus taxStatus) {
        Id = id;
        Reference = reference;
        Type = type;
        Individual = individual;
        Organization = organization;
        Address = address;
        Email = email;
        Telephone = telephone;
        Preferences = preferences;
        TaxStatus = taxStatus;
    }

    public Guid Id { get; }
    public WebhookReference Reference { get; }
    public WebhookLookup Type { get; }
    public WebhookElementsIndividual Individual { get; }
    public WebhookElementsOrganization Organization { get; }
    public WebhookElementsAddress Address { get; }
    public WebhookElementsEmail Email { get; }
    public WebhookElementsTelephone Telephone { get; }
    public WebhookElementsPreferences Preferences { get; }
    public WebhookElementsTaxStatus TaxStatus { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
        yield return Reference;
        yield return Type;
        yield return Individual;
        yield return Organization;
        yield return Address;
        yield return Email;
        yield return Telephone;
        yield return Preferences;
        yield return TaxStatus;
    }
    
    public string GetName(ILookups lookups) {
        if (Type == null) {
            return null;
        }

        var accountType = lookups.FindById<AccountType>(Type.Id);
        
        if (accountType == AccountTypes.Individual) {
            return $"{Individual?.Name?.FirstName} {Individual?.Name?.LastName}".Trim();
        } else if (accountType == AccountTypes.Organization) {
            return Organization?.Name;
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
}