using System;
using UnityEngine;
using Verse;
using RimWorld;

namespace RimBees
{
    public class PlaceWorker_ShowPollinationRadius : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            
            GenDraw.DrawRadiusRing(center, RimBees_Settings.beeEffectRadius);
        }
    }
}