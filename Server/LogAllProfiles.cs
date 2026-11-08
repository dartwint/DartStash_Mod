using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Servers;

namespace dartwint.DartStash.Server;

[Injectable(TypePriority = OnLoadOrder.PostLoad + 777)]
public class LogAllProfiles(
    ISptLogger<LogAllProfiles> logger,
    SaveServer saveServer)
    : IOnLoad
{
    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        logger.Info("OnLoadAsync begin");

        var profiles = saveServer.GetProfiles();
        logger.Info($"Enumerating all {nameof(SaveServer)} profiles");
        foreach (var profile in profiles)
        {
            var val = profile.Value;
            if (val.ProfileInfo != null && val.ProfileInfo.Username != null)
            {
                logger.Info($"Key:{profile.Key} | name:{val.ProfileInfo.Username}");
            }
            else
            {
                logger.Info($"Key:{profile.Key} | name is NULL");
            }
        }

        return Task.CompletedTask;
    }
}
