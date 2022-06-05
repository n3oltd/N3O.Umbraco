using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Clients.Accounts;
using N3O.Umbraco.Clients.Data.Content;
using N3O.Umbraco.Clients.Data.ContentTypes;
using N3O.Umbraco.Clients.Data.DataTypes;
using N3O.Umbraco.Clients.Data.Exports;
using N3O.Umbraco.Clients.Data.Imports;
using N3O.Umbraco.Clients.Giving;
using N3O.Umbraco.Clients.Giving.Cart;
using N3O.Umbraco.Clients.Giving.Checkout;
using N3O.Umbraco.Clients.Newsletters;
using N3O.Umbraco.Clients.Payments;
using N3O.Umbraco.Clients.Payments.Bambora;
using N3O.Umbraco.Clients.Payments.GoCardless;
using N3O.Umbraco.Clients.Payments.Opayo;
using N3O.Umbraco.Clients.Payments.PayPal;
using N3O.Umbraco.Clients.Payments.Stripe;
using N3O.Umbraco.Clients.Plugins.Cropper;
using N3O.Umbraco.Clients.Plugins.Uploader;
using System;
using System.Net.Http;

namespace N3O.Umbraco.Data.Clients {
    public static class ServiceCollectionExtensions {
        public static void AddUmbracoClients(this IServiceCollection services,
                                             Func<IServiceProvider, string> baseUrlResolver) {
            AddUmbracoClient<IAccountsClient, AccountsClient>(services, baseUrlResolver);
            
            AddUmbracoClient<IContentClient, ContentClient>(services, baseUrlResolver);
            AddUmbracoClient<IContentTypesClient, ContentTypesClient>(services, baseUrlResolver);
            AddUmbracoClient<IDataTypesClient, DataTypesClient>(services, baseUrlResolver);
            AddUmbracoClient<IExportsClient, ExportsClient>(services, baseUrlResolver);
            AddUmbracoClient<IImportsClient, ImportsClient>(services, baseUrlResolver);

            AddUmbracoClient<IGivingClient, GivingClient>(services, baseUrlResolver);
            AddUmbracoClient<ICartClient, CartClient>(services, baseUrlResolver);
            AddUmbracoClient<ICheckoutClient, CheckoutClient>(services, baseUrlResolver);
            
            AddUmbracoClient<INewslettersClient, NewslettersClient>(services, baseUrlResolver);
            
            AddUmbracoClient<IPaymentsClient, PaymentsClient>(services, baseUrlResolver);
            AddUmbracoClient<IBamboraClient, BamboraClient>(services, baseUrlResolver);
            AddUmbracoClient<IGoCardlessClient, GoCardlessClient>(services, baseUrlResolver);
            AddUmbracoClient<IOpayoClient, OpayoClient>(services, baseUrlResolver);
            AddUmbracoClient<IPayPalClient, PayPalClient>(services, baseUrlResolver);
            AddUmbracoClient<IStripeClient, StripeClient>(services, baseUrlResolver);
            
            AddUmbracoClient<ICropperClient, CropperClient>(services, baseUrlResolver);
            AddUmbracoClient<IUploaderClient, UploaderClient>(services, baseUrlResolver);
        }

        private static void AddUmbracoClient<TService, TImplementation>(IServiceCollection services,
                                                                        Func<IServiceProvider, string> baseUrlResolver)
            where TService : class
            where TImplementation : class, TService {
            services.AddHttpClient();
            
            services.AddTransient<TService, TImplementation>(serviceProvider => {
                var baseUrl = baseUrlResolver(serviceProvider);
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient();
                
                var implementation = (TImplementation) Activator.CreateInstance(typeof(TImplementation), httpClient);
                
                typeof(TImplementation).GetProperty("BaseUrl").SetValue(implementation, baseUrl);

                return implementation;
            });
        }
    }
}