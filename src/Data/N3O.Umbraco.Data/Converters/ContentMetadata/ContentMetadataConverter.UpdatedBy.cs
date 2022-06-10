using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Converters {
    public class UpdatedByContentMetadataConverter : ContentMetadataConverter<string> {
        private readonly IUserService _userService;
        
        public UpdatedByContentMetadataConverter(IColumnRangeBuilder columnRangeBuilder, IUserService userService)
            : base(columnRangeBuilder, ContentMetadatas.UpdatedBy) {
            _userService = userService;
        }

        public override object GetValue(IContent content) {
            var user = default(string);

            if (content.WriterId != 0) {
                try {
                    user = _userService.GetUserById(content.WriterId).Name;
                } catch { }
            }

            return user;
        }
        
        protected override string Title => "Updated By";
    }
}