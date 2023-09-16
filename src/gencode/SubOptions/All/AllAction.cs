using Autofac.Extensions.DependencyInjection;
using gencode.service.Services.Interfaces;
using gencode.service.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace gencode.SubOptions.All;
public static class AllAction
{
    public static async Task HandleAsync(string serviceName, string pathArgument, ITemplateService templateService, Config settings)
    {      
        settings.ConfigPathSettings.PathRoot = pathArgument;
        settings.ConfigPathSettings.PathDBML = settings.ConfigPathSettings.PathRoot + settings.ConfigPathSettings.PathDBML;
        await templateService.GenerateAsync(serviceName, settings);
        await templateService.GenerateApiAsync(serviceName, settings);
        await templateService.GenerateModelAsync(serviceName, settings);
        await templateService.GenerateRepositoryAsync(serviceName, settings);
        await templateService.GenerateServiceAsync(serviceName, settings);
    }
}
