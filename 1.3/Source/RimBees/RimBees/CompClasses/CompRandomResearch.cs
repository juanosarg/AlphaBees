using System.Linq;
using Verse;

namespace RimBees
{
    public class CompRandomResearch : ThingComp
    {
        public CompProperties_RandomResearch Props
        {
            get
            {
                return (CompProperties_RandomResearch)this.props;
            }
        }

        public override void CompTick()
        {
            if (!(this.parent.ParentHolder is Pawn_CarryTracker) && this.parent.Map.IsPlayerHome)
            {
                this.Hatch();
            }
        }

        public void Hatch()
        {
            var randomFromResearchList = DefDatabase<ThingDef>.AllDefs.Where(element => element.label == Props.labelString).RandomElement();
            GenSpawn.Spawn(randomFromResearchList, this.parent.Position, this.parent.Map);
            this.parent.Destroy(DestroyMode.Vanish);
        }
    }
}
