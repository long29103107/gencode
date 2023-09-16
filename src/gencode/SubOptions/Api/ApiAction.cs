using gencode.service.Services.Interfaces;
using gencode.service.Settings;

namespace gencode.SubOptions.Api;
public static class ApiAction
{
    public static async Task HandleAsync(string serviceName, string pathArgument, ITemplateService templateService, Config settings)
    {
        settings.ConfigPathSettings.PathRoot = pathArgument;
        settings.ConfigPathSettings.PathDBML = settings.ConfigPathSettings.PathRoot + settings.ConfigPathSettings.PathDBML;
        await templateService.GenerateAsync(serviceName, settings);
        await templateService.GenerateApiAsync(serviceName, settings);
    }
}
