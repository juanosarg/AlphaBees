

using Verse;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;

using System.Diagnostics;


namespace RimBees
{
    class Building_Beehouse : Building, IThingHolder
    {
        //public Thing droneThing;
        // public Thing queenThing;

        public int tickCounter = 0;
        public bool BeehouseIsFull = false;
        public bool BeehouseIsRunning = false;

        public float growOptimalGlow = 0.3f;

        public int ticksToDays = 240;


        public ThingOwner innerContainerDrones = null;
        public ThingOwner innerContainerQueens = null;


        protected bool contentsKnown = false;
        protected bool contentsKnownQueens = false;

        public Map map;

        public Building_Beehouse()
        {
            this.innerContainerDrones = new ThingOwner<Thing>(this, false, LookMode.Deep);
            this.innerContainerQueens = new ThingOwner<Thing>(this, false, LookMode.Deep);

        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look<ThingOwner>(ref this.innerContainerDrones, "innerContainerDrones", new object[]
            {
                this
            });
            Scribe_Deep.Look<ThingOwner>(ref this.innerContainerQueens, "innerContainerQueens", new object[]
            {
                this
            });
           
            Scribe_Values.Look<bool>(ref this.contentsKnown, "contentsKnown", false, false);
            Scribe_Values.Look<bool>(ref this.contentsKnownQueens, "contentsKnownQueens", false, false);
            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounter", 0, false);
            Scribe_Values.Look<bool>(ref this.BeehouseIsFull, "BeehouseIsFull", false, false);
            Scribe_Values.Look<bool>(ref this.BeehouseIsRunning, "BeehouseIsRunning", false, false);


        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (base.Faction != null && base.Faction.IsPlayer)
            {
                this.contentsKnown = true;
            }
        }


        [DebuggerHidden]
        public override IEnumerable<Gizmo> GetGizmos()
        {
            map = this.Map;
            foreach (Gizmo g in base.GetGizmos())
            {
                yield return g;
            }

            if (innerContainerDrones.NullOrEmpty())
            {
                yield return BeeListSetupUtility.SetBeeListCommand(this, map);
            } else
            {
                Command_Action RB_Gizmo_Empty_Drones = new Command_Action();
                RB_Gizmo_Empty_Drones.action = delegate
                {
                    this.EjectContents();
                    
                };
                RB_Gizmo_Empty_Drones.defaultLabel = "RB_ExtractBees".Translate();
                RB_Gizmo_Empty_Drones.defaultDesc = "RB_ExtractBeesDesc".Translate();
                RB_Gizmo_Empty_Drones.icon = ContentFinder<Texture2D>.Get("UI/RB_ExtractDrones_FromBeehouse", true);
                yield return RB_Gizmo_Empty_Drones;
            }

            if (innerContainerQueens.NullOrEmpty())
            {
                yield return BeeListSetupUtility.SetQueenListCommand(this, map);
            }
            else
            {
                Command_Action RB_Gizmo_Empty_Queens = new Command_Action();
                RB_Gizmo_Empty_Queens.action = delegate
                {
                    this.EjectContentsQueens();
                    
                };
                RB_Gizmo_Empty_Queens.defaultLabel = "RB_ExtractQueens".Translate();
                RB_Gizmo_Empty_Queens.defaultDesc = "RB_ExtractQueensDesc".Translate();
                RB_Gizmo_Empty_Queens.icon = ContentFinder<Texture2D>.Get("UI/RB_ExtractQueens_FromBeehouse", true);
                yield return RB_Gizmo_Empty_Queens;
            }

        }

