using System.Collections.Generic;
using Verse;

namespace RimBees
{
    public class Beehouses_MapComponent : MapComponent
    {
        // This class receives calls when a new Beehouse is built or destroyed, storing or deleting it from a List
        // This List is used on WorkGivers. They'll only look for things on the List, causing less lag
        public HashSet<Thing> beehouses_InMap = new HashSet<Thing>();

        public Beehouses_MapComponent(Map map) : base(map)
        {
        }

        public void AddBeehouseToMap(Thing thing)
        {
            beehouses_InMap.Add(thing);
        }

        public void RemoveBeehouseFromMap(Thing thing)
        {
            beehouses_InMap.Remove(thing);
        }
    }
}
