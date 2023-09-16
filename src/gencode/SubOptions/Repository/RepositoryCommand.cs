using gencode.service.Services.Interfaces;
using gencode.service.Settings;
using System.CommandLine;

namespace gencode.SubOptions.Repository;
public static class RepositoryCommand
{
    public static void AddRepositoryCommand(this RootCommand rootCommand, ITemplateService templateService, Config settings)
    {
        Argument<string> nameArgument = new Argument<string>(
          name: "name",
          description: "Set name of project");

        Argument<string> pathArgument = new Argument<string>(
            name: "path",
            description: "Set path of project");

        var repositoryCommand = new Command("repository", "Generate repository");
        repositoryCommand.AddArgument(nameArgument);
        repositoryCommand.AddArgument(pathArgument);
        repositoryCommand.SetHandler(async (nameArgument, pathArgument) =>
        {
            await RepositoryAction.HandleAsync(nameArgument, pathArgument, templateService, settings);
        },
        nameArgument, pathArgument);
        rootCommand.AddCommand(repositoryCommand);
    }
}
