using BepInEx;
using HarmonyLib;
using LethalCompanyFPS.Patches;
using System.ComponentModel;

namespace LethalCompanyFPS
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

        private static Plugin Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            harmony.PatchAll(typeof(Plugin));
            harmony.PatchAll(typeof(StartOfRoundPatch));
            harmony.PatchAll(typeof(ShotgunItemPatch));
            harmony.PatchAll(typeof(EnemyAIPatch));
            harmony.PatchAll(typeof(TurretPatch));
            harmony.PatchAll(typeof(LandminePatch));


        }
    }
}