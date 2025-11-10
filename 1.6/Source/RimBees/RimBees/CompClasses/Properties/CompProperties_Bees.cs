using Verse;
using System.Collections.Generic;

namespace RimBees
{
    public class CompProperties_Bees : CompProperties
    {

        public string species = "temperate";
        public string comb = "RB_Temperate_Honeycomb";
        public float combtimedays = 1;
        public bool nocturnal = false;
        public bool pluviophile = false;
        public bool vacuumResistant = false;
        public string weirdplantneeded = "no";
        public int tempMin = 0;
        public int tempMax = 50;
        public List<AdditionalBeeEffects> additionalBeeEffects;

        public CompProperties_Bees()
        {
            this.compClass = typeof(CompBees);
        }
    }
}
