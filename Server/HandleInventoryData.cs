using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Servers;
using System;
using System.Collections.Generic;
using System.Text;

namespace dartwint.DartStash.Server;

[Injectable(TypePriority = OnLoadOrder.PostLoad + 1)]
public class HandleInventoryData(
    ISptLogger<HandleInventoryData> logger,
    SaveServer saveServer)
    : IOnLoad
{
    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        logger.Info("OnLoadAsync begin");

        

        return Task.CompletedTask;
    }
}
