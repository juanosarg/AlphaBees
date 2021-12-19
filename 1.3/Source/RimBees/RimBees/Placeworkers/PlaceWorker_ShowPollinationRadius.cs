using UnityEngine;
using Verse;

namespace RimBees
{
    public class PlaceWorker_ShowPollinationRadius : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            GenDraw.DrawRadiusRing(center, 6);
        }
    }
}
