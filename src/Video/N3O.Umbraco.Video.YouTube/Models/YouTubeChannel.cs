using System.Collections.Generic;

namespace N3O.Umbraco.Video.YouTube.Models {
    public class YouTubeChannel : Value {
        public YouTubeChannel(string id, string name, string url, IEnumerable<YouTubeVideo> videos) {
            Id = id;
            Name = name;
            Url = url;
            Videos = videos;
        }
        
        public string Id { get; }
        public string Name { get; }
        public string Url { get; }
        public IEnumerable<YouTubeVideo> Videos { get; }
    }
}