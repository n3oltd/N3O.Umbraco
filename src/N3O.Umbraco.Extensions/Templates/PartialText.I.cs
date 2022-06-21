namespace N3O.Umbraco.Templates;

public interface IPartialText {
    string Get(string s);
    void SetPartialName(string name);
}
