using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedOrganizationInfo : Value, IOrganizationInfo {
    public string Name { get; set; }
    public string CharityRegistration { get; set; }
    public string AddressSingleLine { get; set; }
    public string AddressPostalCode { get; set; }
    public Country AddressCountry { get; set; }
    public Uri Logo { get; set; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Name;
        yield return CharityRegistration;
        yield return AddressSingleLine;
        yield return AddressPostalCode;
        yield return AddressCountry;
        yield return Logo?.AbsoluteUri;
    }
}