using gencode.service.Constants;
using gencode.service.Models;
using gencode.service.Models.Solution;
using gencode.service.Services.Interfaces;
using System.Linq;

namespace gencode.service.Services;
public class SolutionService : ISolutionService
{
    private readonly IFileService _fileService;
    public SolutionService(IFileService fileService)
    {
        _fileService = fileService;
    }
    public async Task<List<Line>> GetLineSolutionAsync(string path, string service)
    {
        var readFile = await _fileService.ReadAsync(path);

        var global = readFile.Where(x => x.Content.Trim() == "Global").FirstOrDefault();

        var index = readFile.IndexOf(global) + 1;
        var preIndex = index - 1;

        var solutionItems = new List<SolutionModel>()
        {
            new SolutionModel()
            {
                ProjectTypeId = ProjectTypeIdContants.Folder,
                ProjectName = "{{ModuleName}}",
                ProjectLocation = "{{ModuleName}}",
                UniqueProjectId = "{" + Guid.NewGuid().ToString().ToUpper() + "}",
            },
            new SolutionModel()
            {
                ProjectTypeId = ProjectTypeIdContants.Project,
                ProjectName = "{{ModuleName}}.Api",
                ProjectLocation = "src\\{{ModuleName}}\\{{ModuleName}}.API\\{{ModuleName}}.Api.csproj",
                UniqueProjectId ="{" + Guid.NewGuid().ToString().ToUpper() + "}",
            },
            new SolutionModel()
            {
                ProjectTypeId = ProjectTypeIdContants.Project,
                ProjectName = "{{ModuleName}}.Model",
                ProjectLocation = "src\\{{ModuleName}}\\{{ModuleName}}.Model\\{{ModuleName}}.Model.csproj",
                UniqueProjectId = "{" + Guid.NewGuid().ToString().ToUpper() + "}",
            },
            new SolutionModel()
            {
                ProjectTypeId = ProjectTypeIdContants.Project,
                ProjectName = "{{ModuleName}}.Repository",
                ProjectLocation = "src\\{{ModuleName}}\\{{ModuleName}}.Repository\\{{ModuleName}}.Repository.csproj",
                UniqueProjectId = "{" + Guid.NewGuid().ToString().ToUpper() + "}",
            },
            new SolutionModel()
            {
                ProjectTypeId = ProjectTypeIdContants.Project,
                ProjectName = "{{ModuleName}}.Service",
                ProjectLocation = "src\\{{ModuleName}}\\{{ModuleName}}.Service\\{{ModuleName}}.Service.csproj",
                UniqueProjectId = "{" + Guid.NewGuid().ToString().ToUpper() + "}",
            },
        };
        
        readFile.InsertRange(preIndex, await GetProjectLocationAsync(solutionItems, service, readFile));
        index = readFile.IndexOf(global) + 1;

        var listItem = new List<Line>();
        if (listItem.Any(x => x.Content.Contains("GlobalSection(SolutionConfigurationPlatforms) = preSolution")))
        {
            listItem.AddRange(new List<Line>()
            {
                new Line() { Content = "\tGlobalSection(SolutionConfigurationPlatforms) = preSolution" },
                new Line() { Content = "\t\tDebug|Any CPU = Debug|Any CPU" },
                new Line() { Content = "\t\tRelease|Any CPU = Release|Any CPU" },
                new Line() { Content = "\tEndGlobalSection" },
            });
        }

        listItem.AddRange(await GetConfigLocationAsync(solutionItems, readFile));
        readFile.InsertRange(index, listItem);
                              
        return readFile;
    }

    private async Task<List<Line>> GetProjectLocationAsync(List<SolutionModel> model, string service, List<Line> readFile)
    {
        var result = new List<Line>();

        //Case add folder;
        var modelFolder = model.Where(x => x.ProjectTypeId == ProjectTypeIdContants.Folder).FirstOrDefault();

        bool existProjectFolder = readFile.Where(x => x.Content.Contains(service)).Any();
        var existProjectFolder1 = readFile.Where(x => x.Content.Contains(service)).ToList();

        if (!existProjectFolder)
        {
            result.Add(new Line() { Content = $"Project(\"{modelFolder.ProjectTypeId}\") = \"{modelFolder.ProjectName}\", \"{modelFolder.ProjectLocation}\", \"" + modelFolder.UniqueProjectId + "\"" });
            result.Add(new Line() { Content = "EndProject" });
        }

        //Case add project;
        var modelProject = model.Where(x => x.ProjectTypeId == ProjectTypeIdContants.Project).ToList();
        bool existProjectProject = readFile.Where(x => modelProject.Any(y => x.Content.Contains(y.ProjectName.Replace("{{ModuleName}}", service)))).Any();

        if(!existProjectProject)
        {
            foreach (var item in modelProject)
            {
                result.Add(new Line() { Content = $"Project(\"{item.ProjectTypeId}\") = \"{item.ProjectName}\", \"{item.ProjectLocation}\", \"{item.UniqueProjectId}\"" });
            }

            result.Add(new Line() { Content = "EndProject" });
        }
        
        result.ForEach(x => x.Content = x.Content.Replace("{{ModuleName}}", service));
        return result;
    }


    private async Task<List<Line>> GetConfigLocationAsync(List<SolutionModel> model, List<Line> readFile)
    {
        var result = new List<Line>();

        var existInSolution = readFile.Where(x => model.Any(y => x.Content.Contains(y.UniqueProjectId))).Any();

        if (existInSolution)
        {
            result.Add(new Line() { Content = "\tGlobalSection(ProjectConfigurationPlatforms) = postSolution" });

            var modelProject = model.Where(x => x.ProjectTypeId == ProjectTypeIdContants.Project).ToList();
            var modelFolder = model.Where(x => x.ProjectTypeId == ProjectTypeIdContants.Folder).FirstOrDefault();

            foreach (var item in modelProject)
            {
                result.Add(new Line() { Content = "\t\t" + item.UniqueProjectId + ".Debug|Any CPU.ActiveCfg = Debug|Any CPU" });
                result.Add(new Line() { Content = "\t\t" + item.UniqueProjectId + ".Debug|Any CPU.Build.0 = Debug|Any CPU" });
                result.Add(new Line() { Content = "\t\t" + item.UniqueProjectId + ".Release|Any CPU.ActiveCfg = Release|Any CPU" });
                result.Add(new Line() { Content = "\t\t" + item.UniqueProjectId + ".Release|Any CPU.Build.0 = Release|Any CPU" });
            }
            result.Add(new Line() { Content = "\tEndGlobalSection" });
            result.Add(new Line() { Content = "\tGlobalSection(NestedProjects) = preSolution" });

            foreach (var item in modelProject)
            {
                result.Add(new Line() { Content = "\t\t" + item.UniqueProjectId + " = " + modelFolder.UniqueProjectId });
            }
            result.Add(new Line() { Content = "\tEndGlobalSection" });
        }        

        return result;
    }
}
