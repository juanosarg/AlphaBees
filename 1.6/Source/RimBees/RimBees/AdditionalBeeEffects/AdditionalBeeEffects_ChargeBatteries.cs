using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using RimWorld;
using Verse;



namespace RimBees
{
    public class AdditionalBeeEffects_ChargeBatteries : AdditionalBeeEffects
    {

        public int charge;
        public int rareTickFrequency = 2;
        private int tickCounter = 0;

        public AdditionalBeeEffects_ChargeBatteries()
        {

        }

       

        public override void AdditionalEffectTick(Building_Beehouse building)
        {
            if (tickCounter > rareTickFrequency)
            {
                if (building.Map != null)
                {

                   foreach(PowerNet net in building.Map.powerNetManager.AllNetsListForReading)
                    {
                        foreach(CompPowerBattery battery in net.batteryComps)
                        {

                            if(battery.parent.PositionHeld.DistanceTo(building.PositionHeld) <= RimBees_Settings.beeEffectRadius)
                            {
                                FleckMaker.ThrowMicroSparks(battery.parent.Position.ToVector3(), battery.parent.Map);  
                                battery.AddEnergy(charge*RimBees_Settings.workerBeeEffectMultiplier);
                                break;                              

                            }


                        }

                    }
                }
                tickCounter = 0;
            }
            tickCounter++;
        }



    }
}