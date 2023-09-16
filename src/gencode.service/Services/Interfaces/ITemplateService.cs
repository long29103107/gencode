using gencode.service.Models;
using gencode.service.Models.Templates;
using gencode.service.Settings;

namespace gencode.service.Services.Interfaces;
public interface ITemplateService
{
    Task GenerateAsync(string service, Config config);
    Task GenerateApiAsync(string service, Config config);
    Task GenerateModelAsync(string service, Config config);
    Task GenerateRepositoryAsync(string service, Config config);
    Task GenerateServiceAsync(string service, Config config);
}
