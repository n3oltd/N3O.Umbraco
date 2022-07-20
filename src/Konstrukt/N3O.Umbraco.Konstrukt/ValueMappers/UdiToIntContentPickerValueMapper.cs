using Konstrukt.Mapping;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Konstrukt.ValueMappers;

public class UdiToIntContentPickerValueMapper : KonstruktValueMapper {
    private readonly IIdKeyMap _idKeyMap;

    public UdiToIntContentPickerValueMapper(IIdKeyMap idKeyMap) {
        _idKeyMap = idKeyMap;
    }

    public override object ModelToEditor(object input) {
        var inputStr = input?.ToString();
        
        if (!input.HasValue()) {
            return null;
        }

        var udis = inputStr.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(UdiParser.Parse).ToList();
        var ids = udis.Select(_idKeyMap.GetIdForUdi).Where(x => x.Success).Select(x => x.Result).ToList();

        return string.Join(',', ids);
    }

    public override object EditorToModel(object input) {
        var inputStr = input?.ToString();
        
        if (!input.HasValue()) {
            return null;
        }

        var ids = inputStr.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        var udis = ids.Select(x => _idKeyMap.GetUdiForId(x, UmbracoObjectTypes.Document))
                      .Where(x => x.Success)
                      .Select(x => x.Result.ToString())
                      .ToList();

        return string.Join(',', udis);
    }
}
