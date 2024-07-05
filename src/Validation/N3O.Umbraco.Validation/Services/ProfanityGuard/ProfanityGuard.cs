using System;

namespace N3O.Umbraco.Validation;

public class ProfanityGuard : IProfanityGuard {
    private readonly ProfanityFilter.ProfanityFilter _profanityFilter = new();
    
    public bool Add(string word) {
        throw new NotImplementedException();
    }
    
    public bool ContainsProfanity(string text) {
        return !_profanityFilter.ContainsProfanity(text);
    }
}