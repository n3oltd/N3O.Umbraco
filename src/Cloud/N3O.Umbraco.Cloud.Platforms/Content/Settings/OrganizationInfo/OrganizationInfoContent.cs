using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Settings.OrganizationInfo.Alias)]
public class OrganizationInfoContent : UmbracoContent<OrganizationInfoContent> {
		public Country AddressCountry => GetValue(x => x.AddressCountry);
		public string AddressPostalCode => GetValue(x => x.AddressPostalCode);
		public string AddressSingleLine => GetValue(x => x.AddressSingleLine);
		public string CharityRegistration => GetValue(x => x.CharityRegistration);
		public string Email => GetValue(x => x.Email);
		public MediaWithCrops Logo => GetValue(x => x.Logo);
		public string OrganisationName => GetValue(x => x.OrganisationName);
		public Link SupportUrl => GetValue(x => x.SupportUrl);
		public string Telephone => GetValue(x => x.Telephone);
}