using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Reflection;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Validation {
    public class ValidationComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddFluentValidation(config => {
                config.DisableDataAnnotationsValidation = true;
                config.ImplicitlyValidateChildProperties = true;
                config.ImplicitlyValidateRootCollectionElements = true;

                config.RegisterValidatorsFromAssemblies(OurAssemblies.GetAllAssemblies());
            });
        
            builder.Services.AddSingleton<IPhoneNumberValidator, PhoneNumberValidator>();

            RegisterAll(t => t.ImplementsGenericInterface(typeof(IValidator<>)),
                        t => RegisterValidator(builder, t));

            ValidatorOptions.Global.DisplayNameResolver = (_, member, _) => {
                var propertyInfo = member as PropertyInfo;

                return propertyInfo?.GetCustomAttribute<NameAttribute>()?.Name;
            };
            
            ValidatorOptions.Global.PropertyNameResolver = CamelCasePropertyNameResolver.ResolvePropertyName;
        }

        private void RegisterValidator(IUmbracoBuilder builder, Type validatorType) {
            var interfaceTypes = validatorType.GetInterfaces();

            foreach (var interfaceType in interfaceTypes) {
                builder.Services.AddTransient(interfaceType, validatorType);
            }
        }
    }
}
