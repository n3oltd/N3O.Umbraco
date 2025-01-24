namespace N3O.Umbraco.Crowdfunding.Models;

public class JobError : Value {
    public JobError(string message, string exceptionDetails) {
        Message = message;
        ExceptionDetails = exceptionDetails;
    }

    public string Message { get; }
    public string ExceptionDetails { get; }
}
