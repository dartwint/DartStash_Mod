namespace dartwint.DartStash.Server.DataModel.LocationsData;

public class LocationInfo
{
    public required string Key { get; init; }

    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }
}

public abstract class LocationDepartureInfo
{
    public required string Key { get; init; }
    public required LocationInfo LocationInfo { get; init; }
    public LocationLeavingPlaceKind PlaceKind { get; protected init; }

    public override int GetHashCode()
    {
        return HashCode.Combine(Key, LocationInfo);
    }
}

public class ExitInfo : LocationDepartureInfo
{
    public ExitInfo()
    {
        PlaceKind = LocationLeavingPlaceKind.Exit;
    }
}

public class TransitInfo : LocationDepartureInfo
{
    public required LocationInfo DestinationInfo { get; init; }

    public TransitInfo()
    {
        PlaceKind = LocationLeavingPlaceKind.Transit;
    }
}

public class LocationLeavingPlaceCustomKind
{
    public required string Key { get; init; }

    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }
}

public class CustomLocationDepartureInfo : LocationDepartureInfo
{
    public required LocationLeavingPlaceCustomKind CustomKind { get; init; }

    public CustomLocationDepartureInfo()
    {
        PlaceKind = LocationLeavingPlaceKind.Custom;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), CustomKind);
    }
}

public class GameMapsGraph
{
    /// <summary>
    /// Ключ - локация, с которой можно будет начать новый рейд. 
    /// Значение - список выходов, при использовании которых будет доступна локация из ключа.
    /// </summary>
    private Dictionary<LocationInfo, HashSet<ExitInfo>> _locationExitsBinds = new();

    /// <summary>
    /// Ключ - локация, с которой можно будет начать новый рейд.
    /// Значение - список переходов, после использования которых будет доступна локация из ключа.
    /// Подразумевается, что игрок не завершил успешно рейд.
    /// </summary>
    private Dictionary<LocationInfo, HashSet<TransitInfo>> _locationTransitsBinds = new();
}