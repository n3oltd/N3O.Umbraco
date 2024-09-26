using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Models.AddToCart;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving;
using N3O.Umbraco.Giving.Cart;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class AddToCartHandler : IRequestHandler<AddToCartCommand, AddToCartReq, RevisionId> {
    private readonly IContentLocator _contentLocator;
    private readonly ICartAccessor _cartAccessor;
    private readonly IRepository<N3O.Umbraco.Giving.Cart.Entities.Cart> _repository;
    private readonly IValidation _validation;
    private readonly IValidationHandler _validationHandler;
    private readonly IForexConverter _forexConverter;
    private readonly IPriceCalculator _priceCalculator;
    private readonly IJsonProvider _jsonProvider;

    public AddToCartHandler(IContentLocator contentLocator,
                            ICartAccessor cartAccessor,
                            IRepository<N3O.Umbraco.Giving.Cart.Entities.Cart> repository,
                            IValidation validation,
                            IValidationHandler validationHandler,
                            IForexConverter forexConverter,
                            IPriceCalculator priceCalculator,
                            IJsonProvider jsonProvider) {
        _contentLocator = contentLocator;
        _cartAccessor = cartAccessor;
        _repository = repository;
        _validation = validation;
        _validationHandler = validationHandler;
        _forexConverter = forexConverter;
        _priceCalculator = priceCalculator;
        _jsonProvider = jsonProvider;
    }
    
    public async Task<RevisionId> Handle(AddToCartCommand req, CancellationToken cancellationToken) {
        ICrowdfunderContent crowdfunderContent;
        var cart = _cartAccessor.Get();
        
        if (req.Model.Type == CrowdfunderTypes.Fundraiser) {
            crowdfunderContent = _contentLocator.ById<CampaignContent>(req.Model.Crowdfunding.CrowdfunderId.GetValueOrThrow());
        } else if (req.Model.Type == CrowdfunderTypes.Fundraiser) {
            crowdfunderContent = _contentLocator.ById<FundraiserContent>(req.Model.Crowdfunding.CrowdfunderId.GetValueOrThrow());
        } else {
            throw UnrecognisedValueException.For(req.Model.Type);
        } 
        
        foreach (var goal in req.Model.Items) {
            var goalContent = crowdfunderContent.Goals.Single(x => x.GoalId == goal.GoalId);
            FeedbackAllocation feedbackAllocation = null;
            FundAllocation fundAllocation = null;
            
            var isGoalTypeFeedback = goalContent.Type == AllocationTypes.Feedback;
            var isGoalTypeFund = goalContent.Type == AllocationTypes.Fund;
            
            var feedbackCustomFields = new List<FeedbackNewCustomFieldReq>();

            if (isGoalTypeFeedback) {
                feedbackAllocation = new FeedbackAllocation(goalContent.Feedback.Scheme,
                                                            goal.CustomFields.Entries.Select(x => x.ToFeedbackCustomField(goalContent.Feedback.Scheme)));
                
                var goalFeedbackCustomFields = goalContent.Feedback.CustomFields;

                feedbackCustomFields.AddRange(goalFeedbackCustomFields.Select(customField => new FeedbackNewCustomFieldReq(customField)));
            }

            if (isGoalTypeFund) {
                fundAllocation = new FundAllocation(goalContent.Fund.DonationItem);
            }
            
            var allocationReq = new AllocationReq {
                                                      Type = goalContent.Type,
                                                      Value = goal.Value,
                                                      FundDimensions = new FundDimensionValuesReq {
                                                                                                      Dimension1 = goalContent.FundDimensions.Dimension1,
                                                                                                      Dimension2 = goalContent.FundDimensions.Dimension2,
                                                                                                      Dimension3 = goalContent.FundDimensions.Dimension3,
                                                                                                      Dimension4 = goalContent.FundDimensions.Dimension4
                                                                                                  },
                                                      Fund = fundAllocation != null ? new FundAllocationReq {
                                                                                                                DonationItem = fundAllocation.DonationItem,
                                                                                                            } : null,
                                                      Feedback = feedbackAllocation != null ? new FeedbackAllocationReq {
                                                                                                                            Scheme = goalContent.Feedback.Scheme,
                                                                                                                            CustomFields = new FeedbackNewCustomFieldsReq {
                                                                                                                                                                            Entries = feedbackCustomFields
                                                                                                                                                                          }
                                                                                                                        } : null
                                                  };
            
            var validationFailure = await _validation.ValidateModelAsync(allocationReq);

            if (validationFailure.HasAny()) {
                _validationHandler.Handle(validationFailure);
            }
            
            var allocation = new Allocation(goalContent.Type,
                                            goal.Value,
                                            new FundDimensionValues(goalContent.FundDimensions),
                                            fundAllocation,
                                            null,
                                            feedbackAllocation,
                                            null);

            var crowdfundingReq = new CrowdfunderData(req.Model.Crowdfunding.CrowdfunderId.GetValueOrThrow(),
                                                      req.Model.Type,
                                                      req.Model.Crowdfunding.Comment,
                                                      req.Model.Crowdfunding.Anonymous.GetValueOrThrow());
            
            var serializerSettings = _jsonProvider.GetSettings();
            var jsonSerializer = JsonSerializer.Create(serializerSettings);
            
            var extensions = new Dictionary<string, JToken>();
            extensions.Add(CrowdfundingConstants.Allocations.Extensions.Key, JToken.FromObject(crowdfundingReq, jsonSerializer));

            var allocationWithExtension = new Allocation(allocation, extensions);
            
            await cart.AddAsync(_contentLocator,
                                _forexConverter,
                                _priceCalculator,
                                GivingTypes.Donation,
                                allocationWithExtension);
        }
        
        await _repository.UpdateAsync(cart);
        
        return cart.RevisionId;
    }
}