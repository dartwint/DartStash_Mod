using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Tables;

namespace dartwint.DartStash.Server;

[Injectable(TypePriority = OnLoadOrder.PostLoad + 1)]
public class UnlockAllExits(
    ISptLogger<UnlockAllExits> logger,
    LocationTable locationTable)
    : IOnLoad
{
    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        logger.Info("OnLoadAsync begin");

        SetAllLocationsExitsGuaranteed();
        logger.Info("SetAllLocationsExitsGuaranteed() performed");

        return Task.CompletedTask;
    }

    private void SetAllLocationsExitsGuaranteed()
    {
        foreach (var exit in locationTable.Factory4Day.Base.Exits)
        {
            exit.Chance = 100;
            exit.ChancePVE = 100;
        }

        foreach (var exit in locationTable.Factory4Night.Base.Exits)
        {
            exit.Chance = 100;
            exit.ChancePVE = 100;
        }

        foreach (var exit in locationTable.Bigmap.Base.Exits)
        {
            exit.Chance = 100;
            exit.ChancePVE = 100;
        }

        foreach (var exit in locationTable.Interchange.Base.Exits)
        {
            exit.Chance = 100;
            exit.ChancePVE = 100;
        }

        foreach (var exit in locationTable.RezervBase.Base.Exits)
        {
            exit.Chance = 100;
            exit.ChancePVE = 100;
        }

        foreach (var exit in locationTable.TarkovStreets.Base.Exits)
        {
            exit.Chance = 100;
            exit.ChancePVE = 100;
        }

        foreach (var exit in locationTable.Laboratory.Base.Exits)
        {
            exit.Chance = 100;
            exit.ChancePVE = 100;
        }

        foreach (var exit in locationTable.Woods.Base.Exits)
        {
            exit.Chance = 100;
            exit.ChancePVE = 100;
        }

        foreach (var exit in locationTable.Shoreline.Base.Exits)
        {
            exit.Chance = 100;
            exit.ChancePVE = 100;
        }

        foreach (var exit in locationTable.Lighthouse.Base.Exits)
        {
            exit.Chance = 100;
            exit.ChancePVE = 100;
        }
    }
}
