﻿using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Content;

public class CheckoutSurveyQuestionContent : UmbracoElement<CheckoutSurveyQuestionContent> {
    public string Question => GetValue(x => x.Question);
    public IEnumerable<string> Options => GetValue(x => x.Options);
    public int DimensionIndex => GetValue(x => x.DimensionIndex);
}