using System;
using Verse;
using RimWorld;

namespace RimBees
{
    public class CompProperties_TempRuinableAndDestroy : CompProperties
    {
        public float minSafeTemperature;

        public float maxSafeTemperature = 100f;

        public float progressPerDegreePerTick = 1E-05f;

        public CompProperties_TempRuinableAndDestroy()
        {
            this.compClass = typeof(CompTempRuinableAndDestroy);
        }
    }
}
