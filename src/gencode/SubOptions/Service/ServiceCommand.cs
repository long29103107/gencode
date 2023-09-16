using gencode.service.Services.Interfaces;
using gencode.service.Settings;
using System.CommandLine;

namespace gencode.SubOptions.Service;
public static class ServiceCommand
{
    public static void AddServiceCommand(this RootCommand rootCommand, ITemplateService templateService, Config settings)
    {
        Argument<string> nameArgument = new Argument<string>(
          name: "name",
          description: "Set name of project");

        Argument<string> pathArgument = new Argument<string>(
            name: "path",
            description: "Set path of project");

        var serviceCommand = new Command("service", "Generate service");
        serviceCommand.AddArgument(nameArgument);
        serviceCommand.AddArgument(pathArgument);
        serviceCommand.SetHandler(async (nameArgument, pathArgument) =>
        {
            await ServiceAction.HandleAsync(nameArgument, pathArgument, templateService, settings);
        },
        nameArgument, pathArgument);
        rootCommand.AddCommand(serviceCommand);
    }
}
