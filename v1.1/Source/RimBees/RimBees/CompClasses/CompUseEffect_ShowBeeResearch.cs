using UnityEngine;
using Verse;
using RimWorld;

namespace RimBees
{
    public class CompUseEffect_ShowBeeResearch : CompUseEffect
    {
       
        public Dialog_BeeResearch beeresearch;

        public override void DoEffect(Pawn user)
        {
            base.DoEffect(user);
            string nameOfTheImage = this.parent.TryGetComp<CompBeeResearchImages>().GetImage;
            string textOfTheImage = this.parent.TryGetComp<CompBeeResearchImages>().GetText;

            beeresearch = new Dialog_BeeResearch(nameOfTheImage, textOfTheImage);
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

