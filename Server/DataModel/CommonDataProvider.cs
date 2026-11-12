namespace dartwint.DartStash.Server.DataModel;

public class CommonDataProvider
{
    public static Model CreateDataModel()
    {
        RaidTimeline raidTimeline = new RaidTimeline() { Entries = new HashSet<RaidTimelineEntry>() };
        raidTimeline.Entries.Add(new RaidTimelineEntry() { Key = "LoadingToRaid", Stamp = 100 });
        raidTimeline.Entries.Add(new RaidTimelineEntry() { Key = "RaidStarted", Stamp = 200 });
        raidTimeline.Entries.Add(new RaidTimelineEntry() { Key = "DuringRaid", Stamp = 300 });
        raidTimeline.Entries.Add(new RaidTimelineEntry() { Key = "OnLocationLeaveAction", Stamp = 400 });
        raidTimeline.Entries.Add(new RaidTimelineEntry() { Key = "LoadingAfterTransition", Stamp = 500 });
        raidTimeline.Entries.Add(new RaidTimelineEntry() { Key = "UnloadingFromRaid", Stamp = 600 });
        raidTimeline.Entries.Add(new RaidTimelineEntry() { Key = "OnCustomRaidAction", Stamp = 301 }); // ??

        return new Model() 
        {
            RaidTimeline = raidTimeline 
        };
    }
}