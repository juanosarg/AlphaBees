using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using RimWorld;

using Verse;


namespace RimBees
{
    public class AdditionalBeeEffects_Damage : AdditionalBeeEffects
    {

        public DamageDef damage;
        public float amount;
        public float armorPenetration = 0f;
        public bool onlyHostile = true;
        public int rareTickFrequency = 2;
        public bool singleTarget = false;
        public bool setOnFire = false;
        public float chanceSetOnFire = 0.5f;

        private int tickCounter = 0;


        public AdditionalBeeEffects_Damage()
        {

        }

        private bool IsPawnAffected(Building_Beehouse building,Pawn target)
        {
          
            if (target.Dead || target.health == null)
            {
                return false;
            }

            if (onlyHostile && !target.Faction.HostileTo(Faction.OfPlayerSilentFail))
            {
                return false;
            }

            if (damage == DamageDefOf.EMP && !target.RaceProps.IsFlesh)
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
                    foreach (Pawn pawn in building.Map.mapPawns.AllPawnsSpawned.InRandomOrder())
                    {                    
                        if (IsPawnAffected(building, pawn))
                        {                      
                            DoDamage(building, pawn);
                            if (setOnFire)
                            {
                                if (Rand.Chance(chanceSetOnFire))
                                {
                                    pawn.TryAttachFire(1f, null);
                                }
                            }
                            if (singleTarget)
                            {
                                break;
                            }
                        }

                    }

                }
                tickCounter = 0;
            }
            

            tickCounter++;
        }

        private void DoDamage(Building_Beehouse building,Pawn target)
        {
           
            DamageInfo dinfo = new DamageInfo(damage, amount*RimBees_Settings.damageBeeEffectMultiplier, armorPenetration, -1f, building);

            target.TakeDamage(dinfo);

        }


    }
}