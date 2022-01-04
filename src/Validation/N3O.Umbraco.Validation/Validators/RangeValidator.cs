using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Validation.Validators {
    public class RangeValidator<T> : ModelValidator<Range<T>> {
        public RangeValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x)
                .Must(RangeIsValid)
                .WithMessage(Get<Strings>(s => s.ToMustBeGreaterThanEqualFrom));
        }

        private bool RangeIsValid(Range<T> range) {
            if (!range.HasFromValue() || !range.HasToValue()) {
                return true;
            }

            object fromValue = range.From;
            object toValue = range.To;
            var typeParameter = typeof(T);

            if (typeParameter.IsNullableType()) {
                typeParameter = typeParameter.GetValueTypeForNullableType();
                fromValue = Convert.ChangeType(fromValue, typeParameter);
                toValue = Convert.ChangeType(toValue, typeParameter);
            }

            if (!typeParameter.ImplementsInterface<IComparable>()) {
                return true;
            }

            return RangeIsValid((IComparable)fromValue, (IComparable)toValue);
        }

        private bool RangeIsValid(IComparable from, IComparable to) {
            var order = from.CompareTo(to);

            return order <= 0;
        }

        public class Strings : ValidationStrings {
            public string ToMustBeGreaterThanEqualFrom => $"The {nameof(IRange.To)} value must be greater than or equal to the {nameof(IRange.From)} value";
        }
    }
}