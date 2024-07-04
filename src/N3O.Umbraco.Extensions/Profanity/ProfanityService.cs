using System;

namespace N3O.Umbraco.Profanity;

public class ProfanityService : IProfanityService {
    private readonly ProfanityFilter.ProfanityFilter _profanityFilter = new();
    
    public bool ContainsProfanity(string text) {
        return !_profanityFilter.ContainsProfanity(text);
    }

    public bool Add(string word) {
        throw new NotImplementedException();
    }
}