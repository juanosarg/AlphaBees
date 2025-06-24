using System.Collections.Generic;
using Verse;

namespace RimBees
{
    public class BeeDangerManager_MapComponent : MapComponent
    {
        public HashSet<Thing> bees = new HashSet<Thing>();

        public BeeDangerManager_MapComponent(Map map) : base(map)
        {
        }

        internal void Track(Thing bee)
        {
            bees.Add(bee);
        }

        internal void Untrack(Thing bee)
        {
            bees.Remove(bee);
        }
    }
}
