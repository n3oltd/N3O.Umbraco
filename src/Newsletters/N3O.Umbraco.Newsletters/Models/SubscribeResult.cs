using Newtonsoft.Json;

namespace N3O.Umbraco.Newsletters.Models;

public class SubscribeResult : Value {
    [JsonConstructor]
    public SubscribeResult(bool subscribed, string errorMessage, string errorDetails) {
        Subscribed = subscribed;
        ErrorMessage = errorMessage;
        ErrorDetails = errorDetails;
    }

    public SubscribeResult() : this(true, null, null) { }

    public SubscribeResult(string errorMessage, string errorDetails) : this(false, errorMessage, errorDetails) { }

    public bool Subscribed { get; }
    public string ErrorMessage { get; }

    [JsonIgnore]
    public string ErrorDetails { get; }

    public static SubscribeResult ForFailure(string message, string details) {
        return new SubscribeResult(message, details);
    }

    public static SubscribeResult ForSuccess() {
        return new SubscribeResult();
    }
}
