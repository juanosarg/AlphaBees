﻿using Verse;

namespace RimBees
{
    public class PlaceWorker_NextToBeeHouseLeft : PlaceWorker_NextToBeeHouse
    {
        protected override TaggedString GetFailureMessage()
        {
            return "GU_PlaceNextToBeeHouseLeft".Translate();
        }

        protected override IntVec3 GetOffsetFromBeehouse(Rot4 rot)
        {
            return rot.Opposite.RighthandCell;
        }
    }
}
