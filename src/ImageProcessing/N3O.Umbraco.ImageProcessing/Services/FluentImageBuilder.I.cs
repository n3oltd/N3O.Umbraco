using SixLabors.ImageSharp;
using System;

namespace N3O.Umbraco.ImageProcessing;

public interface IFluentImageBuilder {
    T Do<T>(Func<Image, T> action);
    void Do(Action<Image> action);
    Image LoadMediaImage(string srcPath);
    
    IImageProcessor Processor { get; }
}