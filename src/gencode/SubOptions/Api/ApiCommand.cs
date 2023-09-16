using gencode.service.Services.Interfaces;
using gencode.service.Settings;
using System.CommandLine;

namespace gencode.SubOptions.Api;
public static class ApiCommand
{
    public static void AddApiCommand(this RootCommand rootCommand, ITemplateService templateService, Config settings)
    {
        Argument<string> nameArgument = new Argument<string>(
          name: "name",
          description: "Set name of project");

        Argument<string> pathArgument = new Argument<string>(
            name: "path",
            description: "Set path of project");

        var apiCommand = new Command("api", "Generate api");
        apiCommand.AddArgument(nameArgument);
        apiCommand.AddArgument(pathArgument);
        apiCommand.SetHandler(async (nameArgument, pathArgument) =>
        {
            await ApiAction.HandleAsync(nameArgument, pathArgument, templateService, settings);
        },
        nameArgument, pathArgument);
        rootCommand.AddCommand(apiCommand);
    }
}
