using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using System;
using System.Reflection;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Validation;

public class ValidationComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<IPhoneNumberValidator, PhoneNumberValidator>();
        
        builder.Services.AddTransient<IValidation, Validation>();
        builder.Services.AddTransient<IValidatorFactory, ValidatorFactory>();
        builder.Services.AddTransient<IValidationHandler, ValidationHandler>();
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorPipelineBehaviour<,>));
        
        RegisterAll(t => t.ImplementsGenericInterface(typeof(IValidator<>)),
                    t => RegisterValidator(builder, t));

        RegisterMiddleware(builder);

        ConfigureFluentValidation();
    }

    private void RegisterValidator(IUmbracoBuilder builder, Type validatorType) {
        var interfaceTypes = validatorType.GetInterfaces();

        foreach (var interfaceType in interfaceTypes) {
            builder.Services.AddTransient(interfaceType, validatorType);
        }
    }
    
    private void RegisterMiddleware(IUmbracoBuilder builder) {
        builder.Services.AddScoped<ExceptionMiddleware>();
        
        builder.Services.Configure<UmbracoPipelineOptions>(opt => {
            var filter = new UmbracoPipelineFilter(nameof(ExceptionMiddleware));

            filter.PrePipeline = app => {
                var runtimeState = app.ApplicationServices.GetRequiredService<IRuntimeState>();

                if (runtimeState.Level == RuntimeLevel.Run) {
                    app.UseMiddleware<ExceptionMiddleware>();
                }
            };

            opt.AddFilter(filter);
        });
    }
    
    private void ConfigureFluentValidation() {
        ValidatorOptions.Global.DisplayNameResolver = (_, member, _) => {
            var propertyInfo = member as PropertyInfo;

            return propertyInfo?.GetCustomAttribute<NameAttribute>()?.Name;
        };
    }
}
