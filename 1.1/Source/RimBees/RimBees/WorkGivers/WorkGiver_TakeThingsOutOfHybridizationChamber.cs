using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace RimBees
{
    public class WorkGiver_TakeThingsOutOfHybridizationChamber : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest
        {
            get
            {
                return ThingRequest.ForDef(DefDatabase<ThingDef>.GetNamed("RB_HybridizationChamber", true));
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
            Building_HybridizationChamber building_hybridizationchamber = t as Building_HybridizationChamber;
            bool result;
            if (building_hybridizationchamber == null || !building_hybridizationchamber.hybridizationChamberFull)
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
            return new Job(DefDatabase<JobDef>.GetNamed("RB_TakeThingsOutOfHybridizationChamberJob", true), t);
        }
    }
}
