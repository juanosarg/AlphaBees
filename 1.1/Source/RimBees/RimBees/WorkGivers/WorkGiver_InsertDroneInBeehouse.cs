using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace RimBees
{
    public class WorkGiver_InsertDroneInBeehouse : WorkGiver_Scanner
    {
        private static string NoDronesFound;

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

            WorkGiver_InsertDroneInBeehouse.NoDronesFound = "RB_NoDronesFound".Translate();
        }


        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_Beehouse building_beehouse = t as Building_Beehouse;
            if (building_beehouse == null || !building_beehouse.BeehouseIsExpectingBees)
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
                    if (this.FindDrone(pawn, building_beehouse.theDroneIAmGoingToInsert, building_beehouse) == null)
                    {
                        JobFailReason.Is(WorkGiver_InsertDroneInBeehouse.NoDronesFound, null);
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
            Thing t2 = this.FindDrone(pawn, building_beehouse.theDroneIAmGoingToInsert, building_beehouse);
            return new Job(DefDatabase<JobDef>.GetNamed("RB_InsertingBees", true), t, t2);
        }

        private Thing FindDrone(Pawn pawn, string theDroneIAmGoingToInsert, Building_Beehouse building_beehouse)
        {
            Predicate<Thing> predicate = (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, 1, null, false);
            IntVec3 position = pawn.Position;
            Map map = pawn.Map;
            ThingRequest thingReq = ThingRequest.ForDef(ThingDef.Named(theDroneIAmGoingToInsert));
            PathEndMode peMode = PathEndMode.ClosestTouch;
            TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
            Predicate<Thing> validator = predicate;
            return GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
        }
    }
}

