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
    public class AdditionalBeeEffects_Sow : AdditionalBeeEffects
    {


        public int rareTickFrequency = 10;
        private int tickCounter = 0;
      

        public AdditionalBeeEffects_Sow()
        {
        }

        public override void AdditionalEffectTick(Building_Beehouse building)
        {
            if (tickCounter > rareTickFrequency)
            {
                
                if (building.Map != null)
                {
                    IEnumerable<IntVec3> cells = GenRadial.RadialCellsAround(building.Position, RimBees_Settings.beeEffectRadius, useCenter: true);

                    foreach (IntVec3 current in cells.InRandomOrder())
                    {
                        if (!PlantUtility.GrowthSeasonNow(current, building.Map, forSowing: true))
                        {
                            continue;
                        }

                        
                        Zone_Growing zone;
                        if ((zone = (current.GetZone(building.Map) as Zone_Growing))!=null)
                        {

                            ThingDef wantedPlantDef = zone.PlantDefToGrow;
                            List<Thing> thingList = current.GetThingList(building.Map);


                            if (wantedPlantDef.plant.sowMinSkill > 8 || !wantedPlantDef.CanNowPlantAt(current, building.Map))
                            {
                                continue;
                            }
                            bool flag = false;
                            for (int i = 0; i < thingList.Count; i++)
                            {
                                Thing thing = thingList[i];
                              
                                if ((thing is Blueprint || thing is Frame) && thing.Faction == Faction.OfPlayerSilentFail)
                                {
                                    flag = true;
                                }
                            }
                            if (flag)
                            {
                                Thing edifice = current.GetEdifice(building.Map);
                                if (edifice == null || edifice.def.fertility < 0f)
                                {
                                    continue;
                                }
                            }
                           
                            Plant plant = current.GetPlant(building.Map);
                            if (plant != null && plant.def.plant.blockAdjacentSow)
                            {
                                
                                if (zone != null && !zone.allowCut)
                                {
                                    continue;
                                }
                               
                            }
                            Thing thing2 = PlantUtility.AdjacentSowBlocker(wantedPlantDef, current, building.Map);
                            if (thing2 != null)
                            {
                                Plant plant2 = thing2 as Plant;
                                if (plant2 != null)
                                {
                                    IPlantToGrowSettable plantToGrowSettable = plant2.Position.GetPlantToGrowSettable(plant2.Map);
                                    if (plantToGrowSettable == null || plantToGrowSettable.GetPlantDefToGrow() != plant2.def)
                                    {
                                        Zone_Growing zone_Growing2 = current.GetZone(building.Map) as Zone_Growing;
                                        Zone_Growing zone_Growing3 = plant2.Position.GetZone(building.Map) as Zone_Growing;
                                        if ((zone_Growing2 != null && !zone_Growing2.allowCut) || (zone_Growing3 != null && !zone_Growing3.allowCut && plant2.def == zone_Growing3.GetPlantDefToGrow()))
                                        {
                                            continue;
                                        }
                                        if (PlantUtility.TreeMarkedForExtraction(plant2))
                                        {
                                            continue;
                                        }
                                        plant2.Destroy();
                                        
                                    }
                                }
                                continue; ;
                            }
                            
                            for (int j = 0; j < thingList.Count; j++)
                            {
                                Thing thing3 = thingList[j];
                                if (!thing3.def.BlocksPlanting())
                                {
                                    continue;
                                }
                                
                                if (thing3.def.category == ThingCategory.Plant)
                                {
                                    
                                        if (zone != null && !zone.allowCut)
                                        {
                                        continue;
                                    }
                                        
                                        if (PlantUtility.TreeMarkedForExtraction(thing3))
                                        {
                                        continue;
                                    }
                                    thing3.Destroy();

                                    continue;
                                }
                                
                            }
                            Plant planted = GenSpawn.Spawn(wantedPlantDef, current, building.Map) as Plant;
                            planted.Growth = 0.0001f;
                            building.Map.mapDrawer.MapMeshDirty(planted.Position, MapMeshFlagDefOf.Things);
                          
                            Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SowedPlant));
                            if (planted.def.plant.humanFoodPlant)
                            {
                                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SowedHumanFoodPlant));
                            }
                            break;





                        }
                       

                    }
                    //plantfound: { }

                }
                tickCounter = 0;
            }
            tickCounter++;
        }


     


    }
}