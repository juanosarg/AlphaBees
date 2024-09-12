using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;



namespace RimBees
{
    public class AdditionalBeeEffects_CleanArea : AdditionalBeeEffects
    {

        public int rareTickFrequency = 8;

        private int tickCounter = 0;

        public AdditionalBeeEffects_CleanArea()
        {

        }

       

        public override void AdditionalEffectTick(Building_Beehouse building)
        {
            if (tickCounter > rareTickFrequency)
            {
                if (building.Map != null)
                {
                    foreach (Thing filth in building.Map.listerFilthInHomeArea.FilthInHomeArea)
                    {
                        if (filth.Position.InHorDistOf(building.Position, RimBees_Settings.beeEffectRadius))
                        {
                            filth.Destroy(DestroyMode.Vanish);
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