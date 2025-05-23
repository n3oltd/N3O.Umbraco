namespace N3O.Umbraco.Search.Typesense;

//public class Schema {
    // Id -> Guid
    // Type -> string (contentTypeAlias)
    // Version -> int32 (umbraco content version)
    // Name -> string (for a page, this would be the title)
    // CreatedAt -> int32 (unix timestamp) 
    // LastUpdatedAt -> int32 (unix timestamp)
    // Author -> string
    // Image -> string (url of the cropped image)
    // Summary -> string
    // Path -> string (absolute path without domain)
    
    // Matching these fields should boost the result
    // PrimaryForKeywords -> string[] (if yemen is in this field, the ranking is much higher than the same keyword in content)
    // Level1Title -> string (page, from rendered html, first h1 tag content)
    // Level2Titles -> string[] (page, from rendered html, all h2 tags content)
    // Level3Titles -> string[] (page, from rendered html, all h3 tags content)
    
    // vs matches in here:
    
    // Content -> string (content, stripped of html tags -> render the blocks to HTML & then strip the tags from them (Noor/Talha))
//}