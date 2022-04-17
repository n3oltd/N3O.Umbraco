using N3O.Umbraco.Video.YouTube.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Video.YouTube {
    public interface IYouTube {
        Task<YouTubeChannel> GetChannelByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}