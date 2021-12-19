using RimWorld;
using Verse;

namespace RimBees
{
    public class CompUseEffect_ShowBeeResearch : CompUseEffect
    {
        public Dialog_BeeResearch beeresearch;

        public override void DoEffect(Pawn user)
        {
            base.DoEffect(user);
            var image = this.parent.TryGetComp<CompBeeResearchImages>().GetTexture;
            var label = this.parent.TryGetComp<CompBeeResearchImages>().GetText;

            if (user.Faction == Faction.OfPlayer)
            {
                user.health.AddHediff(HediffDef.Named("RB_RecentlyResearched"));
            }

            beeresearch = new Dialog_BeeResearch(image, label);
            Find.WindowStack.Add(beeresearch);
        }

        public override bool CanBeUsedBy(Pawn p, out string failReason)
        {
            if (p.skills == null)
            {
                failReason = null;
                return false;
            }

            if (p.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled)
            {
                failReason = "SkillDisabled".Translate();
                return false;
            }

            return base.CanBeUsedBy(p, out failReason);
        }
    }
}

