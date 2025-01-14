using Microsoft.AspNetCore.Mvc;

namespace N3O.Umbraco.Crowdfunding.Attributes;

public class UmbracoMemberOrApiKeyAuthorizeAttribute : TypeFilterAttribute<UmbracoMemberOrApiKeyAuthorizeFilter> { }