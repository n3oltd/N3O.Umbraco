using SixLabors.ImageSharp.Processing;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.ImageProcessing.Operations;

public interface IImageOperation {
    Task ApplyAsync(IPublishedElement options, IImageProcessingContext image);
    bool IsOperation(IPublishedElement options);
}