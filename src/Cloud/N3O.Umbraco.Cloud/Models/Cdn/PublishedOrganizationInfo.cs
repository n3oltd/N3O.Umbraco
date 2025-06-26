using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedOrganizationInfo : IOrganizationInfo {
    public string Name { get; set; }
    public string CharityRegistration { get; set; }
    public string AddressSingleLine { get; set; }
    public string AddressPostalCode { get; set; }
    public Country AddressCountry { get; set; }
    public Uri Logo { get; set; }
}