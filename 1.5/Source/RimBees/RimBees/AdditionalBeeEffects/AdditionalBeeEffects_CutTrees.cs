using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.Sound;



namespace RimBees
{
    public class AdditionalBeeEffects_CutTrees : AdditionalBeeEffects
    {

        public int rareTickFrequency = 8;

        private int tickCounter = 0;

        public AdditionalBeeEffects_CutTrees()
        {

        }



        public override void AdditionalEffectTick(Building_Beehouse building)
        {
            if (tickCounter > rareTickFrequency)
            {
                if (building.Map != null)
                {
                    
                    List<Designation> designations = building.Map.designationManager.designationsByDef[DesignationDefOf.CutPlant].Concat(building.Map.designationManager.designationsByDef[DesignationDefOf.HarvestPlant]).ToList();

                    foreach (Designation item in designations)
                    {
                       
                        if (item.target.Thing.Position.InHorDistOf(building.Position, RimBees_Settings.beeEffectRadius))
                        {
                         
                            if (CanCut(item.target.Thing))
                            {
                               
                                CutPlant(item.target.Thing);
                                break;
                            }

                        }

                    }

                }
                tickCounter = 0;
            }
            tickCounter++;
        }

        public bool CanCut(Thing t)
        {
            if (t.def.category != ThingCategory.Plant)
            {
                return false;
            }
            if (t.IsBurning())
            {
                return false;
            }
            if (t.def.plant?.IsTree != true)
            {
                return false;
            }
            return true;

        }

        public void CutPlant(Thing t)
        {
            Plant plant = t as Plant;
            if (plant.def.plant.harvestedThingDef != null)
            {

                int num = plant.YieldNow();

                if (num > 0)
                {
                    Thing thing = ThingMaker.MakeThing(plant.def.plant.harvestedThingDef);
                    thing.stackCount = num;

                    GenPlace.TryPlaceThing(thing, t.Position, t.Map, ThingPlaceMode.Near);

                }
                if (plant.HarvestableNow)
                {
                    foreach (ThingComp allComp in plant.AllComps)
                    {
                        foreach (ThingDefCountClass item in allComp.GetAdditionalHarvestYield())
                        {
                            Thing thing2 = ThingMaker.MakeThing(item.thingDef);
                            thing2.stackCount = item.count;
                            GenPlace.TryPlaceThing(thing2, t.Position, t.Map, ThingPlaceMode.Near);
                        }
                    }
                }
            }

            plant.def.plant.soundHarvestFinish.PlayOneShot(t);
            plant.Destroy();




        }



    }
}