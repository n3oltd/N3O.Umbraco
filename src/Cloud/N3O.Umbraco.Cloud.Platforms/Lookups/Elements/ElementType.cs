using Microsoft.AspNetCore.Mvc.Rendering;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class ElementType : NamedLookup {
    public ElementType(string id, string name, string tagName) : base(id, name) {
        TagName = tagName;
    }
    
    public string TagName { get; }
}

public class ElementTypes : StaticLookupsCollection<ElementType> {
    public static readonly ElementType CreateCrowdfunderButton = new("createCrowdfunderButton",
                                                                     "Create Crowdfunder Button",
                                                                     "n3o-crowdfunder-button");
    
    public static readonly ElementType DonationButton = new("donationButton",
                                                            "Donation Button",
                                                            "n3o-donation-button");

    public static readonly ElementType DonationForm = new("donationForm",
                                                          "Donation Form",
                                                          "n3o-donation-form");
    
    public static readonly ElementType DonationPopup = new("donationPopup",
                                                           "Donation Popup",
                                                           "n3o-donation-popup");
}