using gencode.service.Models;

namespace gencode.service.Services.Interfaces;
public interface ISolutionService
{
    Task<List<Line>> GetLineSolutionAsync(string path, string service);
}
