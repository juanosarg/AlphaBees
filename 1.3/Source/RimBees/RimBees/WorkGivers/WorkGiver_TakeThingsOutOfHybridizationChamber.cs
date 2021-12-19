using RimWorld;
using Verse;
using Verse.AI;

namespace RimBees
{
    public class WorkGiver_TakeThingsOutOfHybridizationChamber : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest
        {
            get
            {
                return ThingRequest.ForDef(RimBeesDefOf.RB_HybridizationChamber);
            }
        }

        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            var chamber = t as Building_HybridizationChamber;
            if (chamber?.hybridizationChamberFull != true)
            {
                return false;
            }

            return !t.IsBurning() && !t.IsForbidden(pawn) && pawn.CanReserve(t, ignoreOtherReservations: forced);
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(RimBeesDefOf.RB_TakeThingsOutOfHybridizationChamberJob, t);
        }
    }
}
