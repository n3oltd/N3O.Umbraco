using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Exceptions;

public class UnrecognisedValueException : Exception {
    private UnrecognisedValueException(string message) : base(message) { }

    public static UnrecognisedValueException For<T>(T value) {
        return new UnrecognisedValueException($"Unrecognised value {value} for {typeof(T).GetFriendlyName()}");
    }
}
