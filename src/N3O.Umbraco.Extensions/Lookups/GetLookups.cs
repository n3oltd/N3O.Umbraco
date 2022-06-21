using N3O.Umbraco.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Lookups;

public class GetLookups<TLookupsRes> where TLookupsRes : LookupsRes, new() {
    private readonly ILookups _lookups;
    private readonly IUmbracoMapper _mapper;

    public GetLookups(ILookups lookups, IUmbracoMapper mapper) {
        _lookups = lookups;
        _mapper = mapper;
    }

    public async Task<TLookupsRes> RunAsync(LookupsCriteria criteria, CancellationToken cancellationToken = default) {
        var lookupInfos = criteria.Types.OrEmpty().ToList();
        
        if (lookupInfos.None()) {
            lookupInfos.AddRange(await _lookups.GetAllAsync<LookupInfo>(cancellationToken));
        }
        
        var lookupsRes = new TLookupsRes();
        var resProperties = lookupsRes.GetType().GetAllProperties();

        foreach (var resProperty in resProperties) {
            var attribute = resProperty.GetCustomAttribute<FromLookupTypeAttribute>();

            if (attribute == null) {
                throw new Exception($"Property {resProperty.Name} of {typeof(TLookupsRes).Name} is missing a required {nameof(FromLookupTypeAttribute)} attribute");
            }

            var lookupInfo = lookupInfos.SingleOrDefault(x => x.Id.EqualsInvariant(attribute.LookupTypeId));
            
            if (lookupInfo != null) {
                var resultsListType = resProperty.PropertyType;
                
                if (!resultsListType.IsSubclassOrSubInterfaceOfGenericType(typeof(IEnumerable<>))) {
                    throw new Exception($"Results list type {resultsListType.GetFriendlyName()} must inherit from {nameof(IEnumerable<LookupRes>)}");
                }
                
                var resType = resultsListType.GetParameterTypesForGenericInterface(typeof(IEnumerable<>)).Single();
                
                var lookups = await _lookups.GetAllAsync(lookupInfo.LookupType, cancellationToken);
                var list = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(resType));

                foreach (var lookup in lookups) {
                    var lookupRes = Activator.CreateInstance(resType);
                    
                    _mapper.CallMethod(nameof(IUmbracoMapper.Map))
                           .OfGenericType(lookupInfo.LookupType)
                           .OfGenericType(resType)
                           .WithParameter(lookupInfo.LookupType, lookup)
                           .WithParameter(resType, lookupRes)
                           .Run();
                    
                    list.Add(lookupRes);
                }
                
                resProperty.SetValue(lookupsRes, list);
            }
        }

        return lookupsRes;
    }
}
