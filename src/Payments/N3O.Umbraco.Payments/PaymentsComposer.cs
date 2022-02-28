using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Payments {
    public class PaymentsComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddTransient<IPaymentsScope, PaymentsScope>();
            builder.Services.AddOpenApiDocument(PaymentsConstants.ApiName);
            
            RegisterAll(t => t.ImplementsGenericInterface(typeof(IPaymentMethodViewModel<>)),
                        t => builder.Services.AddTransient(GetServiceType(t), t));
        }

        private Type GetServiceType(Type type) {
            var paymentMethodType = type.GetParameterTypesForGenericInterface(typeof(IPaymentMethodViewModel<>))
                                        .Single();

            return typeof(IPaymentMethodViewModel<>).MakeGenericType(paymentMethodType);
        }
    }
}