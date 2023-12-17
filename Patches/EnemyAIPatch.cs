using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Net;
using GameNetcodeStuff;
using BepInEx.Logging;

namespace LethalCompanyFPS.Patches
{

    [HarmonyPatch(typeof(EnemyAI))]
    internal class EnemyAIPatch
    {

        private static ManualLogSource MyLogger = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.PLUGIN_NAME);
        [HarmonyPatch("Start")]
            [HarmonyPostfix]
            private static void Delete(EnemyAI __instance)
            {        
                if (!__instance.enemyType.canDie || __instance.enemyType.isOutsideEnemy)
                {
                    MyLogger.LogInfo($"enemy {__instance.name} not spawned");
                    Object.Destroy(__instance.gameObject);
                }
            }

            [HarmonyPatch("KillEnemy")]
            [HarmonyPostfix]
            private static void AddLootForKill(EnemyAI __instance)
            {

            MyLogger.LogInfo("starting add Loot for kill");
            Vector3[] array2 = new Vector3[2];
            array2[0] = RoundManager.Instance.playersManager.playerSpawnPositions[1].position;
            array2[0].x += UnityEngine.Random.Range(-0.7f, 0.7f);
            array2[0].z += UnityEngine.Random.Range(2f, 2f);
            array2[0].y += 0.5f;
            GrabbableObject obj = GetGrabbableObject("CookieMoldPan");
            GrabbableObject component = UnityEngine.Object.Instantiate(obj.itemProperties.spawnPrefab, array2[0], Quaternion.identity, RoundManager.Instance.playersManager.elevatorTransform).GetComponent<GrabbableObject>();
            component.fallTime = 1f;
            component.hasHitGround = true;
            component.itemProperties.isScrap = true;
            // component.scrapPersistedThroughRounds = true;
            component.isInElevator = true;
            component.isInShipRoom = true;
            component.SetScrapValue(50);
            component.NetworkObject.Spawn();
            RoundManager.Instance.CollectNewScrapForThisRound(component);
            }

            private static GrabbableObject GetGrabbableObject(string v)
            {
                foreach (GrabbableObject it in Resources.FindObjectsOfTypeAll<GrabbableObject>())
                {
                   // MyLogger.LogInfo($"{it.name}");
                    if (it.name == v) return it;
                }
                return null;
            }
    }

        [HarmonyPatch(typeof(Turret))]
        internal class TurretPatch
        {
            [HarmonyPatch("Start")]
            [HarmonyPostfix]
            private static void Delete(Turret __instance)
            {
                Object.Destroy(__instance.gameObject);
            }
        }

        [HarmonyPatch(typeof(Landmine))]
        internal class LandminePatch
        {
            [HarmonyPatch("Start")]
            [HarmonyPostfix]
            private static void Delete(Landmine __instance)
            {
                Object.Destroy(__instance.gameObject);
            }
        }
    }
