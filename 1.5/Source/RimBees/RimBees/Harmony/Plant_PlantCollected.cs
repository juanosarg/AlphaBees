using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RimBees
{



    /*This Harmony Postfix allows us to regrow plants with a 25% percentage if an active beehouse is nearby
*/
    [HarmonyPatch(typeof(Plant))]
    [HarmonyPatch("PlantCollected")]
    public static class Plant_PlantCollected_Patch
    {
        [HarmonyPrefix]
        public static bool RegrowIfBeehouseNearby(ref Plant __instance)
        {

            if (__instance.Blighted)
                return true;

            if (__instance.def.plant.HarvestDestroys && __instance.def.plant.Sowable && !__instance.def.plant.IsTree)

            {
                int num = GenRadial.NumCellsInRadius(RimBees_Settings.beeEffectRadius);
                for (int i = 0; i < num; i++)
                {
                    IntVec3 current = __instance.Position + GenRadial.RadialPattern[i];
                    if (current.InBounds(__instance.Map))
                    {
                        Building getbeehouse = current.GetEdifice(__instance.Map);
                        if ((getbeehouse != null) && (getbeehouse is Building_Beehouse))
                        {

                            Building_Beehouse thebeehouse = (Building_Beehouse)getbeehouse;

                            if (thebeehouse.BeehouseIsRunning)
                            {
                                Random random = new Random();
                                if (random.NextDouble() > 0.75)
                                {
                                    Thing thing = ThingMaker.MakeThing(ThingDef.Named(__instance.def.defName), null);
                                    Plant plant = (Plant)thing;
                                    GenSpawn.Spawn(plant, __instance.Position, __instance.Map);
                                    plant.Growth = 0.25f;
                                    __instance.Map.mapDrawer.MapMeshDirty(__instance.Position, MapMeshFlagDefOf.Things);
                                    return true;
                                }

                            }


                        }


                    }

                }
                return true;

            }
            else return true;





        }



    }
}



