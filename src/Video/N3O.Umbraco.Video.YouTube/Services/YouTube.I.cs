using N3O.Umbraco.Video.YouTube.Criteria;
using N3O.Umbraco.Video.YouTube.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Video.YouTube {
    public interface IYouTube {
        Task<IReadOnlyList<YouTubeVideo>> FindVideosAsync(string channelId,
                                                          YouTubeVideoCriteria criteria,
                                                          CancellationToken cancellationToken = default);
        Task<YouTubeChannel> GetChannelByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}