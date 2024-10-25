using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SocialPlatforms;
using Verse;
using static UnityEngine.GraphicsBuffer;

namespace RimBees
{



    /*This Harmony Postfix allows us to regrow plants with a 25% percentage if an active beehouse is nearby
*/
    [HarmonyPatch(typeof(Plant))]
    [HarmonyPatch("PlantCollected")]
    public static class Plant_PlantCollected_Patch
    {
        [HarmonyPrefix]
        public static void RegrowIfBeehouseNearby(ref Plant __instance)
        {
            if (__instance.def.plant.HarvestDestroys && __instance.def.plant.Sowable && !__instance.def.plant.IsTree && !__instance.Blighted)
            {
                HashSet<Thing> beehouses = __instance.Map.GetComponent<Beehouses_MapComponent>().beehouses_InMap;
                bool doOnce = false;
                foreach (var beehouse in beehouses)
                {
                    if (beehouse.PositionHeld.DistanceTo(__instance.PositionHeld) <= RimBees_Settings.beeEffectRadius)
                    {
                        Building_Beehouse thebeehouse = (Building_Beehouse)beehouse;

                        if (thebeehouse.BeehouseIsRunning)
                        {
                            if (Rand.Value > 0.75)
                            {
                                Thing thing = ThingMaker.MakeThing(__instance.def, null);
                                Plant plant = (Plant)thing;
                                GenSpawn.Spawn(plant, __instance.Position, __instance.Map);
                                plant.Growth = 0.25f;
                                __instance.Map.mapDrawer.MapMeshDirty(__instance.Position, MapMeshFlagDefOf.Things);
                                doOnce = true;
                            }
                        }
                    }
                    if (doOnce)
                    {
                        break;
                    }
                }
            }
        }
    }
}



