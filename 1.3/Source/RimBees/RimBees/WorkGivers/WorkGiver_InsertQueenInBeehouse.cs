using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace RimBees
{
    public class WorkGiver_InsertQueenInBeehouse : WorkGiver_Scanner
    {
        private static string NoQueensFound;

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
            NoQueensFound = "RB_NoQueensFound".Translate();
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            var beehouse = t as Building_Beehouse;
            if (beehouse?.BeehouseIsExpectingQueens != true)
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

                    if (this.FindQueen(pawn, beehouse.theQueenIAmGoingToInsert) == null)
                    {
                        JobFailReason.Is(NoQueensFound, null);
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
            var queen = this.FindQueen(pawn, beehouse.theQueenIAmGoingToInsert);
            return new Job(RimBeesDefOf.RB_InsertingQueenBees, beehouse, queen);
        }

        private Thing FindQueen(Pawn pawn, ThingDef queen)
        {
            return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(queen), PathEndMode.ClosestTouch, TraverseParms.For(pawn), validator: x => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, 1));
        }
    }
}

