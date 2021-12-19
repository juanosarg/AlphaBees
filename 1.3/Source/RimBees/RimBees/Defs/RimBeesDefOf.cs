using RimWorld;
using Verse;

namespace RimBees
{
    [DefOf]
    public static class RimBeesDefOf
    {
        public static ThingDef RB_BroodChamber;
        public static ThingDef RB_HybridizationChamber;

        public static ThingSetMakerDef RB_BeeResourcePod;

        public static JobDef RB_InsertingBees;
        public static JobDef RB_InsertingQueenBees;
        public static JobDef RB_TakeThingsOutOfBeehouseJob;
        public static JobDef RB_TakeThingsOutOfBroodChamberJob;
        public static JobDef RB_TakeThingsOutOfHybridizationChamberJob;

        public static BeeSpeciesDef Temperate;
        public static BeeSpeciesDef Mild;
        public static BeeSpeciesDef Hybrid;
        public static BeeSpeciesDef Amalgam;

        static RimBeesDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RimBeesDefOf));
        }
    }
}
