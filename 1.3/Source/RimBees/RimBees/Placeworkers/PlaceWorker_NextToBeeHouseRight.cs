using Verse;

namespace RimBees
{
    public class PlaceWorker_NextToBeeHouseRight : PlaceWorker_NextToBeeHouse
    {
        protected override TaggedString GetFailureMessage()
        {
            return "GU_PlaceNextToBeeHouse".Translate();
        }

        protected override IntVec3 GetOffsetFromBeehouse(Rot4 rot)
        {
            return rot.RighthandCell;
        }
    }
}
