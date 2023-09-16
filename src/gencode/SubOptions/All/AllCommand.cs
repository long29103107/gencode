using gencode.service.Services.Interfaces;
using gencode.service.Settings;
using System.CommandLine;

namespace gencode.SubOptions.All;
public static class AllCommand
{
    public static void AddAllCommand(this RootCommand rootCommand, ITemplateService templateService, Config config)
    {
        Argument<string> nameArgument = new Argument<string>(
          name: "name",
          description: "Set name of project");

        Argument<string> pathArgument = new Argument<string>(
            name: "path",
            description: "Set path of project");

        var allCommand = new Command("all", "Generate all(api, model, repository, service)");
        allCommand.AddArgument(nameArgument);
        allCommand.AddArgument(pathArgument);
        allCommand.SetHandler(async (nameArgument, pathArgument) =>
        {
            await AllAction.HandleAsync(nameArgument, pathArgument, templateService, config);
        },
        nameArgument, pathArgument);
        rootCommand.AddCommand(allCommand);
    }
}
