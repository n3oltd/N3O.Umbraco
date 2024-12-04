using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Content;

public class CheckoutSurveyContent : UmbracoContent<CheckoutSurveyContent> {
    public IEnumerable<CheckoutSurveyQuestionContent> Questions => GetNestedAs(x => x.Questions);
}