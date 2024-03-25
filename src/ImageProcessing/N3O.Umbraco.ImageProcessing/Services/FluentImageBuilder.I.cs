using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace N3O.Umbraco.ImageProcessing;

public interface IFluentImageBuilder {
    Task<Image> LoadImageAsync(string srcPath);
    Task<string> PublishToUrlAsync(Func<Image, Stream, Task> saveAsync, string filename);
    Task<T> SaveAsync<T>(Func<Image, Task<T>> saveAsync);
    
    IImageProcessor Processor { get; }
}