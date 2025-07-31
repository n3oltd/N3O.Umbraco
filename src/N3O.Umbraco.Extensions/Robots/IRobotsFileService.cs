using System.Threading.Tasks;

namespace N3O.Umbraco.Robots;

public interface IRobotsFileService {
    Task SaveRobotsFileToWwwroot();
}