using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RimWorld;
using Verse;



namespace RimBees
{
    public class AdditionalBeeEffects_LowerPrisonerResistance : AdditionalBeeEffects
    {

        public int rareTickFrequency = 20;
        public bool doDamage = true;
        public DamageDef damage;
        public float amount;
        public float armorPenetration = 0f;
        public float resistanceLoweredBy = 1;
       

        private int tickCounter = 0;


        public AdditionalBeeEffects_LowerPrisonerResistance()
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

            if (!target.guest.Recruitable)
            {
                return false;
            }

            if (target.guest.Resistance<=0)
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
                            pawn.guest.resistance = Mathf.Max(0f, pawn.guest.resistance - (resistanceLoweredBy*RimBees_Settings.workerBeeEffectMultiplier));
                            if (pawn.guest.Resistance <= 0)
                            {
                                pawn.guest.resistance = 0;
                            }

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