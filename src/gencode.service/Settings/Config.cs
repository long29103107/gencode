using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace gencode.service.Settings;
public class Config
{  
    public const string ConfigKey = "Config";
    public ConfigPathSettings ConfigPathSettings { get; set; }
    public SolutionSettings SolutionSettings { get; set; }
    public ProjectTypeIdSettings ProjectTypeIdSettings { get; set; }
}
