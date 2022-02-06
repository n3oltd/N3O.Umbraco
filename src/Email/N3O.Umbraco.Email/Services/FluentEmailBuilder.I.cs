using N3O.Umbraco.Email.Models;

namespace N3O.Umbraco.Email {
    public interface IFluentEmailBuilder {
        IFluentEmailBuilder From(string email, string name = null);
        IFluentEmailBuilder To(string email, string name = null);
        IFluentEmailBuilder Cc(string email, string name = null);
        IFluentEmailBuilder Bcc(string email, string name = null);

        IFluentEmailBuilder Subject(string text);
        IFluentEmailBuilder Body(string content);
        IFluentEmailBuilder Model(object model);

        SendEmailReq Build();
        void Queue();
    }
}
