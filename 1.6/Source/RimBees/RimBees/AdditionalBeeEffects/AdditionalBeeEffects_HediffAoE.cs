using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;


namespace RimBees
{
    public class AdditionalBeeEffects_HediffAoE : AdditionalBeeEffects
    {

        public HediffDef hediffDef;
        public bool affectAnimals = false;



        public AdditionalBeeEffects_HediffAoE()
        {

        }

        private bool IsPawnAffected(Building_Beehouse building, Pawn target)
        {

            if (target.Dead || target.health == null)
            {
                return false;
            }

            

            return target.PositionHeld.DistanceTo(building.PositionHeld) <= RimBees_Settings.beeEffectRadius;


        }


        public override void AdditionalEffectTick(Building_Beehouse building)
        {

            if (building.Map != null)
            {
                List<Pawn> pawns;
                if (affectAnimals)
                {
                    pawns = building.Map.mapPawns.SpawnedColonyAnimals;                 
                }
                else
                {
                    pawns = building.Map.mapPawns.FreeColonists;
                }
                if (pawns.Any()) {
                    foreach (Pawn pawn in pawns)
                    {
                        if (IsPawnAffected(building, pawn))
                        {
                            GiveOrUpdateHediff(building, pawn);
                        }
                    }
                }            
            }
        }

        private void GiveOrUpdateHediff(Building_Beehouse building, Pawn target)
        {
            Hediff hediff = target.health.hediffSet.GetFirstHediffOfDef(hediffDef);
            if (hediff == null)
            {
                hediff = target.health.AddHediff(hediffDef, target.health.hediffSet.GetBrain());
                hediff.Severity = 1f;
                HediffComp_Link hediffComp_Link = hediff.TryGetComp<HediffComp_Link>();
                if (hediffComp_Link != null)
                {
                    hediffComp_Link.drawConnection = true;
                    hediffComp_Link.other = building;
                }
            }
            HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
            if (hediffComp_Disappears == null)
            {
                Log.ErrorOnce("CompCauseHediff_AoE has a hediff in props which does not have a HediffComp_Disappears", 78945945);
            }
            else
            {
                hediffComp_Disappears.ticksToDisappear = 500;
            }

        }


    }
}