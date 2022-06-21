using N3O.Umbraco.Email.Content;

namespace N3O.Umbraco.Email.Extensions;

public static class EmailBuilderExtensions {
    public static void QueueTemplate<TTemplate, TModel>(this IEmailBuilder emailBuilder,
                                                        TTemplate template,
                                                        string to,
                                                        TModel model)
        where TTemplate : EmailTemplateContent<TTemplate> {
        emailBuilder.Create<TModel>()
                    .From(template.FromEmail, template.FromName)
                    .To(to)
                    .Subject(template.Subject)
                    .Body(template.Body)
                    .Model(model)
                    .Queue();
    }
}
