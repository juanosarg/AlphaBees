using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using HarmonyLib;
using Verse;
using RimWorld;

namespace AchievementsExpanded
{
    /*public class ItemTrackerMultiple:ItemTracker 
    {
       

        public ItemTrackerMultiple()
        {
        }

        public ItemTrackerMultiple(ItemTrackerMultiple reference) : base(reference)
        {
            thingList = reference.thingList;
           
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref thingList, "thingList", LookMode.Def, LookMode.Value);

        }

        public override bool Trigger()
        {
            bool playerHasIt = false;
            foreach (KeyValuePair<ThingDef, int> set in thingList)
            {
                playerHasIt = UtilityMethods.PlayerHas(set.Key, out int total, count);
                if (playerHasIt) { break; }
            }
            return playerHasIt;
        }

        Dictionary<ThingDef, int> thingList = new Dictionary<ThingDef, int>();
    }*/
}
