using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Mesh;
using static UnityEngine.ParticleSystemJobs.NativeParticleData;

namespace LethalCompanyFPS.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch
    {

        private static ManualLogSource MyLogger = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.PLUGIN_NAME);

        private static bool completed = false;


        [HarmonyPatch("Start")]
        [HarmonyPostfix]

        private static void AddShotgunPostFix(StartOfRound __instance)
        {   

            foreach (GrabbableObject it in Resources.FindObjectsOfTypeAll<GrabbableObject>())
            {
                MyLogger.LogInfo($"{it.name}");
            }

        GameObject ship = GameObject.Find("/Environment/HangarShip");
            
            GrabbableObject[] shotgunsMade = ship.GetComponentsInChildren<GrabbableObject>().Where(x=> x.name == "ShotgunItem(Clone)" || x.name == "ShotgunItem").ToArray();
            MyLogger.LogInfo($"Shotguns already made {shotgunsMade.Length}");
            long numberOfPlayers = __instance.allPlayerObjects.LongLength;

            if (shotgunsMade.Length >= numberOfPlayers) return;

            long difference = (numberOfPlayers - shotgunsMade.Length);
            
            
            MyLogger.LogInfo($"Players on the map {numberOfPlayers}");
            Item shotgun = getItem("Shotgun");
           

            for (int i = 0; i < difference; i++)
            {
                CreateItemForShip(__instance, shotgun);
            }

        }

        private static void CreateItemForShip(StartOfRound __instance, Item item)
        {
            Vector3[] array2 = new Vector3[2];
            array2[0] = __instance.playerSpawnPositions[1].position;
            array2[0].x += UnityEngine.Random.Range(-0.7f, 0.7f);
            array2[0].z += UnityEngine.Random.Range(2f, 2f);
            array2[0].y += 0.5f;
            GrabbableObject component = UnityEngine.Object.Instantiate(item.spawnPrefab, array2[0], Quaternion.identity, __instance.elevatorTransform).GetComponent<GrabbableObject>();
            component.fallTime = 1f;
            component.hasHitGround = true;
            component.scrapPersistedThroughRounds = true;
            component.isInElevator = true;
            component.isInShipRoom = true;

            component.NetworkObject.Spawn();
            MyLogger.LogInfo("Loaded!");
        }
        private static Item getItem(string v)
        {
            foreach (Item it in Resources.FindObjectsOfTypeAll<Item>())
            {
                //MyLogger.LogInfo($"{it.name}");
                if (it.name == v) return it;
            }
            return null;
        }
    }
}
