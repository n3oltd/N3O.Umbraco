using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Converters {
    public class CreatedByContentMetadataConverter : ContentMetadataConverter<string> {
        private readonly IUserService _userService;
        
        public CreatedByContentMetadataConverter(IColumnRangeBuilder columnRangeBuilder, IUserService userService)
            : base(columnRangeBuilder, ContentMetadatas.CreatedBy) {
            _userService = userService;
        }

        public override object GetValue(IContent content) {
            var user = default(string);

            if (content.CreatorId != 0) {
                try {
                    user = _userService.GetUserById(content.CreatorId).Name;
                } catch { }
            }

            return user;
        }
        
        protected override string Title => "Created By";
    }
}