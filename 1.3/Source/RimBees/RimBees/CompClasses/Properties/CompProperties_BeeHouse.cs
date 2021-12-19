using System.Collections.Generic;
using RimWorld;
using Verse;

namespace RimBees
{
    public class CompProperties_BeeHouse : CompProperties
    {
        public bool isBeehouse = true;
        public bool electricBeehouse = false;
        public bool climatizedBeehouse = false;
        public float beehouseRate = 1f;

        public CompProperties_BeeHouse()
        {
            this.compClass = typeof(CompBeeHouse);
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (var error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }

            if (this.electricBeehouse && parentDef.GetCompProperties<CompProperties_Power>() == null)
            {
                yield return parentDef.defName + " is an electric beehouse but does not have CompProperties_Power";
            }
        }
    }
}
