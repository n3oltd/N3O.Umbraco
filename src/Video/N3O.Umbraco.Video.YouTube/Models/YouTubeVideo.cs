using N3O.Umbraco.Extensions;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Video.YouTube.Models {
    public class YouTubeVideo : Value {
        public YouTubeVideo(string id,
                            string title,
                            string description,
                            IEnumerable<string> keywords,
                            string url,
                            LocalDateTime uploadedAt,
                            TimeSpan? duration,
                            YouTubeThumbnail thumbnail) {
            Id = id;
            Title = title;
            Description = description;
            Keywords = keywords.OrEmpty().ToList();
            Url = url;
            UploadedAt = uploadedAt;
            Duration = duration;
            Thumbnail = thumbnail;
        }

        public string Id { get; }
        public string Title { get; }
        public string Description { get; }
        public IReadOnlyList<string> Keywords { get; }
        public string Url { get; }
        public LocalDateTime UploadedAt { get; }
        public TimeSpan? Duration { get; }
        public YouTubeThumbnail Thumbnail { get; }
    }
}