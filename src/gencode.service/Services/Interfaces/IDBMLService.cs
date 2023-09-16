using gencode.service.Models;

namespace gencode.service.Services.Interfaces;
public interface IDBMLService
{
    Dictionary<string, Entity> GetEntities(List<Line> lines);
}
