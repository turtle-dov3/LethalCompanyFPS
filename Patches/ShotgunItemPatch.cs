using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace LethalCompanyFPS.Patches
{
    [HarmonyPatch(typeof(ShotgunItem))]


    internal class ShotgunItemPatch
    {
        private static ManualLogSource MyLogger = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.PLUGIN_NAME);

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static void UpdatePreFix(ShotgunItem __instance)
        {
            __instance.shellsLoaded = 2;
            __instance.isReloading = false;

        }
        [HarmonyPatch("delayedEarsRinging")]
        [HarmonyPrefix]
        private static void EarRingingPreFix(ref float effectSeverity)
        {
            effectSeverity = 0.0f;
        }
    }




}
