using Verse;

namespace RimBees
{
    public class CompProperties_TempRuinableAndDestroy : CompProperties
    {
        public float minSafeTemperature = 0f;
        public float maxSafeTemperature = 100f;

        public float progressPerDegreePerTick = 1E-05f;

        public CompProperties_TempRuinableAndDestroy() : base(typeof(CompTempRuinableAndDestroy))
        {
        }
    }
}
