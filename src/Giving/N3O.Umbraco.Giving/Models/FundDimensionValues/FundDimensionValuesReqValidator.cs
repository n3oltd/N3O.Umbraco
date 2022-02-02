using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace N3O.Giving.Models {
    public class FundDimensionValuesReqValidator : ModelValidator<FundDimensionValuesReq> {
        public FundDimensionValuesReqValidator(IFormatter formatter, IFundStructureAccessor fundStructureAccessor)
            : base(formatter) {
            var fundStructure = fundStructureAccessor.GetFundStructure();
            
            ValidateDimension(x => x.Dimension1, fundStructure.Dimension1);
            ValidateDimension(x => x.Dimension2, fundStructure.Dimension2);
            ValidateDimension(x => x.Dimension3, fundStructure.Dimension3);
            ValidateDimension(x => x.Dimension4, fundStructure.Dimension4);
        }

        private void ValidateDimension<T, TValue>(Expression<Func<FundDimensionValuesReq, TValue>> expression,
                                                  FundDimension<T, TValue> fundDimension)
            where T : FundDimension<T, TValue>
            where TValue : FundDimensionValue<TValue> {
            RuleFor(expression)
                .NotNull()
                .When(_ => fundDimension.IsActive)
                .WithMessage(_ => Get<Strings>(s => s.SpecifyDimensionValue_1,
                                               fundDimension.Name));
        
            RuleFor(expression)
                .Must(x => fundDimension.Options.Contains(x))
                .When(x => fundDimension.IsActive &&
                           expression.Compile().Invoke(x).HasValue())
                .WithMessage(x => Get<Strings>(s => s.InvalidDimensionValue_2,
                                               expression.Compile().Invoke(x).Name,
                                               fundDimension.Name));
        }
        
        public class Strings : ValidationStrings {
            public string InvalidDimensionValue_2 => $"{"{0}".Quote()} is not an allowed value for {"{1}".Quote()}";
            public string SpecifyDimensionValue_1 => $"Please specify a value for {"{0}".Quote()}";
        }
    }
}