using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers.Profile;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services.Modding;
using SPTarkov.Server.Core.Utils;

namespace dartwint.DartStash.Server;

[Injectable]
public class InventoryStaticRouter(JsonUtil jsonUtil, InventoryStaticRouterCallback routerCallback)
    : StaticRouter(jsonUtil, [
            new RouteAction<AccessToStashRequestData>(
                DefaultsHelper.ValidateWithClientRoute,
                async(
                    url,
                    info,
                    sessionId,
                    output,
                    cancellationToken
                ) => await routerCallback.HandleValidationRoute(url, info, sessionId)
            ),

            new RouteAction<EmptyRequestData>(
                DefaultsHelper.CheckAccessToTheStashRoute,
                async(
                    url,
                    info,
                    sessionId,
                    output,
                    cancellationToken
                ) => await routerCallback.HandleCheckAccessToStashRoute(url, info, sessionId)
            )
        ])
{}

[Injectable]
public class InventoryStaticRouterCallback(
    ISptLogger<InventoryStaticRouterCallback> logger,
    HttpResponseUtil httpResponseUtil,
    ProfileHelper profileHelper,
    ProfileDataService profileDataService,
    ModProfileHelper modProfileHelper)
{

    // Returns empty response
    public async ValueTask<string> HandleCheckAccessToStashRoute(string url, EmptyRequestData info, MongoId sessionId)
    {
        logger.Info($"Callback on {url} route received! {nameof(HandleCheckAccessToStashRoute)} | SessionId:{sessionId}");

        var response = new AccessToStashRequestData() 
        {
            PlayerPMCStashAccessFlag = await modProfileHelper.PlayersPMCCheckAccessToStash(sessionId) 
        };

        return httpResponseUtil.NoBody(response);
    }

    public ValueTask<string> HandleValidationRoute(string url, AccessToStashRequestData info, MongoId sessionId)
    {
        logger.Info($"Callback on {url} route received! {nameof(HandleValidationRoute)} | SessionId:{sessionId}");

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

        var responseData = new AccessToStashRequestData()
        {
            PlayerPMCStashAccessFlag = false,
        };

        return new ValueTask<string>(httpResponseUtil.NoBody(responseData));
    }
}

/// <summary>
/// Represents the data sent by a client. That type must be the same on the client mod.
/// </summary>
public record AccessToStashRequestData : IRequestData
{
    public required bool PlayerPMCStashAccessFlag { get; set; }
}