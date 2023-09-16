using Autofac;
using Autofac.Extensions.DependencyInjection;
using gencode.Options;
using gencode.service.Services.Interfaces;
using gencode.service.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.ComponentModel;
using System.Reflection;

namespace gencode;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("Welcome to the show !!!")
        {
             new Option<string?>("--myoption", () => null, "My option. Defaults to null")
        };

        var container = CompositionRoot();

        var configuration = GetConfiguration();

        var serviceProvider = new AutofacServiceProvider(container);

        //Get settings 
        Config settings = new Config();
        configuration.GetSection(Config.ConfigKey).Bind(settings);

        //Get Service
        var templateService = serviceProvider.GetRequiredService<ITemplateService>();
        var appRoot = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.LastIndexOf("src\\gencode\\bin"));
        Console.WriteLine(appRoot);

        settings.ConfigPathSettings.PathStructure = appRoot + "Solution Items\\Structures\\" + settings.ConfigPathSettings.PathStructure;
        settings.ConfigPathSettings.PathApiStructure = appRoot + "Solution Items\\Structures\\" + settings.ConfigPathSettings.PathApiStructure;
        settings.ConfigPathSettings.PathModelStructure = appRoot + "Solution Items\\Structures\\" + settings.ConfigPathSettings.PathModelStructure;
        settings.ConfigPathSettings.PathRepositoryStructure = appRoot + "Solution Items\\Structures\\" + settings.ConfigPathSettings.PathRepositoryStructure;
        settings.ConfigPathSettings.PathServiceStructure = appRoot + "Solution Items\\Structures\\" + settings.ConfigPathSettings.PathServiceStructure;
        settings.ConfigPathSettings.PathTemplate = appRoot + settings.ConfigPathSettings.PathTemplate;

        rootCommand.CreateOptions(templateService, settings);

        return await rootCommand.InvokeAsync(args);
    }


    private static Autofac.IContainer CompositionRoot()
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterAssemblyTypes(Assembly.Load("gencode.service"))
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        containerBuilder.RegisterAssemblyTypes(Assembly.Load("gencode.service"))
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        var container = containerBuilder.Build();

        return container;
    }

    private static IConfigurationRoot GetConfiguration()
    {
        var environmentName = Environment.GetEnvironmentVariable("ENVIRONMENT");
        var builder = new ConfigurationBuilder()
                                  .SetBasePath(AppContext.BaseDirectory)
                                  .AddJsonFile("appsettings.json", optional: false)
                                  .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                                  // Override config by env, using like Logging:Level or Logging__Level
                                  .AddEnvironmentVariables();
        var configurationRoot = builder.Build();
        return configurationRoot;
    }
}