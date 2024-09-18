using System;

namespace N3O.Umbraco.Exceptions;

public class InvalidAccountException : Exception {
    public InvalidAccountException() : base("Invalid account selected") { }

    public InvalidAccountException(string message) : base(message) { }

    public InvalidAccountException(string message, Exception innerException) : base(message, innerException) { }
}