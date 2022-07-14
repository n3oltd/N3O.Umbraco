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
                                             Func<IServiceProvider, string> baseUrlResolver,
                                             Func<IServiceProvider, HttpClient> httpClientFactory = null) {
            httpClientFactory ??= serviceProvider => serviceProvider.GetRequiredService<IHttpClientFactory>()
                                                                    .CreateClient();
            
            AddUmbracoClient<IAccountsClient, AccountsClient>(services, baseUrlResolver, httpClientFactory);
            
            AddUmbracoClient<IContentClient, ContentClient>(services, baseUrlResolver, httpClientFactory);
            AddUmbracoClient<IContentTypesClient, ContentTypesClient>(services, baseUrlResolver, httpClientFactory);
            AddUmbracoClient<IDataTypesClient, DataTypesClient>(services, baseUrlResolver, httpClientFactory);
            AddUmbracoClient<IExportsClient, ExportsClient>(services, baseUrlResolver, httpClientFactory);
            AddUmbracoClient<IImportsClient, ImportsClient>(services, baseUrlResolver, httpClientFactory);

            AddUmbracoClient<IGivingClient, GivingClient>(services, baseUrlResolver, httpClientFactory);
            AddUmbracoClient<ICartClient, CartClient>(services, baseUrlResolver, httpClientFactory);
            AddUmbracoClient<ICheckoutClient, CheckoutClient>(services, baseUrlResolver, httpClientFactory);
            
            AddUmbracoClient<INewslettersClient, NewslettersClient>(services, baseUrlResolver, httpClientFactory);
            
            AddUmbracoClient<IPaymentsClient, PaymentsClient>(services, baseUrlResolver, httpClientFactory);
            AddUmbracoClient<IBamboraClient, BamboraClient>(services, baseUrlResolver, httpClientFactory);
            AddUmbracoClient<IGoCardlessClient, GoCardlessClient>(services, baseUrlResolver, httpClientFactory);
            AddUmbracoClient<IOpayoClient, OpayoClient>(services, baseUrlResolver, httpClientFactory);
            AddUmbracoClient<IPayPalClient, PayPalClient>(services, baseUrlResolver, httpClientFactory);
            AddUmbracoClient<IStripeClient, StripeClient>(services, baseUrlResolver, httpClientFactory);
            
            AddUmbracoClient<ICropperClient, CropperClient>(services, baseUrlResolver, httpClientFactory);
            AddUmbracoClient<IUploaderClient, UploaderClient>(services, baseUrlResolver, httpClientFactory);
        }

        private static void AddUmbracoClient<TService, TImplementation>(IServiceCollection services,
                                                                        Func<IServiceProvider, string> baseUrlResolver,
                                                                        Func<IServiceProvider, HttpClient> httpClientFactory)
            where TService : class
            where TImplementation : class, TService {
            services.AddHttpClient();
            
            services.AddTransient<TService, TImplementation>(serviceProvider => {
                var baseUrl = baseUrlResolver(serviceProvider);
                var httpClient = httpClientFactory(serviceProvider);
                
                var implementation = (TImplementation) Activator.CreateInstance(typeof(TImplementation), httpClient);
                
                typeof(TImplementation).GetProperty("BaseUrl").SetValue(implementation, baseUrl);

                return implementation;
            });
        }
    }
}