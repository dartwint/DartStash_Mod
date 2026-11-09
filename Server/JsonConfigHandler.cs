using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers.Server;
using SPTarkov.Server.Core.Utils;
using System.Reflection;
using System.Text.Json.Serialization;

namespace dartwint.DartStash.Server;

/// <summary>
/// Handler for global config
/// </summary>
/// <param name="logger"></param>
/// <param name="modHelper"></param>
[Injectable(TypePriority = OnLoadOrder.Preload + 1)]
public class JsonConfigHandler(
        ISptLogger<JsonConfigHandler> logger,
        ModHelper modHelper,
        FileUtil fileUtil,
        JsonUtil jsonUtil)
{
    public readonly Lazy<ModConfig> DefaultConfig = new Lazy<ModConfig>(() => new ModConfig()
    {
        NewPlayerPMCLocation = "factory", NewPlayerPMCStashLocation = "factory"
    });

    public async Task<ModConfig> GetOrCreateConfig(CancellationToken cancellationToken = default)
    {
        ModConfig config = DefaultConfig.Value;
        ModConfig? loadedConfig = await LoadConfig(cancellationToken);

        if (loadedConfig == null)
        {
            await WriteDefaultConfig(cancellationToken);
        }
        else
        {
            config = loadedConfig;
        }

        return config;
    }

    public async Task<bool> WriteDefaultConfig(CancellationToken cancellationToken = default)
    {
        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly()) + DefaultsHelper.ModConfigFileName;        

        string fileContent = jsonUtil.Serialize(DefaultConfig.Value, indented: true) ?? throw new Exception("The profile data when serialized resulted in a null value");
        await fileUtil.WriteFileAsync(pathToMod, fileContent, cancellationToken);

        return true;
    }

    public async Task<ModConfig?> LoadConfig(CancellationToken cancellationToken = default)
    {
        ModConfig? config = null;

        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        if (fileUtil.FileExists(pathToMod))
        {
            config = modHelper.GetJsonDataFromFile<ModConfig>(pathToMod, DefaultsHelper.ModConfigFileName);
        }

        return config;
    }
}

/// <summary>
/// Represents the config structure (e.g. global config)
/// </summary>
public record ModConfig
{
    [JsonPropertyName("NewPlayerPMCLocation")]
    public required string NewPlayerPMCLocation { get; set; }

    [JsonPropertyName("NewPlayerPMCStashLocation")]
    public required string NewPlayerPMCStashLocation { get; set; }
}