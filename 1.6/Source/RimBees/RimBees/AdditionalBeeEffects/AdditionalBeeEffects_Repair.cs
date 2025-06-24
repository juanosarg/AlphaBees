using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;



namespace RimBees
{
    public class AdditionalBeeEffects_Repair : AdditionalBeeEffects
    {

        public int amount;
        public int rareTickFrequency = 2;
        private int tickCounter = 0;

        public AdditionalBeeEffects_Repair()
        {
        }

        public override void AdditionalEffectTick(Building_Beehouse building)
        {
            if (tickCounter > rareTickFrequency)
            {
                if (building.Map != null)
                {

                   foreach(Thing buildingToRepair in building.Map.listerBuildingsRepairable.RepairableBuildings(Faction.OfPlayerSilentFail))
                    {
                       
                            if(buildingToRepair.PositionHeld.DistanceTo(building.PositionHeld) <= RimBees_Settings.beeEffectRadius)
                            {
                                FleckMaker.ThrowMicroSparks(buildingToRepair.Position.ToVector3(), buildingToRepair.Map);
                                buildingToRepair.HitPoints+=(int)(amount * RimBees_Settings.workerBeeEffectMultiplier);
                                buildingToRepair.HitPoints = Mathf.Min(buildingToRepair.HitPoints, buildingToRepair.MaxHitPoints);
                                buildingToRepair.Map.listerBuildingsRepairable.Notify_BuildingRepaired((Building)buildingToRepair);
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