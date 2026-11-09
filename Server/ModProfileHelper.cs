namespace dartwint.DartStash.Server;

using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Services.Modding;
using System.Text.Json.Serialization;

[Injectable(TypePriority = OnLoadOrder.Preload + 1)]
public class ModProfileHelper(
    ISptLogger<ModProfileHelper> logger,
    ProfileDataService profileDataService,
    JsonConfigHandler jsonConfigHandler)
{
    public async Task<bool> PlayersPMCCheckAccessToStash(MongoId profileId)
    {
        var data = await profileDataService.GetProfileDataAsync<ProfileModData>(profileId, DefaultsHelper.ModKey);

        if (data == null)
        {
            ////
        }

        return false;
    }

    public async Task<ProfileModData> GetOrCreateProfileData(MongoId profileId, CancellationToken cancellationToken = default)
    {
        ProfileModData? data = await profileDataService.GetProfileDataAsync<ProfileModData>(profileId, DefaultsHelper.ModKey);
        if (data == null)
        {
            data = await WriteProfileDataAsync(profileId, cancellationToken);
        }

        return data;
    }

    public async Task<ProfileModData> WriteProfileDataAsync(MongoId profileId, CancellationToken cancellationToken = default)
    {
        logger.Info($"WritingProfileData...");

        var profileModData = ProfileModData.FromConfig(await jsonConfigHandler.GetOrCreateConfig(cancellationToken));

        try
        {
            logger.Info($"Saving ProfileId:{profileId}");

            await profileDataService.SaveProfileDataAsync(profileId, DefaultsHelper.ModKey, profileModData, cancellationToken);

            logger.LogWithColor($"{nameof(profileDataService.SaveProfileDataAsync)}... data was written", Spectre.Console.Color.Green);
        }
        catch (Exception ex)
        {
            logger.Error($"{ex.ToString()}");
        }

        return profileModData;
    }
}

/// <summary>
/// Represents per-user (client) data.
/// </summary>
public record ProfileModData
{
    [JsonPropertyName("PlayerPMCLocation")]
    public required string PlayerPMCLocation { get; set; }

    [JsonPropertyName("PlayerPMCStashLocation")]
    public required string PlayerPMCStashLocation { get; set; }

    /// <summary>
    /// Creates new mod-related profile data via global config
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static ProfileModData FromConfig(ModConfig config)
    {
        return new ProfileModData()
        {
            PlayerPMCLocation = config.NewPlayerPMCLocation,
            PlayerPMCStashLocation = config.NewPlayerPMCStashLocation
        };
    }
}
