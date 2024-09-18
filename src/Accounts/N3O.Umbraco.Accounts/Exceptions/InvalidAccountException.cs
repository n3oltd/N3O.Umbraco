using System;

namespace N3O.Umbraco.Accounts.Exceptions;

public class InvalidAccountException : Exception {
    public InvalidAccountException() : base("The selected account is not associated with your sign in email") { }
}