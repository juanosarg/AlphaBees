using Verse;

namespace RimBees
{
    class CompProperties_Bees : CompProperties
    {

        public string species = "temperate";
        public string comb = "RB_Temperate_Honeycomb";
        public float combtimedays = 1;




        public CompProperties_Bees()
        {
            this.compClass = typeof(CompBees);
        }
    }
}
