using Autofac.Core;
using gencode.service.Constants;
using gencode.service.Models;
using gencode.service.Models.Templates;
using gencode.service.Services.Interfaces;
using gencode.service.Settings;
using Microsoft.Build.Construction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.IO;


namespace gencode.service.Services;
public class TemplateService : ITemplateService
{
    private readonly IFolderService _folderService;
    private readonly IFileService _fileService;
    private readonly IDBMLService _dbmlService;
    private readonly ISolutionService _solutionService;

    public TemplateService(IFolderService folderService
        , IFileService fileService
        , IDBMLService dbmlService
        ,ISolutionService solutionService)
    {
        _folderService = folderService;
        _fileService = fileService;
        _dbmlService = dbmlService;
        _solutionService = solutionService;
    }
    #region GenerateAsync
    public async Task GenerateAsync(string service, Config config)
    {
        //Get Struture
        Console.WriteLine($"Path {config.ConfigPathSettings.PathStructure}");
        var jsonData = File.ReadAllText(config.ConfigPathSettings.PathStructure);
  
        var modelTemplate = JsonConvert.DeserializeObject<List<ModelTemplate>>(jsonData);
        modelTemplate.ForEach(x => x.Module = service);
        modelTemplate.ForEach(x => x.Name = x.Name.Replace("{{ModuleName}}", service));
        modelTemplate.ForEach(x => x.Path = x.Path.Replace("{{ModuleName}}", service));

        await GenFolderAndFileAsync(service, config, config.ConfigPathSettings.PathStructure);
        await AddSolutionAsync(config, modelTemplate, service);
    }
    private async Task AddSolutionAsync(Config config, List<ModelTemplate> modelTemplate, string service)
    {
        var path = Directory.GetFiles(config.ConfigPathSettings.PathRoot).Where(x => x.EndsWith(".sln")).FirstOrDefault();

        var solution = SolutionFile.Parse(path);

        if (File.Exists(path))
        {
            var lineSolutions = await _solutionService.GetLineSolutionAsync(path, service);

            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path))
            {
                foreach (var item in lineSolutions)
                {
                    sw.WriteLine(item.Content);
                }
            }
        }
    }
    private async Task GenCodeAsync(ModelTemplate child, string rootPath, string pathTemplate)
    {
        if (string.IsNullOrEmpty(child.Type))
        {
            Console.WriteLine("Type must be not null !");
        }

        switch (child.Type)
        {
            case TemplateContants.Folder:
                {
                    CreateFolder(child, rootPath);
                    break;
                }
            case TemplateContants.File:
                {
                    await CreateFileAsync(child, rootPath, pathTemplate);
                    break;
                }
            default:
                {
                    break;
                }
        };
    }
    private void CreateFolder(ModelTemplate child, string rootPath)
    {
        string rootPathLocal = rootPath + child.Path;

        _folderService.CreateFolder(rootPathLocal);
    }
    private async Task CreateFileAsync(ModelTemplate child, string rootPath, string pathTemplate)
    {
        string rootPathLocal = rootPath;

        await _fileService.CreateFileAsync(rootPathLocal, child, pathTemplate);
    }
    #endregion

    #region GenFolderAndFileAsync
    private async Task GenFolderAndFileAsync(string service, Config config, string pathService)
    {
        //Get entities
        List<Line> lines = new();
        if (pathService != config.ConfigPathSettings.PathStructure)
        {
            lines = await _fileService.ReadAsync(config.ConfigPathSettings.PathDBML);
            var dictionary = _dbmlService.GetEntities(lines);
        }    

        //Get Struture
        var jsonData = File.ReadAllText(pathService);

        var modelTemplate = JsonConvert.DeserializeObject<List<ModelTemplate>>(jsonData);
        modelTemplate.ForEach(x => x.Module = service);
        modelTemplate.ForEach(x => x.Name = x.Name.Replace("{{ModuleName}}", service));
        modelTemplate.ForEach(x => x.Path = x.Path.Replace("{{ModuleName}}", service));

        if (string.IsNullOrEmpty(service))
        {
            Console.WriteLine("Module must be not null !");
        }
        var rootPath = config.ConfigPathSettings.PathRoot + "src\\" + service;

        _folderService.CreateFolder(rootPath);

        if (!modelTemplate.Any())
        {
            Console.WriteLine($"There are not any item within {service}");
        }

        var folderChildren = modelTemplate.Where(x => x.Type == TemplateContants.Folder).ToList();

        foreach (var child in folderChildren)
        {
            await GenCodeAsync(child, rootPath, config.ConfigPathSettings.PathTemplate);
        }

        var fileChildren = modelTemplate.Where(x => x.Type == TemplateContants.File).ToList();

        foreach (var child in fileChildren)
        {
            await GenCodeAsync(child, rootPath, config.ConfigPathSettings.PathTemplate);
        }
    }
    #endregion

    public async Task GenerateApiAsync(string service, Config config)
    {
        await GenFolderAndFileAsync(service, config, config.ConfigPathSettings.PathApiStructure);
    }

    public async Task GenerateModelAsync(string service,  Config config)
    {
        await GenFolderAndFileAsync(service, config, config.ConfigPathSettings.PathModelStructure);
    }

    public async Task GenerateRepositoryAsync(string service, Config config)
    {
        await GenFolderAndFileAsync(service, config, config.ConfigPathSettings.PathRepositoryStructure);
    }

    public async Task GenerateServiceAsync(string service, Config config)
    {
        await GenFolderAndFileAsync(service, config, config.ConfigPathSettings.PathServiceStructure);
    }

}
