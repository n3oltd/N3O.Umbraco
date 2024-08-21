using FluentValidation.Results;
using N3O.Umbraco.Crowdfunding.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.CrowdFunding;

public interface IContentPropertyValidator {
    bool IsValidator(string contentTypeAlias, string propertyAlias);
    void PopulatePropertyConfiguration(IPropertyType property, ContentPropertyConfigurationRes res);
    ValidationResult Validate(IPublishedContent content, string propertyAlias, ValueReq req);
}