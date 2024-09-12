using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;


namespace RimBees
{
    public class AdditionalBeeEffects_HeatPusher: AdditionalBeeEffects
    {

        public float heatPushMaxTemperature;
        public float heatPushMinTemperature;
        public float heatPerSecond;


        public AdditionalBeeEffects_HeatPusher()
        {

        }

        public bool ShouldPushHeatNow(Building_Beehouse building)
        {
          
                if (!building.SpawnedOrAnyParentSpawned)
                {
                    return false;
                }
               
                float ambientTemperature = building.AmbientTemperature;
                if (ambientTemperature < heatPushMaxTemperature)
                {
                    return ambientTemperature > heatPushMinTemperature;
                }
                return false;
            
        }


        public override void AdditionalEffectTick(Building_Beehouse building)
        {

            if (ShouldPushHeatNow(building))
            {
                GenTemperature.PushHeat(building.PositionHeld, building.MapHeld,heatPerSecond * 4.16666651f * RimBees_Settings.workerBeeEffectMultiplier);

            }
        }


    }
}