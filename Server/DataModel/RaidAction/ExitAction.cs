using dartwint.DartStash.Server.DataModel.LocationsData;

namespace dartwint.DartStash.Server.DataModel;

public class ExitAction : LocationLeaveAction
{
    public required ExitInfo ExitInfo { get; init; }
}

public class ExitAction<TResult> : LocationLeaveAction<TResult>
{
    public required ExitInfo ExitInfo { get; init; }
}