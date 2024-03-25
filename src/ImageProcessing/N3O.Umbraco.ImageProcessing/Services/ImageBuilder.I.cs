using SixLabors.ImageSharp;
using System.Threading.Tasks;

namespace N3O.Umbraco.ImageProcessing;

public interface IImageBuilder {
    IFluentImageBuilder Create(int width, int height);
    IFluentImageBuilder Create(Size size);
    Task<IFluentImageBuilder> CreateAsync(string srcPath);
}