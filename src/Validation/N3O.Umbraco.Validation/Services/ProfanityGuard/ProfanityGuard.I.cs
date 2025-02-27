namespace N3O.Umbraco.Validation;

public interface IProfanityGuard {
    bool Add(string text);
    bool ContainsProfanity(string text);
    bool HasAnyProfanity(string text);
}