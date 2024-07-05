namespace N3O.Umbraco.Profanity;

public interface IProfanityService {
    bool ContainsProfanity(string text);
    bool Add(string text);
}