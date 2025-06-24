using System;
using Verse;
using Verse.AI;
using RimWorld;
using System.Collections.Generic;


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
            Building_Beehouse building_beehouse = t as Building_Beehouse;
            bool result;
            if (building_beehouse == null || !building_beehouse.BeehouseIsFull)
            {
                result = false;
            }
            else if (t.IsBurning())
            {
                result = false;
            }
            else
            {
                if (!t.IsForbidden(pawn))
                {
                    LocalTargetInfo target = t;
                    if (pawn.CanReserve(target, 1, -1, null, forced))
                    {
                        result = true;
                        return result;
                    }
                }
                result = false;
            }
            return result;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(InternalDefOf.RB_TakeThingsOutOfBeehouseJob, t);
        }
    }
}
