using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Models {
    public class ErrorLog {
        private readonly List<Func<IFormatter, string>> _errorMessages = new();

        public void AddError<TStrings>(Func<TStrings, string> propertySelector, params object[] formatArgs)
            where TStrings : class, IStrings, new() {
            AddError(f => f.Text.Format(propertySelector, formatArgs));
        }

        public void AddError(Func<IFormatter, string> getErrorMessage) {
            _errorMessages.Add(getErrorMessage);
        }

        public IReadOnlyList<string> GetErrors(IFormatter formatter) {
            return _errorMessages.Select(x => x(formatter))
                                 .GroupBy(x => x)
                                 .Select(x => x.Count() == 1 ? x.Key : $"{x.Key} ({x.Count()})")
                                 .ToList();
        }

        // public override string ToString() {
        //     return string.Join(GetErrors(Formatter.Invariant), "\n");
        // }

        public bool HasErrors => _errorMessages.Any();
    }
}