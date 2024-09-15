
using RimWorld;
using Verse;


namespace RimBees
{
    [DefOf]
    public static class InternalDefOf
    {

        public static HediffDef RB_RecentlyResearched;
        public static ThingSetMakerDef RB_BeeResourcePod;
        public static JobDef RB_InsertingBees;
        public static JobDef RB_InsertingQueenBees;
        public static JobDef RB_TakeThingsOutOfBeehouseJob;
        public static JobDef RB_TakeThingsOutOfBroodChamberJob;
        public static JobDef RB_TakeThingsOutOfHybridizationChamberJob;

        public static ThingDef RB_BroodChamber;
        public static ThingDef RB_HybridizationChamber;

        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }
    }
}
