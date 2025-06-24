using RimWorld;
using System.Collections.Generic;
using Verse;

namespace RimBees
{
    public class CompProperties_BeeResearchData : CompProperties
    {
        public ThingDef firstBee;
        public ThingDef secondBee;
        public List<ThingDef> resultingBees;

        public CompProperties_BeeResearchData()
        {
            this.compClass = typeof(CompBeeResearchData);
        }
    }
}