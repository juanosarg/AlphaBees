using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using RimWorld;
using Verse.Sound;
using Verse;



namespace RimBees
{
    public class AdditionalBeeEffects_Harvest : AdditionalBeeEffects
    {


        public int rareTickFrequency = 10;
        private int tickCounter = 0;
        public PlantDestructionMode PlantDestructionMode => PlantDestructionMode.Smash;

        public AdditionalBeeEffects_Harvest()
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
                        List<Thing> plantList = current.GetThingList(building.Map);
                        for (int i = 0; i < plantList.Count; i++)
                        {
                            Plant plant;
                            if ((plant = plantList[i] as Plant) != null && plant.HarvestableNow)
                            {
                                HarvestPlant(plant, building);
                                goto plantfound;
                            }
                        }

                    }
                    plantfound: { }

                }
                tickCounter = 0;
            }
            tickCounter++;
        }


        public void HarvestPlant(Plant plant, Building_Beehouse building)
        {

            int num = plant.YieldNow();

            if (num > 0)
            {
                Thing thing = ThingMaker.MakeThing(plant.def.plant.harvestedThingDef);
                thing.stackCount = num;

                GenPlace.TryPlaceThing(thing, plant.Position, plant.Map, ThingPlaceMode.Near);

            }

            foreach (ThingComp allComp in plant.AllComps)
            {
                foreach (ThingDefCountClass item in allComp.GetAdditionalHarvestYield())
                {
                    Thing thing2 = ThingMaker.MakeThing(item.thingDef);
                    thing2.stackCount = item.count;
                    GenPlace.TryPlaceThing(thing2, plant.Position, plant.Map, ThingPlaceMode.Near);
                }
            }
            plant.def.plant.soundHarvestFinish.PlayOneShot(building);
            plant.Destroy();
           

        }


    }
}