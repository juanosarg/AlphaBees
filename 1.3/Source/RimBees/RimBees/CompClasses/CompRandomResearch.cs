using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimBees
{
    public class CompRandomResearch : ThingComp
    {
        List<ThingDef> researchResults = new List<ThingDef>();

        public CompProperties_RandomResearch Props
        {
            get
            {
                return (CompProperties_RandomResearch)this.props;
            }
        }

        public override void CompTick()
        {

            // Log.Warning(this.parent.ParentHolder.ToString());
            if (!(this.parent.ParentHolder is Pawn_CarryTracker) && this.parent.Map.IsPlayerHome)
            {
                this.Hatch();
            }
        }

        public void Hatch()
        {
            foreach (ThingDef element in DefDatabase<ThingDef>.AllDefs.Where(element => element.label == Props.labelString))
            {
                //Log.Message(element.defName);
                researchResults.Add(element);

            }
            ThingDef randomFromResearchList = researchResults.RandomElement();
            Log.Message(randomFromResearchList.defName);
            GenSpawn.Spawn(ThingDef.Named(randomFromResearchList.defName), this.parent.Position, this.parent.Map);
            this.parent.Destroy(DestroyMode.Vanish);
        }
    }
}

