using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using RimWorld;
using Verse.Sound;
using Verse;
using static UnityEngine.GraphicsBuffer;
using Verse.AI;
using System.Security.Cryptography;
using UnityEngine;



namespace RimBees
{
    public class AdditionalBeeEffects_Mine : AdditionalBeeEffects
    {


        public int rareTickFrequency = 3;
        private int tickCounter = 0;
        private Effecter effecter;

        public AdditionalBeeEffects_Mine()
        {
        }

        public override void AdditionalEffectTick(Building_Beehouse building)
        {
            if (tickCounter > rareTickFrequency)
            {
                if (building.Map != null)
                {
                    IEnumerable<IntVec3> cells = GenRadial.RadialCellsAround(building.Position, RimBees_Settings.beeEffectRadius, useCenter: true);

                    foreach (IntVec3 current in cells)
                    {
                        

                        if (building.Map.designationManager.DesignationAt(current, DesignationDefOf.Mine) != null && current.InBounds(building.Map)               
                            )
                        {
                            List<Thing> thingsInCell = current.GetThingList(building.Map);
                            foreach (Thing thing in thingsInCell)
                            {
                                if(thing is Mineable mineable)
                                {
                                    effecter = EffecterDefOf.Mine.Spawn();
                                    effecter.Trigger(thing, thing);
                                    DoDamage(thing, thing.Position);
                                    break;
                                }
                            }
                            
                        }
                    }


                }
                tickCounter = 0;
            }
            tickCounter++;
        }


        public void DoDamage(Thing target, IntVec3 mineablePos)
        {
            int num = (target.def.building.isNaturalRock ? 80 : 40);
            Mineable mineable = target as Mineable;
            if (mineable == null || target.HitPoints > num)
            {
                DamageInfo dinfo = new DamageInfo(DamageDefOf.Mining, num, 0f, -1f);
                target.TakeDamage(dinfo);
                return;
            }
          
            mineable.Notify_TookMiningDamage(target.HitPoints, null);
            mineable.HitPoints = 0;
            DestroyMined(target);
           
        }

        public void DestroyMined(Thing target)
        {
            Map map = target.Map;
            IntVec3 position = target.Position;
            ThingDef targetDef = target.def;
            target.Destroy(DestroyMode.KillFinalize);
            TrySpawnYield(targetDef, map, position);
           
        }
        private void TrySpawnYield(ThingDef targetDef,Map map,IntVec3 position)
        {
            if (targetDef.building.mineableThing != null && !(Rand.Value > targetDef.building.mineableDropChance))
            {
                int num = Mathf.Max(1, targetDef.building.EffectiveMineableYield);
               
                Thing thing2 = ThingMaker.MakeThing(targetDef.building.mineableThing);
                thing2.stackCount = num;
                GenPlace.TryPlaceThing(thing2, position, map, ThingPlaceMode.Near);
            }
           
        }


    }
}