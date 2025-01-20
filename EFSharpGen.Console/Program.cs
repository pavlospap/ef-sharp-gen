using System.Reflection;

using CommandLine;

using DustInTheWind.ConsoleTools.Controls.Spinners;
using DustInTheWind.ConsoleTools.Controls.Spinners.Templates;

using EFSharpGen;
using EFSharpGen.Console;

using Mapster;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

try
{
    var result = Parser.Default
        .ParseArguments<ConsoleOptions>(args)
        .WithParsed(Execute);

    if (result.Tag == ParserResultType.NotParsed)
    {
        Environment.Exit(1);
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;

    Console.WriteLine(ex.Message);
}
finally
{
    Console.ResetColor();
}

static void Execute(ConsoleOptions consoleOptions)
{
    Console.ForegroundColor = ConsoleColor.Green;

    using var spinner = CreateSpinner();

    spinner.Display();

    using var host = CreateHost(consoleOptions);

    host.Services.GetService<ICodeEngine>()!.GenerateCode();

    spinner.Close();

    Console.WriteLine("Files generated successfully!");
}

static Spinner CreateSpinner()
{
    var template = new StickSpinnerTemplate();

    return new(template)
    {
        FrameIntervalMilliseconds = 100,
        Label = "Generating files... "
    };
}

static IHost CreateHost(ConsoleOptions consoleOptions)
{
    return Host.CreateDefaultBuilder()
        .ConfigureServices(services =>
        {
            services.AddEFSharpGen(options =>
                consoleOptions.Adapt(options));

            RegisterCustomImplementations(
                services, consoleOptions.CustomAssemblyPath!);
        })
        .Build();
}

static void RegisterCustomImplementations(
    IServiceCollection services, string customAssemblyPath)
{
    if (string.IsNullOrWhiteSpace(customAssemblyPath))
    {
        return;
    }

    var customTypes = Assembly.LoadFrom(customAssemblyPath).GetTypes();

    var serviceTypes = typeof(ICodeEngine).Assembly.GetTypes()
        .Where(t => t.IsInterface)
        .Where(t => t.IsPublic);

    foreach (var serviceType in serviceTypes)
    {
        var implementationType = customTypes
            .FirstOrDefault(serviceType.IsAssignableFrom);

        if (implementationType != null)
        {
            var currentService = services
                .Single(s => s.ServiceType == serviceType);

            services.Replace(
                new(serviceType, implementationType, currentService.Lifetime));
        }
    }
}
