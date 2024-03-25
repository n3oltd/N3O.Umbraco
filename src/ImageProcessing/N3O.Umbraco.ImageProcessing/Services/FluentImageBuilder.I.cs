using SixLabors.ImageSharp;
using System;
using System.IO;

namespace N3O.Umbraco.ImageProcessing;

public interface IFluentImageBuilder {
    Image LoadMediaImage(string srcPath);
    string PublishToUrl(Action<Image, Stream> save, string filename);
    T Save<T>(Func<Image, T> save);
    
    IImageProcessor Processor { get; }
}