        public override string GetInspectString()
        {
            string text = base.GetInspectString();
            string strContentDrones = "";
            string strContentQueens = "";
            string strPercentProgress = "";
            string strDaysProgress = "";
            string strStoppedBecauseNight = " ";
            string strStoppedBecauseRain = " ";
            string strStoppedBecauseTemperature = " ";
            string strStoppedBecauseNoPLants = " ";



            if (!innerContainerDrones.NullOrEmpty())
            {
                //str = innerContainerDrones.RandomElement().def.label;
                strContentDrones = innerContainerDrones.FirstOrFallback().def.label;

            }
            else { strContentDrones = "RB_BeehouseNonePresent".Translate(); }

            if (!innerContainerQueens.NullOrEmpty())
            {
                strContentQueens = innerContainerQueens.FirstOrFallback().def.label;
            }
            else { strContentQueens = "RB_BeehouseNonePresent".Translate(); }

            if (!innerContainerDrones.NullOrEmpty()&& !innerContainerQueens.NullOrEmpty())
            {
                //str3 = (((float)tickCounter/240)*100).ToString();
                strPercentProgress = ((float)tickCounter / ((ticksToDays)* CalculateTheTicksAverage())).ToStringPercent();
                strDaysProgress = " (aprox " + CalculateTheTicksAverage().ToString("N1") + " days)";
                if (!CheckLightLevels())
                {
                    strStoppedBecauseNight = "\n"+"RB_BeehouseCombNoProgressNight".Translate();
                }
                if (!CheckRainLevels())
                {
                    strStoppedBecauseRain = "\n" + "RB_BeehouseCombNoProgressRain".Translate();
                }
                if (!CheckTemperatureLevels())
                {
                    strStoppedBecauseTemperature = "\n" + "RB_BeehouseCombNoProgressTemperature".Translate();
                }
                if (!CheckPlantsNearby())
                {
                    strStoppedBecauseNoPLants = "\n" + "RB_BeehouseCombNoProgressPlants".Translate();
                }

            }
            else {
                strPercentProgress = "RB_BeehouseCombNoProgress".Translate();
                strDaysProgress = "";

            }
      

            return text + "RB_BeehouseContainsDrone".Translate() + ": " + strContentDrones.CapitalizeFirst()
                + "      " + "RB_BeehouseContainsQueen".Translate() + ": " + strContentQueens.CapitalizeFirst() + "\n" +
                "RB_BeehouseCombProgress".Translate() + ": "+ strPercentProgress + strDaysProgress + strStoppedBecauseNight
                 + strStoppedBecauseRain  + strStoppedBecauseTemperature  + strStoppedBecauseNoPLants;
        }

        public bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
        {
            bool result;
           
                bool flag;
                if (thing.holdingOwner != null)
                {
                    thing.holdingOwner.TryTransferToContainer(thing, this.innerContainerDrones, thing.stackCount, true);
                    flag = true;
                }
                else
                {
                    flag = this.innerContainerDrones.TryAdd(thing, true);
                }
                if (flag)
                {
                    if (thing.Faction != null && thing.Faction.IsPlayer)
                    {
                        this.contentsKnown = true;
                    }
                    result = true;
                }
                else
                {
                    result = false;
                }
            
            return result;
        }

        public bool TryAcceptAnyQueen(Thing thing, bool allowSpecialEffects = true)
        {
            bool result;

            bool flag;
            if (thing.holdingOwner != null)
            {
                thing.holdingOwner.TryTransferToContainer(thing, this.innerContainerQueens, thing.stackCount, true);
                flag = true;
            }
            else
            {
                flag = this.innerContainerQueens.TryAdd(thing, true);
            }
            if (flag)
            {
                if (thing.Faction != null && thing.Faction.IsPlayer)
                {
                    this.contentsKnownQueens = true;
                }
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return this.innerContainerDrones;
        }

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
        }

        public virtual void EjectContents()
        {
            this.innerContainerDrones.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null);
            this.contentsKnown = true;
        }

