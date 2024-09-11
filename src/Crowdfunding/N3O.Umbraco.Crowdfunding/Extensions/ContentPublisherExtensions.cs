using N3O.Umbraco.Content;
using N3O.Umbraco.Crm;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Giving;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class ContentPublisherExtensions {
    public static void PopulateFundraiser(this IContentPublisher contentPublisher,
                                          IContentLocator contentLocator,
                                          IAccountInfoAccessor accountInfoAccessor,
                                          CreateFundraiserReq req,
                                          IPublishedContent member) {
        var accountReference = accountInfoAccessor.GetReference();

        if (!accountReference.HasValue()) {
            throw new Exception("Could not resolve a valid account reference when populating fundraiser");
        }
        
        var campaign = contentLocator.ById<CampaignContent>(req.CampaignId.GetValueOrThrow());
        
        contentPublisher.Content.Label(Fundraiser.Properties.Slug).Set(req.Slug);
        contentPublisher.Content.Label(Fundraiser.Properties.Title).Set(req.Title);
        contentPublisher.Content.Label(Fundraiser.Properties.AccountReference).Set(accountReference);
        contentPublisher.Content.Label(Fundraiser.Properties.DisplayName).Set(req.Name);
        contentPublisher.Content.DateTime(Fundraiser.Properties.EndDate).SetDate(req.EndDate);
        contentPublisher.Content.Label(Fundraiser.Properties.Status).Set(FundraiserStatuses.Pending.Name);
        contentPublisher.Content.ContentPicker(Fundraiser.Properties.Owner).SetMember(member);
        contentPublisher.Content.ContentPicker(Fundraiser.Properties.Campaign).SetContent(campaign);
        
        PopulateFundraiserGoals(contentPublisher, req.Goals, campaign);
    }
    
    public static void PopulateFundraiserGoals(this IContentPublisher contentPublisher,
                                               IEnumerable<FundraiserGoalReq> reqs,
                                               CampaignContent campaign) {
        var nestedContent = contentPublisher.Content.Nested(Fundraiser.Properties.Goals);
        
        foreach (var req in reqs) {
            var campaignGoal = campaign.Goals.SingleOrDefault(x => x.GoalId == req.GoalId.GetValueOrThrow());

            if (campaignGoal == null) {
                throw new Exception($"No campaign goal found with ID {req.GoalId}");
            }
            
            if (campaignGoal.Type == AllocationTypes.Fund) {
                AddFundraiserFundGoal(nestedContent, req, campaignGoal);
            } else if (campaignGoal.Type == AllocationTypes.Feedback) {
                AddFundraiserFeedbackGoal(nestedContent, req, campaignGoal);
            } else {
                throw UnrecognisedValueException.For(campaignGoal.Type);
            }
        }
    }
    
    private static void AddFundraiserFundGoal(NestedPropertyBuilder nestedPropertyBuilder,
                                              FundraiserGoalReq req,
                                              GoalElement campaignGoal) {
        var contentBuilder = nestedPropertyBuilder.Add(Goal.Fund.Alias, campaignGoal.GoalId);
        
        PopulateFundraiserGoal(contentBuilder, req, campaignGoal);
        
        contentBuilder.ContentPicker(Goal.Fund.Properties.DonationItem)
                      .SetContent(campaignGoal.Fund.DonationItem);
    }
    
    private static void AddFundraiserFeedbackGoal(NestedPropertyBuilder nestedPropertyBuilder,
                                                  FundraiserGoalReq req,
                                                  GoalElement campaignGoal) {
        var contentBuilder = nestedPropertyBuilder.Add(Goal.Feedback.Alias);
        
        PopulateFundraiserGoal(contentBuilder, req, campaignGoal);
        
        contentBuilder.ContentPicker(Goal.Feedback.Properties.Scheme).SetContent(campaignGoal.Feedback.Scheme);

        PopulateCustomFields(contentBuilder,
                             campaignGoal.Feedback.Scheme,
                             req.Feedback.OrEmpty(x => x?.CustomFields.Entries));
    }
    
    private static void PopulateFundraiserGoal(IContentBuilder contentBuilder,
                                               FundraiserGoalReq req,
                                               GoalElement campaignGoal) {
        contentBuilder.Numeric(Goal.Properties.Amount).SetDecimal(req.Amount);
        contentBuilder.TextBox(Goal.Properties.Title).Set(campaignGoal.Title);
        contentBuilder.ContentPicker(Goal.Properties.FundDimension1).SetContent(campaignGoal.FundDimension1);
        contentBuilder.ContentPicker(Goal.Properties.FundDimension2).SetContent(campaignGoal.FundDimension2);
        contentBuilder.ContentPicker(Goal.Properties.FundDimension3).SetContent(campaignGoal.FundDimension3);
        contentBuilder.ContentPicker(Goal.Properties.FundDimension4).SetContent(campaignGoal.FundDimension4);
        contentBuilder.ContentPicker(Goal.Properties.Tags).SetContent(campaignGoal.Tags);
        
        var priceHandlesBuilder = contentBuilder.Nested(Goal.Properties.PriceHandles);

        foreach (var priceHandle in campaignGoal.PriceHandles.OrEmpty()) {
            AddPriceHandle(priceHandlesBuilder, priceHandle);
        }
    }
    
    private static void PopulateCustomFields(IContentBuilder contentBuilder,
                                             FeedbackScheme feedbackScheme,
                                             IEnumerable<FeedbackNewCustomFieldReq> newCustomFields) {
        var nestedContent = contentBuilder.Nested(Goal.Feedback.Properties.CustomFields);

        foreach (var newCustomField in newCustomFields) {
            var customFieldBuilder = nestedContent.Add(GivingConstants.Aliases.FeedbackCustomField.ContentType);
            var feedbackCustomField = newCustomField.ToFeedbackCustomField(feedbackScheme);
            
            customFieldBuilder.TextBox(GivingConstants.Aliases.FeedbackCustomField.Properties.Alias).Set(newCustomField.Alias);
            customFieldBuilder.DataList(GivingConstants.Aliases.FeedbackCustomField.Properties.Type).SetLookups(feedbackCustomField.Type);
            customFieldBuilder.TextBox(GivingConstants.Aliases.FeedbackCustomField.Properties.Name).Set(feedbackCustomField.Name);

            if (feedbackCustomField.Type == FeedbackCustomFieldTypes.Bool) {
                customFieldBuilder.Boolean(GivingConstants.Aliases.FeedbackCustomField.Properties.Bool).Set(feedbackCustomField.Bool);
            } else if (feedbackCustomField.Type == FeedbackCustomFieldTypes.Date) {
                customFieldBuilder.DateTime(GivingConstants.Aliases.FeedbackCustomField.Properties.Date).SetDate(feedbackCustomField.Date);
            }  else if (feedbackCustomField.Type == FeedbackCustomFieldTypes.Text) {
                customFieldBuilder.TextBox(GivingConstants.Aliases.FeedbackCustomField.Properties.Text).Set(feedbackCustomField.Text);
            } else {
                throw UnrecognisedValueException.For(feedbackCustomField.Type);
            }
        }
    }
    
    private static void AddPriceHandle(NestedPropertyBuilder propertyBuilder, PriceHandleElement priceHandleElement) {
        var contentBuilder = propertyBuilder.Add(GivingConstants.Aliases.PriceHandle.ContentType);
        
        contentBuilder.Numeric(GivingConstants.Aliases.PriceHandle.Properties.Amount)
                      .SetDecimal(priceHandleElement.Amount);
        
        contentBuilder.TextBox(GivingConstants.Aliases.PriceHandle.Properties.Description)
                      .Set(priceHandleElement.Description);
    }
}