using N3O.Umbraco.Data.Exceptions;
using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Models;

public class ErrorLog {
    private readonly IFormatter _formatter;
    private readonly List<string> _errorMessages = [];

    public ErrorLog(IFormatter formatter) {
        _formatter = formatter;
    }
    
    public void AddError<TStrings>(Func<TStrings, string> propertySelector, params object[] formatArgs)
        where TStrings : class, IStrings, new() {
        AddError(f => f.Text.Format(propertySelector, formatArgs));
    }

    public void AddError(Func<IFormatter, string> getErrorMessage) {
        _errorMessages.Add(getErrorMessage(_formatter));
    }

    public IReadOnlyList<string> GetErrors() {
        return _errorMessages.GroupBy(x => x)
                             .Select(x => x.Count() == 1 ? x.Key : $"{x.Key} ({x.Count()})")
                             .ToList();
    }
    
    public bool HasErrors() => _errorMessages.Any();
    
    public void ThrowIfHasErrors() {
        if (HasErrors()) {
            var errors = GetErrors();

            throw new ProcessingException(errors);
        }
    }

    public override string ToString() {
        if (HasErrors()) {
            return string.Join("\n", GetErrors());
        } else {
            return "No errors";
        }
    }
}