        public virtual void EjectContentsQueens()
        {
            this.innerContainerQueens.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null);
            this.contentsKnownQueens = true;
        }


        public override void TickRare()
        {
            base.TickRare();
            if(!innerContainerDrones.NullOrEmpty() && !innerContainerQueens.NullOrEmpty())
            {
                if (!BeehouseIsFull) {

                   
                    if (CheckLightLevels()) {
                        if (CheckRainLevels()) {
                            if (CheckTemperatureLevels()) {
                                if (CheckPlantsNearby())
                                {
                                    BeehouseIsRunning = true;
                                    tickCounter++;
                                    if (tickCounter > ((ticksToDays * CalculateTheTicksAverage()) - 1))
                                    {
                                        SignalBeehouseFull();
                                    }
                                }
                                else
                                {
                                    BeehouseIsRunning = false;
                                }
                            }
                            else
                            {
                                BeehouseIsRunning = false;
                            }

                        }
                        else
                        {
                            BeehouseIsRunning = false;
                        }

                    }
                    else
                    {
                        BeehouseIsRunning = false;
                    }

                }

            }
            else
            {
                BeehouseIsRunning = false;
            }

        }

        public bool CheckLightLevels()
        {
            bool bee1nocturnal = innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetNocturnal;
            bool bee2nocturnal = innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetNocturnal;
            if (bee1nocturnal || bee2nocturnal)
            {
                return true;
            } else
            {
                int currentHour = GenLocalDate.HourInteger(this.Map);
                //float num = this.Map.glowGrid.GameGlowAt(this.Position, false);
                if (currentHour>=5&& currentHour<=22)
                {
                    return true;

                }
                else return false;

            }

        }

        public bool CheckRainLevels()
        {
            bool bee1pluviophile = innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetPluviophile;
            bool bee2pluviophile = innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetPluviophile;
            if (bee1pluviophile || bee2pluviophile)
            {
                return true;
            }
            else
            {
                bool isWeatherRain = (this.Map.weatherManager.curWeather.defName != "Clear")&& (this.Map.weatherManager.curWeather.defName != "Fog")&& (this.Map.weatherManager.curWeather.defName != "DryThunderstorm");
                if (isWeatherRain)
                {
                    return false;

                }
                else return true;

            }
        }

        public bool CheckTemperatureLevels()
        {
            int bee1tempMin = innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetTempMin;
            int bee2tempMin = innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetTempMin;

            int bee1tempMax = innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetTempMax;
            int bee2tempMax = innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetTempMax;

            float currentTempInMap = this.Map.mapTemperature.OutdoorTemp;

            if (   (currentTempInMap > (bee1tempMin+ bee2tempMin)/2)    &&   (currentTempInMap < (bee1tempMax+bee2tempMax)/2)   )
            {
                return true;

            }
            else return false;

        }

        public bool CheckPlantsNearby()
        {

            string bee1plantNeeded = innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetWeirdPlant;
            string bee2plantNeeded = innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetWeirdPlant;

            if ((bee1plantNeeded=="no") && (bee2plantNeeded == "no"))
            {

                return true;

            }else 
            {

                CellRect rect = GenAdj.OccupiedRect(this.Position, this.Rotation, IntVec2.One);
                rect = rect.ExpandedBy(6);

                foreach (IntVec3 current in rect.Cells)
                {
                    List<Thing> plantList = current.GetThingList(this.Map);
                    for (int i = 0; i < plantList.Count; i++)
                    {
                        if ((plantList[i].def.defName == bee1plantNeeded)||(plantList[i].def.defName == bee2plantNeeded))
                        {
                           
                            return true;
                        }
                    }

                }
                return false;


            }

        }


        public void SignalBeehouseFull()
        {
            BeehouseIsFull = true;
        }

        public float CalculateTheTicksAverage()
        {
            if (!innerContainerDrones.NullOrEmpty() && !innerContainerQueens.NullOrEmpty())
            {
                float extraRate = 1;
                if (this.def.defName == "RB_AdvancedBeehouse")
                {
                    extraRate = (float)0.9;
                }
                float bee1ticks = innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetCombtimedays;
                float bee2ticks = innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetCombtimedays;
                float beeticksaverage = extraRate*((bee1ticks + bee2ticks) / 2);
                return beeticksaverage;

            }
            else return 0;
               
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {

            EjectContents();
            EjectContentsQueens();
            base.Destroy(mode);
        }

    }
}
