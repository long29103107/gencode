using gencode.service.Models;
using gencode.service.Models.Templates;
using gencode.service.Services.Interfaces;

namespace gencode.service.Services;
public class FileService : IFileService
{
    public async Task CreateFileAsync(string path, ModelTemplate chilren, string pathTemplate)
    {
        //Get Template
        var template = await GetTemplateAsync(pathTemplate, chilren);
        var stringTemplate = template.Select(x => x.Content).ToList();

        var pathGenerate = $"{path}{chilren.Path}{chilren.Name}";

        if (File.Exists(pathGenerate))
        {
            File.Delete(pathGenerate);
        }

        using (var writer =  File.CreateText(pathGenerate))
        {
            await writer.FlushAsync();
            foreach (var item in stringTemplate)
            {
                await writer.WriteLineAsync(item);
            }

            writer.Close();
        }
    }


    public async Task<List<Line>> ReadAsync(string path)
    {
        List<string> lines = (await File.ReadAllLinesAsync(path)).ToList();

        int index = 1;
        List<Line> lineWithIndexs = lines.Select(x => new Line { Content = x, Index = index++ }).ToList();

        lineWithIndexs = lineWithIndexs.Where(x => !string.IsNullOrEmpty(x.Content.Trim()) && !x.Content.Trim().StartsWith("//"))
                                    .ToList();

        return lineWithIndexs;
    }

    #region Get template

    private async Task<List<Line>> GetTemplateAsync(string pathTemplate, ModelTemplate chilren)
    {
        var tempPathTemplate = $"{pathTemplate}{chilren.Resource}\\{chilren.Template}";

        var file = await ReadAsync(tempPathTemplate);
        foreach (Line item in file)
        {
            item.Content = item.Content.Replace("{{ModuleName}}", chilren.Module);
            item.Content = item.Content.Replace("{{ModuleNameLowerCase}}", chilren.Module.ToLower());
        }

        return file;
    }
    #endregion
}
