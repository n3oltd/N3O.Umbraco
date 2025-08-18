using N3O.Umbraco.Content;
using N3O.Umbraco.Helpdesk.Services;

namespace N3O.Umbraco.Helpdesk.GitHub;

public class GitHubIssueManager : IHelpdeskIssueManager {}

public class GitHubHelpdeskWebhookReceiver {
    // Recieving the issue comment, and it will search DB to see who the comment should be sent to
}