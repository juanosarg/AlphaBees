using Harmony;
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
            var harmony = HarmonyInstance.Create("com.rimbees");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
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

            if (__instance.def.plant.HarvestDestroys&& !__instance.def.plant.IsTree)

            {
                int num = GenRadial.NumCellsInRadius(6);
                for (int i = 0; i < num; i++)
                {
                    IntVec3 current = __instance.Position + GenRadial.RadialPattern[i];
                    if (current.InBounds(__instance.Map))
                    {
                        if((Building_Beehouse)current.GetEdifice(__instance.Map) != null) {

                            Building_Beehouse thebeehouse = (Building_Beehouse)current.GetEdifice(__instance.Map);

                            if (thebeehouse.BeehouseIsRunning)
                            {
                                Random random = new Random();
                                if (random.NextDouble() > 0.75)
                                {
                                    Thing thing = ThingMaker.MakeThing(ThingDef.Named(__instance.def.defName), null);
                                    GenSpawn.Spawn(thing, __instance.Position, __instance.Map);
                                    __instance.Map.mapDrawer.MapMeshDirty(__instance.Position, MapMeshFlag.Things);
                                    __instance.Destroy(DestroyMode.Vanish);
                                }
                                else __instance.Destroy(DestroyMode.Vanish);
                            }

                        }
                        
                       
                    }
                }
                return false;
            }
            else return true;





        }


    }
            

}

  








