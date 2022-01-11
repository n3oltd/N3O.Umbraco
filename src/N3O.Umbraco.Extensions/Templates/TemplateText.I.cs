namespace N3O.Umbraco.Templates {
    public interface ITemplateText {
        string Get(string s);
        void SetTemplateName(string name);
    }
}