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

            if (user.Faction == Faction.OfPlayer)
            {
                user.health.AddHediff(HediffDef.Named("RB_RecentlyResearched"));
            }

            beeresearch = new Dialog_BeeResearch(nameOfTheImage, textOfTheImage);
            Find.WindowStack.Add(beeresearch);
        }

        public override AcceptanceReport CanBeUsedBy(Pawn p)
        {
            if (p.skills == null)
            {
              
                return false;
            }
            if (p.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled)
            {
                
                return "SkillDisabled".Translate(); 
            }
            return base.CanBeUsedBy(p);
        }
    }
}

