using RimWorld;

namespace RimBees
{
    [DefOf]
    public static class BeeSpeciesDefOf
    {
        public static BeeSpeciesDef Temperate;
        public static BeeSpeciesDef Mild;
        public static BeeSpeciesDef Hybrid;
        public static BeeSpeciesDef Amalgam;

        static BeeSpeciesDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BeeSpeciesDefOf));
        }
    }
}
