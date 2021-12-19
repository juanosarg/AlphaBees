using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace RimBees
{
    public class WorkGiver_TakeThingsOutOfBeehouse : WorkGiver_Scanner
    {
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.GetComponent<Beehouses_MapComponent>().beehouses_InMap;
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
            var beehouse = t as Building_Beehouse;
            if (beehouse?.BeehouseIsFull != true)
            {
                return false;
            }

            return !t.IsBurning() && !t.IsForbidden(pawn) && pawn.CanReserve(t, ignoreOtherReservations: forced);
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(RimBeesDefOf.RB_TakeThingsOutOfBeehouseJob, t);
        }
    }
}
