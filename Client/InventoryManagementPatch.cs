using EFT.UI;
using HarmonyLib;
using Newtonsoft.Json;
using SPT.Common.Http;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace dartwint.DartStash.Client;

internal class InventoryManagementPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        //return AccessTools.Method(typeof(MenuScreen), nameof(MenuScreen.Click));

        // Scene: DontDestroyOnLoad
        // Common UI/Common UI/InventoryScreen/Items Panel/Stash Panel/ - Панель со схроном, можно ее отключить если игрок находится не в локации со схроном

        return AccessTools.Method(typeof(ItemsPanel), nameof(ItemsPanel.Show));
    }

    private static async Task SendTestPostRequest()
    {
        // Структура должна соответствовать серверному типу
        var requestData = new ProfileModData();

        string jsonBody = JsonConvert.SerializeObject(requestData);
        string response = RequestHandler.PostJson(DefaultsHelper.CheckAccessToTheStashRoute, jsonBody);

        if (string.IsNullOrWhiteSpace(response))
        {
            InventoryControllerPlugin.LogSource.LogError("Ответ сервера пуст или не удалось разобрать.");
        }
        else
        {
            try
            {
                var responseData = JsonConvert.DeserializeObject<ProfileModData>(response);

                if (responseData != null)
                {
                    InventoryControllerPlugin.LogSource.LogInfo($"Получены данные: Location={responseData.PlayerPMCLocation}, StashLocation={responseData.PlayerPMCStashLocation}");

                    bool playerHasAccessToStash = true;

                    if (!playerHasAccessToStash)
                    {
                        // TO DO: REPLACE THAT SH*T
                        GameObject stashPanel = GameObject.Find("Common UI/Common UI/InventoryScreen/Items Panel/Stash Panel");
                        if (stashPanel != null)
                        {
                            stashPanel.SetActive(false);
                            InventoryControllerPlugin.LogSource.LogMessage($"Stash panel was disabled.");
                        }
                        else
                        {
                            InventoryControllerPlugin.LogSource.LogError($"GameObject not found: 'Common UI / Common UI / InventoryScreen / Items Panel / Stash Panel'!");
                        }
                    }
                }
                else
                {
                    InventoryControllerPlugin.LogSource.LogError("Ответ сервера пуст или не удалось разобрать.");
                }
            }
            catch (Exception ex)
            {
                InventoryControllerPlugin.LogSource.LogError($"{ex.ToString()}");
            }
        }
    }

    private static async Task SendTestGetRequest()
    {
        var responseData = JsonConvert.DeserializeObject<AccessToStashRequestData>(RequestHandler.GetJson(DefaultsHelper.CheckAccessToTheStashRoute));
        if (responseData != null)
        {
            InventoryControllerPlugin.LogSource.LogInfo($"Получены данные: HasAccessToStash={responseData.PlayerPMCStashAccessFlag}");

            bool blockStash = !responseData.PlayerPMCStashAccessFlag;
            if (blockStash)
            {
                // TO DO: REPLACE THAT SH*T
                GameObject stashPanel = GameObject.Find("Common UI/Common UI/InventoryScreen/Items Panel/Stash Panel");
                if (stashPanel != null)
                {
                    stashPanel.SetActive(false);
                    InventoryControllerPlugin.LogSource.LogMessage($"Stash panel was disabled.");
                }
                else
                {
                    InventoryControllerPlugin.LogSource.LogError($"GameObject not found: 'Common UI / Common UI / InventoryScreen / Items Panel / Stash Panel'!");
                }
            }
        }
        else
        {
            InventoryControllerPlugin.LogSource.LogError("Ответ сервера пуст или не удалось разобрать.");
        }
    }

    /// <summary>
    /// Runs BEFORE original code is executed.
    /// </summary>
    /// <returns> true - the original code will still run; false - the original code will be skipped.
    /// </returns>
    [PatchPrefix]
    static bool Prefix()
    {
        return true;
    }

    // Runs AFTER the original code is executed
    [PatchPostfix]
    static void Postfix()
    {
        //InventoryControllerPlugin.LogSource.LogMessage("Clicked on MenuScreen!");
        InventoryControllerPlugin.LogSource.LogMessage($"Postfix {typeof(InventoryManagementPatch)}. Attempt to disable GameObject: Common UI/Common UI/InventoryScreen/Items Panel/Stash Panel");

        //CommonUI.Instance.InventoryScreen.

        // "/dartwint/DartStash/static"
        // https://127.0.0.1:6969

        //_ = SendTestRequest();
        _ = SendTestGetRequest();
    }
}

/// <summary>
/// Represents the data sent by a client. Tip: that type must be the same on the client mod.
/// </summary>
public class AccessToStashRequestData
{
    public bool PlayerPMCStashAccessFlag { get; set; }
}

public class ProfileModData
{
    public string PlayerPMCLocation { get; set; } = "unknown";

    public string PlayerPMCStashLocation { get; set; } = "unknown";
}