using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Extensions;
using NodaTime.Extensions;
using Umbraco.Cms.Core.Mapping;
using CampaignType = N3O.Umbraco.Cloud.Platforms.Clients.CampaignType;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CreateCampaignReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CampaignContent, CreateCampaignReq>((_, _) => new CreateCampaignReq(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(CampaignContent src, CreateCampaignReq dest, MapperContext ctx) {
        dest.Type = src.Type.ToEnum<CampaignType>();
        dest.Name = src.Name;
        dest.Notes = src.Notes;
        
        if (src.Type == CampaignTypes.Telethon) {
            dest.Telethon = new TelethonCampaignOptionsReq();
            
            dest.Telethon.Begin = src.Telethon.BeginAt.ToLocalDateTime().ToString("o", null);
            dest.Telethon.End = src.Telethon.EndAt.ToLocalDateTime().ToString("o", null);
        } else if (src.Type == CampaignTypes.ScheduledGiving) {
            dest.ScheduledGiving = new ScheduledGivingCampaignOptionsReq();
            dest.ScheduledGiving.ScheduleId = src.ScheduledGiving.Schedule.Id;
        }
    }
}