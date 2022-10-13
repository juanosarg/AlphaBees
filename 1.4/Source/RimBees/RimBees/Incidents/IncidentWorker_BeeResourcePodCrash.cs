using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace RimBees
{
    public class IncidentWorker_BeeResourcePodCrash : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            List<Thing> things = DefDatabase<ThingSetMakerDef>.GetNamed("RB_BeeResourcePod").root.Generate();
            IntVec3 intVec = DropCellFinder.RandomDropSpot(map);
            DropPodUtility.DropThingsNear(intVec, map, things, 110, false, true, true, true);
            base.SendStandardLetter("RB_LetterLabelBeeCargoPodCrash".Translate(), "RB_BeeCargoPodCrash".Translate(), LetterDefOf.PositiveEvent, parms, new TargetInfo(intVec, map, false), Array.Empty<NamedArgument>());
            return true;
        }
    }
}