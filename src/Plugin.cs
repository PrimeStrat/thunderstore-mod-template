using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace MyMod;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log = null!;
    internal static ConfigEntry<bool> EnableMod = null!;
    private Harmony _harmony = null!;

    /// <summary>
    /// BepInEx entry point. Runs once when the plugin is loaded by the game.
    /// Initializes the static logger, binds configuration entries, and applies
    /// all Harmony patches discovered in this assembly. Keep this lightweight;
    /// defer heavy work to scene load or game-specific hooks.
    /// </summary>
    private void Awake()
    {
        Log = Logger;
        EnableMod = Config.Bind("General", "Enabled", true, "Master toggle for the mod.");

        if (!EnableMod.Value)
        {
            Log.LogInfo($"{PluginInfo.PLUGIN_NAME} disabled via config.");
            return;
        }

        _harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        _harmony.PatchAll();

        Log.LogInfo($"{PluginInfo.PLUGIN_NAME} v{PluginInfo.PLUGIN_VERSION} loaded.");
    }

    private void OnDestroy()
    {
        _harmony?.UnpatchSelf();
    }
}
