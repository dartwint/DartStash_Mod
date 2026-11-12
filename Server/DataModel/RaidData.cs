using dartwint.DartStash.Server.DataModel.LocationsData;

namespace dartwint.DartStash.Server.DataModel;

public class RaidData
{
    public List<TransitInfo> PerformedTransits { get; } = new();
}