using UnityEngine;
using Verse;
using RimWorld;
using System.Collections.Generic;


namespace RimBees
{
    public class CompUseEffect_ShowBeeResearch : CompUseEffect
    {
       
        public Dialog_BeeResearch beeresearch;

        public override void DoEffect(Pawn user)
        {
            base.DoEffect(user);
            CompBeeResearchData comp = this.parent.TryGetComp<CompBeeResearchData>();
            if (comp != null)
            {
                ThingDef firstBee = comp.Props.firstBee;
                ThingDef secondBee = comp.Props.secondBee;
                List<ThingDef> resultingBees = comp.Props.resultingBees;


                if (user.Faction == Faction.OfPlayer)
                {
                    user.health.AddHediff(InternalDefOf.RB_RecentlyResearched);
                }

                beeresearch = new Dialog_BeeResearch(firstBee, secondBee, resultingBees);
                Find.WindowStack.Add(beeresearch);
            } else
            {
                Log.Message("Bee research data item found without a CompBeeResearchData. Moan to the mod author");
            }
            
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

