using System.Collections.Generic;
using Verse;

namespace AchievementsExpanded
{
    public class ItemTrackerMultiple : ItemTracker
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
            foreach (KeyValuePair<ThingDef, int> set in thingList)
            {
                if (UtilityMethods.PlayerHas(set.Key, out int total, count))
                {
                    return true;
                }
            }

            return false;
        }

        Dictionary<ThingDef, int> thingList = new Dictionary<ThingDef, int>();
    }
}
