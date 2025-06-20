using Humanizer;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedFeedbackDesignationMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackDesignationContent, PublishedFeedbackDesignation>((_, _) => new PublishedFeedbackDesignation(), Map);
    }
    
    protected void Map(FeedbackDesignationContent src, PublishedFeedbackDesignation dest, MapperContext ctx) {
       dest.Scheme = new PublishedFeedbackScheme();
       dest.Scheme.Id = src.Scheme.Name.Camelize();
        
       dest.CustomFieldDefinitions = src.Scheme.CustomFields.OrEmpty().Select(GetCustomField).ToList();

       if (src.Scheme.HasPricing()) {
           dest.Pricing = new PublishedPricing();
           dest.Pricing.Price = ctx.Map<IPrice, PublishedPrice>(src.Scheme);
           dest.Pricing.Rules = src.Scheme.PriceRules.OrEmpty().Select(ctx.Map<IPricingRule, PublishedPricingRule>).ToList();
       }
    }
    
    private PublishedCustomFieldDefinition GetCustomField(FeedbackCustomFieldDefinitionElement src) {
        var customFieldDefinitionPub = new PublishedCustomFieldDefinition();
        
        customFieldDefinitionPub.Alias = src.Alias;
        customFieldDefinitionPub.Name = src.Name;
        customFieldDefinitionPub.Type = (CustomFieldType) Enum.Parse(typeof(CustomFieldType), src.Type.Id, true);
        customFieldDefinitionPub.Required = src.Required;
        
        customFieldDefinitionPub.Text = new PublishedTextFieldOptions();
        customFieldDefinitionPub.Text.MaxLength = src.TextMaxLength;
        
        return customFieldDefinitionPub;
    }
}
