using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Content {
    public abstract class ContentValidator : IContentValidator {
        protected ContentValidator(IContentHelper contentHelper) {
            ContentHelper = contentHelper;
        }
    
        protected void ValidateProperty<TContent, TProperty>(IContent content,
                                                             Expression<Func<TContent, TProperty>> memberLambda,
                                                             Func<TProperty, bool> isValid, string message) {
            var propertyAlias = AliasHelper.ForProperty(memberLambda);
            var propertyValue = content.GetValue(memberLambda);

            if (!isValid(propertyValue)) {
                ErrorResult(content.Properties.Single(x => x.Alias == propertyAlias), message);
            }
        }

        protected void ErrorResult<TContent, TProperty>(IContent content,
                                                        Expression<Func<TContent, TProperty>> memberLambda,
                                                        string message) {
            var propertyAlias = AliasHelper.ForProperty(memberLambda);

            ErrorResult(content.Properties.Single(x => x.Alias == propertyAlias), message);
        }

        protected void ErrorResult(IProperty property, string message) {
            ErrorResult($"Property {property.PropertyType.Name.Quote()} {message}");
        }

        protected void ErrorResult(string message) {
            throw new ContentValidationErrorException(message);
        }

        protected void WarningResult<TContent, TProperty>(IContent content,
                                                          Expression<Func<TContent, TProperty>> memberLambda,
                                                          string message) {
            var propertyAlias = AliasHelper.ForProperty(memberLambda);

            WarningResult(content.Properties.Single(x => x.Alias == propertyAlias), message);
        }

        protected void WarningResult(IProperty property, string message) {
            WarningResult($"Property {property.PropertyType.Name.Quote()} {message}");
        }

        protected void WarningResult(string message) {
            throw new ContentValidationWarningException(message);
        }

        public abstract bool IsValidator(IContent content);
        public abstract void Validate(IContent content);
    
        protected IContentHelper ContentHelper { get; }
    }
}
