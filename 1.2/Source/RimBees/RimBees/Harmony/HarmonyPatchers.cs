using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Collections.Generic;
using RimWorld.Planet;
using System.Linq;
using System;

// So, let's comment this code, since it uses Harmony and has moderate complexity

namespace RimBees
{



    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.rimbees");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }


    }


    /*This Harmony Postfix changes terrain calculation for floating creatures
   */
    [HarmonyPatch(typeof(Verse.AI.Pawn_PathFollower))]
    [HarmonyPatch("CostToMoveIntoCell")]
    [HarmonyPatch(new Type[] { typeof(Pawn), typeof(IntVec3) })]
    public static class Pawn_PathFollower_CostToMoveIntoCell_Patch
    {
        [HarmonyPostfix]
        public static void MakeFloatingCreaturesGreatAgain(Pawn pawn, IntVec3 c, ref int __result)

        {
            if ((pawn.Map != null) && (pawn.def.defName.Contains("GR_BeeHybrid_")))
            {
                
                    int num;
                    if (c.x == pawn.Position.x || c.z == pawn.Position.z)
                    {
                        num = pawn.TicksPerMoveCardinal;
                    }
                    else
                    {
                        num = pawn.TicksPerMoveDiagonal;
                    }
                    TerrainDef terrainDef = pawn.Map.terrainGrid.TerrainAt(c);
                    if (terrainDef == null)
                    {
                        num = 10000;
                    }
                    else if ((terrainDef.passability == Traversability.Impassable) && !terrainDef.IsWater)
                    {
                        num = 10000;
                    }
                    else if (terrainDef.IsWater)
                    {
                        num = 10000;
                    }
                    List<Thing> list = pawn.Map.thingGrid.ThingsListAt(c);
                    for (int i = 0; i < list.Count; i++)
                    {
                        Thing thing = list[i];
                        if (thing.def.passability == Traversability.Impassable)
                        {
                            num = 10000;
                        }

                        if (thing is Building_Door)
                        {
                            num += 45;
                        }
                    }

                    __result = num;

                

            }




        }
    }


    /*This Harmony Postfix allows us to regrow plants with a 25% percentage if an active beehouse is nearby
*/
    [HarmonyPatch(typeof(Plant))]
    [HarmonyPatch("PlantCollected")]
    public static class Plant_PlantCollected_Patch
    {
        [HarmonyPrefix]
        public static bool RegrowIfBeehouseNearby(ref Plant __instance) {

            if (__instance.Blighted)
                return true;

            if (__instance.def.plant.HarvestDestroys&& __instance.def.plant.Sowable && !__instance.def.plant.IsTree)

            {
                int num = GenRadial.NumCellsInRadius(6);
                for (int i = 0; i < num; i++)
                {
                    IntVec3 current = __instance.Position + GenRadial.RadialPattern[i];
                    if (current.InBounds(__instance.Map))
                    {
                        Building getbeehouse = current.GetEdifice(__instance.Map);
                        if ((getbeehouse != null)&&((getbeehouse.def.defName== "RB_Beehouse") ||(getbeehouse.def.defName == "RB_AdvancedClimatizedBeehouse") ||
                            (getbeehouse.def.defName == "RB_ClimatizedBeehouse") || (getbeehouse.def.defName == "RB_AdvancedBeehouse"))) {

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
                                    __instance.Map.mapDrawer.MapMeshDirty(__instance.Position, MapMeshFlag.Things);
                                    return true;
                                }
                                
                            }
                            

                        }
                        
                       
                    }
                     
                } return true;
                
            } else return true;





        }


    }
            

}

  








