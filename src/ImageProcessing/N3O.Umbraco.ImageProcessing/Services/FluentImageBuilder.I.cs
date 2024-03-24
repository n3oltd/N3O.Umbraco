using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace N3O.Umbraco.ImageProcessing;

public interface IFluentImageBuilder {
    Task<string> PublishToUrl(Func<Image, Task<Stream>> saveAsync, string filename);
    Task<T> SaveAsync<T>(Func<Image, Task<T>> saveAsync);
    
    IImageProcessor Processor { get; }
}