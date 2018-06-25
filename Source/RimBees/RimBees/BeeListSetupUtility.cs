using RimWorld;
using Verse;

using UnityEngine;


namespace RimBees
{
    public static class BeeListSetupUtility
    {
        public static Command_SetBeeList SetBeeListCommand(Building beehouse,Map map)
        {
            return new Command_SetBeeList()
            {
                defaultDesc = "RB_InsertBeesDesc".Translate(),
                defaultLabel = "RB_InsertBees".Translate(),
                icon = ContentFinder<Texture2D>.Get("Things/Item/Bees/RB_Bee_Temperate_Drone", true),
                hotKey = KeyBindingDefOf.Misc1,
                map = map,
                beehouse = beehouse
                //settable = settable
            };
        }
    }
}
