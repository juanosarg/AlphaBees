using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RimWorld;
using Verse;



namespace RimBees
{
    public class AdditionalBeeEffects_LowerPrisonerCertainty : AdditionalBeeEffects
    {

        public int rareTickFrequency = 20;
        public bool doDamage = true;
        public DamageDef damage;
        public float amount;
        public float armorPenetration = 0f;
        public float certaintyLoweredBy = 1;
       

        private int tickCounter = 0;


        public AdditionalBeeEffects_LowerPrisonerCertainty()
        {

        }

        private bool IsPawnAffected(Building_Beehouse building,Pawn target)
        {
          
            if (target.Dead || target.health == null)
            {
                return false;
            }

            if (!target.IsPrisonerOfColony)
            {
                return false;
            }

            if (target.DevelopmentalStage.Baby())
            {
                return false;
            }

            if (target.ideo?.Certainty <= 0)
            {
                return false;
            }

            if (target.ideo?.Ideo == Current.Game.World?.factionManager?.OfPlayer?.ideos?.PrimaryIdeo)
            {
                return false;
            }


            return target.PositionHeld.DistanceTo(building.PositionHeld) <= RimBees_Settings.beeEffectRadius;
            
            
        }


        public override void AdditionalEffectTick(Building_Beehouse building)
        {

            if (tickCounter > rareTickFrequency-1)
            {
                if (building.Map != null)
                {
                    foreach (Pawn pawn in building.Map.mapPawns.PrisonersOfColonySpawned.InRandomOrder())
                    {                    
                        if (IsPawnAffected(building, pawn))
                        {
                            pawn.ideo.Debug_ReduceCertainty(certaintyLoweredBy * RimBees_Settings.workerBeeEffectMultiplier);
                           
                            DebugActionsUtility.DustPuffFrom(pawn);
                            DamageInfo dinfo = new DamageInfo(damage, amount * RimBees_Settings.damageBeeEffectMultiplier, armorPenetration, -1f, building);
                            pawn.TakeDamage(dinfo);
                            break;
                        }

                    }

                }
                tickCounter = 0;
            }
            

            tickCounter++;
        }

       


    }
}