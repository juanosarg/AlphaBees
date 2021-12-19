using System.Collections.Generic;
using Verse;

namespace RimBees
{
    public class CompProperties_Bees : CompProperties
    {
        public BeeSpeciesDef species;
        public string comb = "RB_Temperate_Honeycomb";
        public float combtimedays = 1;
        public bool nocturnal = false;
        public bool pluviophile = false;
        public string weirdplantneeded = "no";
        public int tempMin = 0;
        public int tempMax = 50;

        public CompProperties_Bees() : base(typeof(CompBees))
        {
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (var error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }

            if (species == null)
            {
                yield return parentDef.defName + " missing species in CompProperties_Bees";
            }
        }
    }
}
