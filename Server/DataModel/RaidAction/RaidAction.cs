namespace dartwint.DartStash.Server.DataModel;

public abstract class RaidAction
{
    public required RaidTimeline Timeline { get; init; }
    public required RaidActionStatus ActionStatus { get; init; }
}

public abstract class RaidAction<TResult> : RaidAction
{
    public required TResult Result { get; set; } // OR init?
}

public enum LocationLeavingPlaceKind
{
    Exit,
    Transit,
    Custom
}

public abstract class RaidActionResult
{
    //public required LocationInfo LocationInfo { get; init; }
}

public class EndOfRaidResult : RaidActionResult
{
    public required RaidStatus RaidStatus { get; init; }
}