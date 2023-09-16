using gencode.service.Models;
using gencode.service.Models.Templates;

namespace gencode.service.Services.Interfaces;
public interface IFileService
{
    Task<List<Line>> ReadAsync(string path);

    Task CreateFileAsync(string path, ModelTemplate chilren, string pathTemplate);
}
