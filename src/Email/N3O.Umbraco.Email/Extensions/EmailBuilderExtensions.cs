using N3O.Umbraco.Email.Content;

namespace N3O.Umbraco.Email.Extensions {
    public static class EmailBuilderExtensions {
        public static void QueueTemplate<T>(this IEmailBuilder emailBuilder,
                                            T template,
                                            string to,
                                            object model)
            where T : EmailTemplateContent<T> {
            emailBuilder.Create()
                        .From(template.FromEmail, template.FromName)
                        .To(to)
                        .Subject(template.Subject)
                        .Body(template.Body)
                        .Model(model)
                        .Queue();
        }
    }
}