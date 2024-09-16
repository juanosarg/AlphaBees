using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.Sound;



namespace RimBees
{
    public class AdditionalBeeEffects_Flick : AdditionalBeeEffects
    {

        public int rareTickFrequency = 8;

        private int tickCounter = 0;

        public AdditionalBeeEffects_Flick()
        {

        }



        public override void AdditionalEffectTick(Building_Beehouse building)
        {
            if (tickCounter > rareTickFrequency)
            {
                if (building.Map != null)
                {
                    
                    List<Designation> designations = building.Map.designationManager.designationsByDef[DesignationDefOf.Flick].ToList();
                    if(designations.Any()) {

                        foreach (Designation item in designations.InRandomOrder())
                        {

                            if (item.target.Thing.Position.InHorDistOf(building.Position, RimBees_Settings.beeEffectRadius))
                            {

                                ThingWithComps thingWithComps = (ThingWithComps)item.target.Thing;
                                for (int i = 0; i < thingWithComps.AllComps.Count; i++)
                                {
                                    CompFlickable compFlickable = thingWithComps.AllComps[i] as CompFlickable;
                                    if (compFlickable != null && compFlickable.WantsFlick())
                                    {
                                        compFlickable.DoFlick();
                                    }
                                }

                                building.Map.designationManager.DesignationOn(thingWithComps, DesignationDefOf.Flick)?.Delete();

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