using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers.Server;
using System.Reflection;

namespace dartwint.DartStash.Server;

/// <summary>
/// Чтение и загрузка общего конфига
/// </summary>
/// <param name="logger"></param>
/// <param name="modHelper"></param>
[Injectable(TypePriority = OnLoadOrder.Preload + 1)]
public class ReadJsonConfig(
        ISptLogger<ReadJsonConfig> logger,
        ModHelper modHelper) 
    : IOnLoad
{
    public const string ConfigFileName = "config.json";

    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        // This will get us the full path to the mod, e.g. C:\spt\user\mods\5ReadCustomJsonConfig-0.0.1
        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());

        // We give the path to the mod folder and the file we want to get, giving us the config, supply the files 'type' between the diamond brackets
        var config = modHelper.GetJsonDataFromFile<ModConfig>(pathToMod, ConfigFileName);

        //logger.Success($"Read property: 'ExampleProperty' from config with value: {config.ExampleProperty}");

        return Task.CompletedTask;
    }
}

// Represents the config structure
public record ModConfig
{
    public required string PlayerPMCLocation { get; set; }
    public required string PlayerPMCStashLocation { get; set; }
}
