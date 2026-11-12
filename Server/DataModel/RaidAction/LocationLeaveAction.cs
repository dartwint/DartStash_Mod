namespace dartwint.DartStash.Server.DataModel;

public abstract class LocationLeaveAction : RaidAction
{
    public required DateTime TimeSpan { get; init; }
}

public abstract class LocationLeaveAction<TResult> : RaidAction<TResult>
{
    public required DateTime TimeSpan { get; init; }
}
