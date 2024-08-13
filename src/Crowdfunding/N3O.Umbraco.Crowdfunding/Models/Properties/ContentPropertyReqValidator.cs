using FluentValidation;
using N3O.Umbraco.Content;
using N3O.Umbraco.CrowdFunding;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace N3O.Umbraco.Crowdfunding.Models;

public class ContentPropertyReqValidator : ModelValidator<ContentPropertyReq> {
    private readonly IContentLocator _contentLocator;
    private readonly ContentId _contentId;
    private readonly IEnumerable<IContentPropertyValidator> _validators;
    
    public ContentPropertyReqValidator(IFormatter formatter,
                                       IContentLocator contentLocator,
                                       ContentId contentId,
                                       IEnumerable<IContentPropertyValidator> validators)
        : base(formatter) {
        _contentLocator = contentLocator;
        _contentId = contentId;
        _validators = validators;
        
        RuleFor(x => x.Alias)
           .NotEmpty()
           .WithMessage(Get<Strings>(x => x.SpecifyAlias));

        RuleFor(x => x.Type)
           .NotNull()
           .WithMessage(Get<Strings>(x => x.SpecifyPropertyType));
        
        RuleFor(x => x)
           .Must(ValidatePropertyType)
           .WithMessage(Get<Strings>(s => s.PropertyTypeInvalid));
        
        RuleFor(x => x)
           .Must(ValidatePropertyTypeValue)
           .WithMessage(Get<Strings>(s => s.SpecifyValidPropertyTypeValue));
    }
    
    private bool ValidatePropertyType(ContentPropertyReq req) {
        var content = _contentId.Run(id => _contentLocator.ById(id), false);
        
        var validator = _validators.SingleOrDefault(x => x.IsValidator(content.ContentType.Alias, req.Alias));
        
        if (validator != null) {
            var result = validator.IsValid(content, req.Alias, req.Value.Value);

            return result;
        } else {
            return true;
        }
    }
    
    private bool ValidatePropertyTypeValue(ContentPropertyReq req) {
        var type = req.Type?.GetType().BaseType.GenericTypeArguments.First();
        
        var modelProperties = req.GetType()
                                 .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                 .Where(x => typeof(ValueReq).IsAssignableFrom(x.PropertyType))
                                 .ToList();

        bool isValid = modelProperties.SingleOrDefault(x => x.PropertyType == type)?.GetValue(req).HasValue() == true;

        if (modelProperties.Where(x => x.PropertyType != type).Any(x => x.GetValue(req).HasValue())) {
            isValid = false;
        }

        return isValid;
    }

    public class Strings : ValidationStrings {
        public string PropertyTypeInvalid => "The property type is invalid for the specified property";
        public string SpecifyAlias => "Please specify the alias";
        public string SpecifyPropertyType => "Please specify the property type";
        public string SpecifyValidPropertyTypeValue => "Please specify valid value for the specified property type";
    }
}