
using gencode.service.Models;
using System.Reflection;

namespace ReadFile.Extensions;
public static class GenCodeExtentions
{
    public static void GenCode(this List<Line> lines, string path, string folderName, string fileName)
    {
        var folder = $"{path}{folderName}";
        //string templateContent = File.ReadAllText(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Resources/HtmlTemplates/UserList.html");
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        using (StreamWriter writer = System.IO.File.AppendText($"{path}{folderName}\\{fileName}"))
        {
            var pathTemplate = $"D:\\3. DotNet\\ReadFile\\Solution Items\\Templates\\Repository\\GenericRepository.txt";

            List<string> tempalte = File.ReadAllLines(pathTemplate).ToList();
            var index = 1;

            List<Line> genericLines = tempalte.Select(x => new Line { Content = x, Index = index++ }).ToList();

            genericLines = genericLines.Where(x => !string.IsNullOrEmpty(x.Content.Trim()) && !x.Content.Trim().StartsWith("//"))
                                        .ToList();

            foreach (var item in genericLines) 
            { 
                writer.WriteLine(item.Content);
            }
        }

        Console.WriteLine(folder);
    }
}
