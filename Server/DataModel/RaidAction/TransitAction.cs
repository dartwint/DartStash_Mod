using dartwint.DartStash.Server.DataModel.LocationsData;

namespace dartwint.DartStash.Server.DataModel;

public class TransitAction : LocationLeaveAction
{
    public required TransitInfo TransitInfo { get; init; }
}

public class TransitAction<TResult> : LocationLeaveAction<TResult>
{
    public required TransitInfo TransitInfo { get; init; }
}