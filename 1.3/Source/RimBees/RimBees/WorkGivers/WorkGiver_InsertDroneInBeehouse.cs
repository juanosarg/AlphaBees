using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace RimBees
{
    public class WorkGiver_InsertDroneInBeehouse : WorkGiver_Scanner
    {
        private static string NoDronesFound;

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

        public static void ResetStaticData()
        {
            NoDronesFound = "RB_NoDronesFound".Translate();
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            var beehouse = t as Building_Beehouse;
            if (beehouse?.BeehouseIsExpectingBees != true)
            {
                return false;
            }

            if (!t.IsForbidden(pawn))
            {
                if (pawn.CanReserve(t, 1, 1, null, forced))
                {
                    if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
                    {
                        return false;
                    }

                    if (this.FindDrone(pawn, beehouse.theDroneIAmGoingToInsert) == null)
                    {
                        JobFailReason.Is(NoDronesFound, null);
                        return false;
                    }

                    return !t.IsBurning();
                }
            }

            return false;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            var beehouse = (Building_Beehouse)t;
            var drone = this.FindDrone(pawn, beehouse.theDroneIAmGoingToInsert);
            return new Job(RimBeesDefOf.RB_InsertingBees, beehouse, drone);
        }

        private Thing FindDrone(Pawn pawn, ThingDef drone)
        {
            return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(drone), PathEndMode.ClosestTouch, TraverseParms.For(pawn), validator: x => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, 1));
        }
    }
}
