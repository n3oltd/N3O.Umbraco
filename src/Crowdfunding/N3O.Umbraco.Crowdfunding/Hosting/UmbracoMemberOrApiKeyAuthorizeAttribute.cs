using Microsoft.AspNetCore.Mvc;

namespace N3O.Umbraco.Crowdfunding.Hosting;

public class UmbracoMemberOrApiKeyAuthorizeAttribute : TypeFilterAttribute<UmbracoMemberOrApiKeyAuthorizeFilter> { }