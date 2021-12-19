using RimWorld;
using UnityEngine;
using Verse;

namespace RimBees
{
    public static class BeeListSetupUtility
    {
        public static Command_SetBeeList SetBeeListCommand(Building_Beehouse beehouse)
        {
            return new Command_SetBeeList
            {
                defaultDesc = "RB_InsertBeesDesc".Translate(),
                defaultLabel = "RB_InsertBees".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/RB_AddDrones_ToBeehouse", true),
                hotKey = KeyBindingDefOf.Misc1,
                beehouse = beehouse
            };
        }

        public static Command_SetQueenList SetQueenListCommand(Building_Beehouse beehouse)
        {
            return new Command_SetQueenList
            {
                defaultDesc = "RB_InsertQueensDesc".Translate(),
                defaultLabel = "RB_InsertQueens".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/RB_AddQueens_ToBeehouse", true),
                hotKey = KeyBindingDefOf.Misc1,
                beehouse = beehouse
            };
        }
    }
}
