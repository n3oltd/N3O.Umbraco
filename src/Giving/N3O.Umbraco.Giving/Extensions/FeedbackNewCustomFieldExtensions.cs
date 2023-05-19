using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;

namespace N3O.Umbraco.Giving.Extensions; 

public static class FeedbackNewCustomFieldExtensions {
    public static object GetValue(this IFeedbackNewCustomField feedbackNewCustomField, FeedbackCustomFieldType type) {
        if (type == FeedbackCustomFieldTypes.Bool) {
            return feedbackNewCustomField.Bool;
        } else if (type == FeedbackCustomFieldTypes.Date) {
            return feedbackNewCustomField.Date;
        } else if (type == FeedbackCustomFieldTypes.Text) {
            return feedbackNewCustomField.Text;
        } else {
            throw UnrecognisedValueException.For(type);
        }
    }
    
    public static bool PassesValidation(this IFeedbackNewCustomField feedbackNewFeedbackCustomField,
                                        FeedbackCustomFieldElement definition) {
        if (definition.Required && !GetValue(feedbackNewFeedbackCustomField, definition.Type).HasValue()) {
            return false;
        }

        if (definition.HasValue(x => x.TextMaxLength) &&
            (GetValue(feedbackNewFeedbackCustomField, definition.Type) as string)?.Length > definition.TextMaxLength.GetValueOrThrow()) {
            return false;
        }

        return true;
    }

    public static FeedbackCustomField ToFeedbackCustomField(this IFeedbackNewCustomField feedbackNewFeedbackCustomField,
                                                            FeedbackCustomFieldElement definition) {
        var feedbackCustomField = new FeedbackCustomField(definition.Type,
                                                          definition.Alias,
                                                          definition.Name,
                                                          feedbackNewFeedbackCustomField.Bool,
                                                          feedbackNewFeedbackCustomField.Date,
                                                          feedbackNewFeedbackCustomField.Text);

        return feedbackCustomField;
    }
    
    public static FeedbackCustomField ToFeedbackCustomField(this IFeedbackNewCustomField feedbackNewFeedbackCustomField,
                                                            FeedbackScheme scheme) {
        var definition = scheme.GetFeedbackCustomFieldDefinition(feedbackNewFeedbackCustomField.Alias);
        var feedbackCustomField = ToFeedbackCustomField(feedbackNewFeedbackCustomField, definition);

        return feedbackCustomField;
    }
}