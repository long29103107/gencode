using gencode.service.Services.Interfaces;
using gencode.service.Settings;
using System.CommandLine;

namespace gencode.SubOptions.Model;
public static class ModelCommand
{
    public static void AddModelCommand(this RootCommand rootCommand, ITemplateService templateService, Config settings)
    {
        Argument<string> nameArgument = new Argument<string>(
          name: "name",
          description: "Set name of project");

        Argument<string> pathArgument = new Argument<string>(
            name: "path",
            description: "Set path of project");

        var modelCommand = new Command("model", "Generate model");
        modelCommand.AddArgument(nameArgument);
        modelCommand.AddArgument(pathArgument);
        modelCommand.SetHandler(async (nameArgument, pathArgument) =>
        {
            await ModelAction.HandleAsync(nameArgument, pathArgument, templateService, settings);
        },
        nameArgument, pathArgument);
        rootCommand.AddCommand(modelCommand);
    }
}
