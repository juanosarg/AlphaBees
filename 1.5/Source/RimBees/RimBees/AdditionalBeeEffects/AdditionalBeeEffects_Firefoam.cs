using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using static HarmonyLib.Code;
using Verse.AI;
using static UnityEngine.GraphicsBuffer;



namespace RimBees
{
    public class AdditionalBeeEffects_Firefoam : AdditionalBeeEffects
    {

     
        public int rareTickFrequency = 2;
        private int tickCounter = 0;

        public AdditionalBeeEffects_Firefoam()
        {
        }

        public override void AdditionalEffectTick(Building_Beehouse building)
        {
            if (tickCounter > rareTickFrequency)
            {
                if (building.Map != null)
                {

                    if (GenClosest.ClosestThingReachable(building.Position, building.Map, ThingRequest.ForDef(ThingDefOf.Fire), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors), RimBees_Settings.beeEffectRadius) != null)
                    {
                        GenExplosion.DoExplosion(building.Position, building.Map, RimBees_Settings.beeEffectRadius, DamageDefOf.Extinguish, null, -1, -1f, SoundDefOf.Explosion_FirefoamPopper, null, null, null, ThingDefOf.Filth_FireFoam, 1f, 1, null, applyDamageToExplosionCellsNeighbors: true);
                    }
                }
                tickCounter = 0;
            }
            tickCounter++;
        }



    }
}