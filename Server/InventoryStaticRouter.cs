using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers.Profile;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services.Modding;
using SPTarkov.Server.Core.Utils;
using System.Text.Json.Serialization;

namespace dartwint.DartStash.Server;

[Injectable]
public class InventoryStaticRouter(JsonUtil jsonUtil, InventoryStaticRouterCallback routerCallback)
    : StaticRouter(jsonUtil, [
            new RouteAction<InventoryStaticRequestData>(
                DefaultsHelper.TestRoute1,
                async(
                    url,
                    info,
                    sessionId,
                    output,
                    cancellationToken
                ) => await routerCallback.HandleExampleStaticRoute(url, info, sessionId)
            ),
            // There are cases where you dont want to send data to the server, in that case you can ignore ExampleStaticRequestData and use EmptyRequestData
            new RouteAction<EmptyRequestData>(
                DefaultsHelper.TestEmptyStaticRoute,
                async(
                    url,
                    info,
                    sessionId,
                    output,
                    cancellationToken
                ) => await routerCallback.HandleEmptyExampleStaticRoute(url, info, sessionId)
            )
        ])
{}

[Injectable]
public class InventoryStaticRouterCallback(
    ISptLogger<InventoryStaticRouterCallback> logger,
    HttpResponseUtil httpResponseUtil,
    ProfileHelper profileHelper,
    ProfileDataService profileDataService)
{

    // Returns empty response
    public ValueTask<string> HandleEmptyExampleStaticRoute(string url, EmptyRequestData info, MongoId sessionId)
    {
        logger.Info($"Callback on {url} route received! {nameof(HandleEmptyExampleStaticRoute)}|SessionId:{sessionId}");

        return new ValueTask<string>(httpResponseUtil.NullResponse());
    }

    public ValueTask<string> HandleExampleStaticRoute(string url, InventoryStaticRequestData info, MongoId sessionId)
    {
        logger.Info($"Callback on {url} route received! {nameof(HandleExampleStaticRoute)}|SessionId:{sessionId}");

        //var sptProfile = saveServer.GetProfile(sessionId);
        var sptProfile = profileHelper.GetFullProfile(sessionId); // same as previous operation

        var newTestData = new ProfileModData()
        {
            PlayerPMCLocation = "factory",
            PlayerPMCStashLocation = "factory"
        };

        if (sptProfile != null && sptProfile.ProfileInfo != null && sptProfile.ProfileInfo?.ProfileId != null)
        {
            logger.LogWithColor("Attemt to save profile data...", Spectre.Console.Color.Yellow);

            MongoId profileId = (MongoId)sptProfile.ProfileInfo.ProfileId;
            string modKey = DefaultsHelper.ModKey;

            try
            {
                logger.Info($"ProfileId:{profileId} | ModKey:{modKey}");

                _ = profileDataService.SaveProfileDataAsync(profileId, modKey, newTestData);
                logger.LogWithColor($"{nameof(profileDataService.SaveProfileDataAsync)}... ran successfully?", Spectre.Console.Color.Yellow);
            }
            catch (Exception ex)
            {
                logger.Error($"{ex.ToString()}");
            }
        }
        else
        {
            logger.Error("ProfileId or sptProfile.ProfileInfo or sptProfile is null!");
        }

        var responseData = new InventoryStaticRequestData()
        {
            PlayerPMCLocation = "factory",
            PlayerPMCStashLocation = "factory",
            PlayerPMCStashAccessFlag = true,
        };

        return new ValueTask<string>(httpResponseUtil.NoBody(responseData));
    }
}

public record ProfileModData
{
    [JsonPropertyName("PlayerPMCLocation")]
    public required string PlayerPMCLocation { get; set; }

    [JsonPropertyName("PlayerPMCStashLocation")]
    public required string PlayerPMCStashLocation { get; set; }
}

/// <summary>
/// Represents the data sent by a client. Tip: that type must be the same on the client mod.
/// </summary>
public record InventoryStaticRequestData : IRequestData
{
    public required string PlayerPMCLocation { get; set; }
    public required string PlayerPMCStashLocation { get; set; }
    public required bool PlayerPMCStashAccessFlag { get; set; }
}
