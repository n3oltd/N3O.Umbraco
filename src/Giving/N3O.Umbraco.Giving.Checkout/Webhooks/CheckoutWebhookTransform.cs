using Humanizer;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.Content;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Webhooks.Transforms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Checkout.Webhooks;

public class CheckoutWebhookTransform : WebhookTransform {
    private const string Feedbacks = "feedbacks";
    private const string Sponsorships = "sponsorships";
    
    private static readonly string RestrictCollectionDaysToAlias =
        AliasHelper<PaymentMethodSettingsContent<IPaymentMethodSettings>>.PropertyAlias(x => x.RestrictCollectionDaysTo);
    
    private readonly IContentCache _contentCache;

    public CheckoutWebhookTransform(IJsonProvider jsonProvider, IContentCache contentCache) : base(jsonProvider) {
        _contentCache = contentCache;
    }

    public override bool IsTransform(object body) => body is Entities.Checkout;

    public override object Apply(object body) {
        var checkout = (Entities.Checkout) body;
        var serializer = GetJsonSerializer();
        var jObject = JObject.FromObject(checkout, serializer);

        TransformConsent(checkout, jObject);
        TransformCollectionDay(checkout, jObject);
        TransformFeedbacks(serializer, GivingTypes.Donation, checkout.Donation?.Allocations, jObject);
        TransformFeedbacks(serializer, GivingTypes.RegularGiving, checkout.RegularGiving?.Allocations, jObject);
        TransformSponsorships(serializer, GivingTypes.Donation, checkout.Donation?.Allocations, jObject);
        TransformSponsorships(serializer, GivingTypes.RegularGiving, checkout.RegularGiving?.Allocations, jObject);

        return jObject;
    }

    protected override void AddCustomConverters() {
        AddConverter<INamedLookup>(t => t.ImplementsInterface<INamedLookup>(),
                                   x => new {x.Id, x.Name});

        AddConverter<Country>(t => t == typeof(Country),
                              x => new {x.Id, x.Name, x.Iso2Code, x.Iso3Code});
    }

    private void TransformConsent(Entities.Checkout checkout, JObject jObject) {
        var choices = checkout.Account.Consent.Choices.ToList();
        var channels = choices.Select(x => x.Channel).Distinct().ToList();

        var consent = (JObject) jObject["account"]["consent"];

        foreach (var channel in channels) {
            consent[channel.Id] = new JObject();
        }

        foreach (var choice in choices) {
            consent[choice.Channel.Id][choice.Category.Id] = choice.Response.Value;
        }
    }
    
    private void TransformCollectionDay(Entities.Checkout checkout, JObject jObject) {
        if (checkout.RegularGiving.HasValue(x => x.Credential) && checkout.RegularGiving.HasValue(x => x.Options)) {
            var allowedCollectionDays = GetAllowedCollectionDays(checkout.RegularGiving.Credential.Method);

            DayOfMonth collectionDay;

            if (allowedCollectionDays.Contains(checkout.RegularGiving.Options.PreferredCollectionDay)) {
                collectionDay = checkout.RegularGiving.Options.PreferredCollectionDay;
            } else {
                collectionDay = allowedCollectionDays.First();
            }

            jObject["regularGiving"]["options"]["collectionDay"] = collectionDay.Day;
        }
    }

    private IReadOnlyList<DayOfMonth> GetAllowedCollectionDays(PaymentMethod paymentMethod) {
        var settingsContentTypeAlias = paymentMethod.GetSettingsContentTypeAlias();
        var allowedCollectionDays = new List<DayOfMonth>();
        
        var restrictedToDays = (IEnumerable<DayOfMonth>) _contentCache.Single(settingsContentTypeAlias)
                                                                      .GetProperty(RestrictCollectionDaysToAlias)
                                                                      .GetValue();

        if (restrictedToDays.HasAny()) {
            allowedCollectionDays.AddRange(restrictedToDays.OrEmpty());
        } else {
            allowedCollectionDays.AddRange(DaysOfMonth.All);
        }

        return allowedCollectionDays;
    }
    
    private void TransformFeedbacks(JsonSerializer serializer,
                                    GivingType givingType,
                                    IEnumerable<Allocation> allocations,
                                    JObject jObject) {
        foreach (var allocation in allocations.OrEmpty().Where(x => x.Type == AllocationTypes.Feedback)) {
            var key = $"{givingType.Id}{allocation.Feedback.Scheme.Id.Pascalize()}Feedbacks";

            if (!jObject.ContainsKey(key)) {
                jObject[key] = new JArray();
            }
            
            if (!jObject.ContainsKey(Feedbacks)) {
                jObject[Feedbacks] = new JArray();
            }

            var allocationJObject = JObject.FromObject(allocation, serializer);

            ((JArray) jObject[key]).Add(allocationJObject);
            ((JArray) jObject[Feedbacks]).Add(allocationJObject);
        }
    }

    private void TransformSponsorships(JsonSerializer serializer,
                                       GivingType givingType,
                                       IEnumerable<Allocation> allocations,
                                       JObject jObject) {
        foreach (var allocation in allocations.OrEmpty().Where(x => x.Type == AllocationTypes.Sponsorship)) {
            var key = $"{givingType.Id}{allocation.Sponsorship.Scheme.Id.Pascalize()}Sponsorships";

            if (!jObject.ContainsKey(key)) {
                jObject[key] = new JArray();
            }
            
            if (!jObject.ContainsKey(Sponsorships)) {
                jObject[Sponsorships] = new JArray();
            }

            var allocationJObject = JObject.FromObject(allocation, serializer);
            var components = (JArray) allocationJObject["sponsorship"]["components"];

            SetSponsorshipValues(allocation.Value,
                                 allocation.Sponsorship.Duration?.Months,
                                 (k, v) => allocationJObject["sponsorship"][k] = JObject.FromObject(v, serializer));

            foreach (var (component, index) in components.SelectWithIndex()) {
                var componentValue = allocation.Sponsorship.Components.ElementAt(index).Value;
                
                SetSponsorshipValues(componentValue,
                                     allocation.Sponsorship.Duration?.Months,
                                     (k, v) => component[k] = JObject.FromObject(v, serializer));
            }

            ((JArray) jObject[key]).Add(allocationJObject);
            ((JArray) jObject[Sponsorships]).Add(allocationJObject);
        }
    }

    private void SetSponsorshipValues(Money value, int? durationMonths, Action<string, Money> setProperty) {
        setProperty("totalValue", value);

        if (durationMonths != null) {
            var monthlyValue = value.SafeDivide(durationMonths.Value).Distinct().Single();
            
            setProperty("monthlyValue", monthlyValue);
        }
    }
}
