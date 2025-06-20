using MuslimHands.Website.Connect.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Exceptions;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedFeedbackDesignationMapping : PublishedDesignationTypeMapping<PublishedFeedbackDesignation> {
    protected override void Map(DesignationContent src, PublishedFeedbackDesignation dest, MapperContext ctx) {
       var feedbackDesignation = src as FeedbackDesignation;
       
       dest.Scheme = new PublishedFeedbackScheme();
       dest.Scheme.Id = feedbackDesignation.Scheme.Name.Camelize();
        
       dest.CustomFieldDefinitions = feedbackDesignation.Scheme.CustomFields.OrEmpty().Select(GetCustomField).ToList();

       if (HasPricing(feedbackDesignation.Scheme)) {
           dest.Pricing = new PublishedPricing();
           dest.Pricing.Price = ctx.Map<IPrice, PublishedPrice>(feedbackDesignation.Scheme);
           dest.Pricing.Rules = feedbackDesignation.Scheme.PriceRules.OrEmpty().Select(ctx.Map<PricingRule, PublishedPricingRule>).ToList();
       }
    }
    
    private PublishedCustomFieldDefinition GetCustomField(FeedbackCustomFieldDefinition src) {
        var customFieldDefinitionPub = new PublishedCustomFieldDefinition();
        
        customFieldDefinitionPub.Alias = src.FieldName.Camelize();
        customFieldDefinitionPub.Name = src.FieldName;
        customFieldDefinitionPub.Type = GetCustomFieldType(src.FieldType);
        customFieldDefinitionPub.Required = src.FieldRequired;
        
        customFieldDefinitionPub.Text = new PublishedTextFieldOptions();
        customFieldDefinitionPub.Text.MaxLength = src.FieldTextMaxLength;
        
        return customFieldDefinitionPub;
    }

    private CustomFieldType GetCustomFieldType(FeedbackCustomFieldType fieldType) {
        if (fieldType == FeedbackCustomFieldTypes.Text) {
            return CustomFieldType.Text;
        } else if (fieldType == FeedbackCustomFieldTypes.Bool) {
            return CustomFieldType.Bool;
        } else if (fieldType == FeedbackCustomFieldTypes.Date) {
            return CustomFieldType.Date;
        } else {
            throw UnrecognisedValueException.For(fieldType);
        }
    }
}
