﻿using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class ContentPublisherExtensions {
    public static void PopulateFundraiser(this IContentPublisher contentPublisher,
                                          IContentLocator contentLocator,
                                          CreateFundraiserReq req,
                                          IPublishedContent member) {
        var campaign = contentLocator.ById<CampaignContent>(req.CampaignId.GetValueOrThrow());
        
        contentPublisher.Content.Label(Fundraiser.Properties.Slug).Set(req.Slug);
        contentPublisher.Content.Label(Fundraiser.Properties.Title).Set(req.Title);
        contentPublisher.Content.Label(Fundraiser.Properties.AccountReference).Set(req.AccountReference);
        contentPublisher.Content.Label(Fundraiser.Properties.DisplayName).Set(req.DisplayName);
        contentPublisher.Content.DateTime(Fundraiser.Properties.EndDate).SetDate(req.EndDate);
        contentPublisher.Content.Label(Fundraiser.Properties.Status).Set(FundraiserStatuses.Pending.Name);
        contentPublisher.Content.ContentPicker(Fundraiser.Properties.Owner).SetMember(member);
        contentPublisher.Content.ContentPicker(Fundraiser.Properties.Campaign).SetContent(campaign);
        
        PopulateAllocations(contentPublisher, req.Allocations, campaign);
    }
    
    public static void PopulateAllocations(this IContentPublisher contentPublisher,
                                           IEnumerable<FundraiserAllocationReq> allocations,
                                           CampaignContent campaign) {
        var nestedContent = contentPublisher.Content.Nested(Fundraiser.Properties.Allocations);
        
        foreach (var fundraiserAllocation in allocations) {
            var goal = campaign.Goals.Single(x => x.Content().Key == fundraiserAllocation.GoalId);
            
            if (goal.Type == AllocationTypes.Fund) {
                AddFundAllocation(nestedContent.Add(FundraiserAllocation.Fund.Alias), fundraiserAllocation, goal);
            } else if (goal.Type == AllocationTypes.Feedback) {
                AddFeedbackAllocation(nestedContent.Add(FundraiserAllocation.Feedback.Alias), fundraiserAllocation, goal);
            } else {
                throw UnrecognisedValueException.For(goal.Type);
            }
        }
    }
    
    private static void AddFundAllocation(IContentBuilder contentBuilder,
                                          FundraiserAllocationReq allocation,
                                          CampaignGoalElement campaignGoal) {
        contentBuilder.ContentPicker(FundraiserAllocation.Fund.Properties.DonationItem).SetContent(campaignGoal.Fund.DonationItem);
        
        AddAllocation(contentBuilder, allocation, campaignGoal);
    }
    
    private static void AddFeedbackAllocation(IContentBuilder contentBuilder,
                                              FundraiserAllocationReq allocation,
                                              CampaignGoalElement campaignGoal) {
        contentBuilder.ContentPicker(FundraiserAllocation.Feedback.Properties.Scheme).SetContent(campaignGoal.Feedback.Scheme);
        
        PopulateCustomFields(contentBuilder, campaignGoal.Feedback.Scheme, allocation.Feedback.CustomFields.Entries);
        AddAllocation(contentBuilder, allocation, campaignGoal);
    }
    
    private static void AddAllocation(IContentBuilder contentBuilder,
                                      FundraiserAllocationReq allocation,
                                      CampaignGoalElement campaignGoal) {
        contentBuilder.Numeric(FundraiserAllocation.Properties.Amount).SetDecimal(allocation.Amount);
        contentBuilder.TextBox(FundraiserAllocation.Properties.Title).Set(campaignGoal.Title);
        contentBuilder.Label(FundraiserAllocation.Properties.GoalID).Set(campaignGoal.Content().Key.ToString());
        contentBuilder.ContentPicker(FundraiserAllocation.Properties.FundDimension1).SetContent(campaignGoal.FundDimension1);
        contentBuilder.ContentPicker(FundraiserAllocation.Properties.FundDimension2).SetContent(campaignGoal.FundDimension2);
        contentBuilder.ContentPicker(FundraiserAllocation.Properties.FundDimension3).SetContent(campaignGoal.FundDimension3);
        contentBuilder.ContentPicker(FundraiserAllocation.Properties.FundDimension4).SetContent(campaignGoal.FundDimension4);
        contentBuilder.ContentPicker(FundraiserAllocation.Properties.Tags).SetContent(campaignGoal.Tags);
        
        var priceHandles = contentBuilder.Nested(FundraiserAllocation.Properties.PriceHandles);

        foreach (var priceHandle in campaignGoal.PriceHandles.OrEmpty()) {
            PopulatePriceHandles(priceHandles.Add(FundraiserAllocation.Properties.PriceHandle.Alias), priceHandle);
        }
    }
    
    private static void PopulateCustomFields(IContentBuilder contentBuilder,
                                             FeedbackScheme feedbackScheme,
                                             IEnumerable<FeedbackNewCustomFieldReq> customFields) {
        var nestedContent = contentBuilder.Nested(FundraiserAllocation.Feedback.Properties.CustomFields);

        foreach (var customField in customFields) {
            var customFieldBuilder = nestedContent.Add(FundraiserAllocation.Feedback.CustomField.Alias);
            var feedbackCustomField = customField.ToFeedbackCustomField(feedbackScheme);
            
            customFieldBuilder.TextBox(FundraiserAllocation.Feedback.CustomField.Properties.Alias).Set(customField.Alias);
            customFieldBuilder.DataList(FundraiserAllocation.Feedback.CustomField.Properties.Type).SetLookups(feedbackCustomField.Type);

            if (feedbackCustomField.Type == FeedbackCustomFieldTypes.Bool) {
                customFieldBuilder.Boolean(FundraiserAllocation.Feedback.CustomField.Properties.Bool).Set(feedbackCustomField.Bool);
            } else if (feedbackCustomField.Type == FeedbackCustomFieldTypes.Date) {
                customFieldBuilder.DateTime(FundraiserAllocation.Feedback.CustomField.Properties.Date).SetDate(feedbackCustomField.Date);
            }  else if (feedbackCustomField.Type == FeedbackCustomFieldTypes.Text) {
                customFieldBuilder.TextBox(FundraiserAllocation.Feedback.CustomField.Properties.Text).Set(feedbackCustomField.Text);
            } else {
                throw UnrecognisedValueException.For(customField);
            }
        }
    }
    
    private static void PopulatePriceHandles(IContentBuilder contentBuilder, PriceHandleElement priceHandleElement) {
        contentBuilder.Numeric(FundraiserAllocation.Properties.PriceHandle.Properties.Amount).SetDecimal(priceHandleElement.Amount);
        contentBuilder.TextBox(FundraiserAllocation.Properties.PriceHandle.Properties.Description).Set(priceHandleElement.Description);
    }
}