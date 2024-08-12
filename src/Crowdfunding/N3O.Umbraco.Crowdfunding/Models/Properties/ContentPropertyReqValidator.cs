using FluentValidation;
using N3O.Umbraco.CrowdFunding;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace N3O.Umbraco.Crowdfunding.Models;

public class ContentPropertyReqValidator : ModelValidator<ContentPropertyReq> {
    private readonly IEnumerable<IContentPropertyValidator> _validators;
    
    public ContentPropertyReqValidator(IFormatter formatter, IEnumerable<IContentPropertyValidator> validators)
        : base(formatter) {
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
        var validator = _validators.SingleOrDefault(x => x.IsValidator(req.Alias));
        
        if (validator != null) {
            var result = validator.IsValid(req.Alias, req.Type);

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
        public string SpecifyAlias => "Please specify the alias";
        public string SpecifyPropertyType => "Please specify the property type";
        public string PropertyTypeInvalid => "The property type is invalid for the specified property";
        public string SpecifyValidPropertyTypeValue => "Please specify valid value for the specified property type";
    }
}