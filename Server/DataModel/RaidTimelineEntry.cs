namespace dartwint.DartStash.Server.DataModel;

public class RaidTimeline
{
    public required HashSet<RaidTimelineEntry> Entries { get; set; }
}

public class RaidTimelineEntry()
{
    public required string Key { get; init; }
    public int Stamp { get; init; }

    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }
}