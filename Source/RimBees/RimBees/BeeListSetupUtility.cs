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
                icon = ContentFinder<Texture2D>.Get("UI/RB_AddDrones_ToBeehouse", true),
                hotKey = KeyBindingDefOf.Misc1,
                map = map,
                beehouse = beehouse
                //settable = settable
            };
        }

        public static Command_SetQueenList SetQueenListCommand(Building beehouse, Map map)
        {
            return new Command_SetQueenList()
            {
                defaultDesc = "RB_InsertQueensDesc".Translate(),
                defaultLabel = "RB_InsertQueens".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/RB_AddQueens_ToBeehouse", true),
                hotKey = KeyBindingDefOf.Misc1,
                map = map,
                beehouse = beehouse
                //settable = settable
            };
        }
    }
}
