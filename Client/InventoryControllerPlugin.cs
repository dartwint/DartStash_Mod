using BepInEx;
using BepInEx.Logging;

namespace dartwint.DartStash.Client;

[BepInPlugin("dartwint.DartStash.client", "DartStash", "1.0.0")]
public class InventoryControllerPlugin : BaseUnityPlugin
{
    public static ManualLogSource LogSource;

    private void Awake()
    {
        LogSource = Logger;
        LogSource.LogInfo("plugin loaded!");

        new InventoryManagementPatch().Enable();
    }
}
