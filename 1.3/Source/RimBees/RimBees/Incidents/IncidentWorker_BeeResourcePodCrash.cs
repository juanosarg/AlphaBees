using RimWorld;
using Verse;

namespace RimBees
{
    public class IncidentWorker_BeeResourcePodCrash : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            var map = (Map)parms.target;
            var contents = RimBeesDefOf.RB_BeeResourcePod.root.Generate(new ThingSetMakerParams
            {
                totalMarketValueRange = new FloatRange(150f, 600f),
            });
            var position = DropCellFinder.RandomDropSpot(map);

            DropPodUtility.DropThingsNear(position, map, contents, 110, false, true, true, true);
            this.SendStandardLetter("RB_LetterLabelBeeCargoPodCrash".Translate(), "RB_BeeCargoPodCrash".Translate(), LetterDefOf.PositiveEvent, parms, new TargetInfo(position, map, false));

            return true;
        }
    }
}
