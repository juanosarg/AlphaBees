using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace RimBees
{
    public class WorkGiver_InsertQueenInBeehouse : WorkGiver_Scanner
    {
        private static string NoQueensFound;

        public override ThingRequest PotentialWorkThingRequest
        {
            get
            {
                return ThingRequest.ForDef(ThingDef.Named("RB_Beehouse"));
            }
        }

        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }

        public static void ResetStaticData()
        {

            WorkGiver_InsertQueenInBeehouse.NoQueensFound = "RB_NoQueensFound".Translate();
        }


        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_Beehouse building_beehouse = t as Building_Beehouse;
            if (building_beehouse == null || !building_beehouse.BeehouseIsExpectingQueens)
            {
                return false;
            }
           
            if (!t.IsForbidden(pawn))
            {
                LocalTargetInfo target = t;
                if (pawn.CanReserve(target, 1, 1, null, forced))
                {
                    if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
                    {
                        return false;
                    }
                    if (this.FindQueen(pawn, building_beehouse.theQueenIAmGoingToInsert, building_beehouse) == null)
                    {
                        JobFailReason.Is(WorkGiver_InsertQueenInBeehouse.NoQueensFound, null);
                        return false;
                    }
                    return !t.IsBurning();
                }
            }
            return false;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_Beehouse building_beehouse = (Building_Beehouse)t;
            Thing t2 = this.FindQueen(pawn, building_beehouse.theQueenIAmGoingToInsert, building_beehouse);
            return new Job(DefDatabase<JobDef>.GetNamed("RB_InsertingQueenBees", true), t, t2);
        }

        private Thing FindQueen(Pawn pawn, string theQueenIAmGoingToInsert, Building_Beehouse building_beehouse)
        {
            Predicate<Thing> predicate = (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, 1, null, false);
            IntVec3 position = pawn.Position;
            Map map = pawn.Map;
            ThingRequest thingReq = ThingRequest.ForDef(ThingDef.Named(theQueenIAmGoingToInsert));
            PathEndMode peMode = PathEndMode.ClosestTouch;
            TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
            Predicate<Thing> validator = predicate;
            return GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
        }
    }
}

