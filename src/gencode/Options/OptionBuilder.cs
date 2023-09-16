using gencode.service.Services.Interfaces;
using gencode.service.Settings;
using gencode.SubOptions.All;
using gencode.SubOptions.Api;
using gencode.SubOptions.Model;
using gencode.SubOptions.Repository;
using gencode.SubOptions.Service;
using System.CommandLine;

namespace gencode.Options;
public static class OptionBuilder
{
    public static void CreateOptions(this RootCommand rootCommand, ITemplateService templateService, Config config)
    {
        rootCommand.AddApiCommand(templateService, config);
        rootCommand.AddModelCommand(templateService, config);
        rootCommand.AddRepositoryCommand(templateService, config);
        rootCommand.AddServiceCommand(templateService, config);
        rootCommand.AddAllCommand(templateService, config);
    }
}
