using gencode.service.Services.Interfaces;

namespace gencode.service.Services;
public class FolderService : IFolderService
{
    public void CreateFolder(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
