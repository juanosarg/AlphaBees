using HarmonyLib;
using RimWorld;
using Verse;

namespace RimBees
{
    // This Harmony Prefix allows us to regrow plants with a 25% percentage if an active beehouse is nearby
    [HarmonyPatch(typeof(Plant), nameof(Plant.PlantCollected))]
    static class Plant_PlantCollected_Patch
    {
        public static void Prefix(Plant __instance)
        {
            if (__instance.Blighted)
            {
                return;
            }

            if (!__instance.def.plant.HarvestDestroys || !__instance.def.plant.Sowable || __instance.def.plant.IsTree)
            {
                return;
            }

            foreach (var c in GenRadial.RadialCellsAround(__instance.Position, 6, false))
            {
                if (!c.InBounds(__instance.Map))
                {
                    continue;
                }

                var beehouse = c.GetEdifice(__instance.Map) as Building_Beehouse;
                if (beehouse?.BeehouseIsRunning == true && Rand.Chance(0.25f))
                {
                    var plant = (Plant)ThingMaker.MakeThing(__instance.def);
                    GenSpawn.Spawn(plant, __instance.Position, __instance.Map);
                    plant.Growth = 0.25f;
                    __instance.Map.mapDrawer.MapMeshDirty(__instance.Position, MapMeshFlag.Things);

                    return;
                }
            }
        }
    }
}
