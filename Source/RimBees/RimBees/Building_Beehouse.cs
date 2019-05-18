

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
        public bool BeehouseIsExpectingBees = false;


        public float growOptimalGlow = 0.3f;

        public int ticksToDays = 240;
        public int ticksToResetJobs = 20;


        public ThingOwner innerContainerDrones = null;
        public ThingOwner innerContainerQueens = null;

        public string whichPlantNeeds = "";

        public bool flagLight = false;
        public bool flagTemperature = false;
        public bool flagRain = false;
        public bool flagPlants = false;

        public bool flagInitializeConditions = false;

        public int avgTempMin = 0;
        public int avgTempMax = 0;





        protected bool contentsKnown = false;
        protected bool contentsKnownQueens = false;

        public Map map;

        /// <summary>
        /// Returns the graphic of the object.
        /// The renderer will draw the needed object graphic from here.
        /// </summary>
        public override Graphic Graphic
        {
            get
            {
                var customSuffix = "";

                if (BeehouseIsFull)
                    customSuffix = "_NeedRecharge";
                else if (innerContainerDrones.NullOrEmpty() || innerContainerQueens.NullOrEmpty())
                    customSuffix = "_NoBees";
                else if (flagInitializeConditions && !flagLight)
                    customSuffix = "_NoLight";
                else if (flagInitializeConditions && !flagRain)
                    customSuffix = "_Rain";
                else if (flagInitializeConditions && !flagTemperature)
                    customSuffix = "_WrongTemperature";
                else if (flagInitializeConditions && !flagPlants)
                    customSuffix = "_NoPlants";
                else if (!BeehouseIsRunning)
                    customSuffix = "_Stopped";

                if (string.IsNullOrEmpty(customSuffix))
                    return base.Graphic;

                return GraphicDatabase.Get(def.graphicData.graphicClass,
                                           def.graphicData.texPath + customSuffix,
                                           def.graphic.Shader,
                                           def.graphicData.drawSize,
                                           def.graphic.Color,
                                           def.graphic.ColorTwo);
            }
        }

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
            Scribe_Values.Look<bool>(ref this.BeehouseIsExpectingBees, "BeehouseIsExpectingBees", false, false);
            Scribe_Values.Look<string>(ref this.whichPlantNeeds, "whichPlantNeeds", "", false);
            Scribe_Values.Look<bool>(ref this.flagLight, "flagLight", false, false);
            Scribe_Values.Look<bool>(ref this.flagTemperature, "flagTemperature", false, false);
            Scribe_Values.Look<bool>(ref this.flagRain, "flagRain", false, false);
            Scribe_Values.Look<bool>(ref this.flagPlants, "flagPlants", false, false);
            Scribe_Values.Look<bool>(ref this.flagInitializeConditions, "flagInitializeConditions", false, false);
            Scribe_Values.Look<int>(ref this.avgTempMin, "avgTempMin", 0, false);
            Scribe_Values.Look<int>(ref this.avgTempMax, "avgTempMax", 0, false);







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
            if (this.BeehouseIsExpectingBees) {
                Command_Action RB_Gizmo_Drones_Waiting = new Command_Action();
                RB_Gizmo_Drones_Waiting.action = delegate
                {
                   

                };
                RB_Gizmo_Drones_Waiting.defaultLabel = "RB_InsertBees".Translate();
                RB_Gizmo_Drones_Waiting.defaultDesc = "RB_InsertBeesDesc".Translate();
                RB_Gizmo_Drones_Waiting.icon = ContentFinder<Texture2D>.Get("UI/RB_Drones_Waiting", true);
                yield return RB_Gizmo_Drones_Waiting;
            }
            else if (innerContainerDrones.NullOrEmpty())
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

            if (this.BeehouseIsExpectingBees)
            {
                Command_Action RB_Gizmo_Queens_Waiting = new Command_Action();
                RB_Gizmo_Queens_Waiting.action = delegate
                {


                };
                RB_Gizmo_Queens_Waiting.defaultLabel = "RB_InsertQueens".Translate();
                RB_Gizmo_Queens_Waiting.defaultDesc = "RB_InsertQueensDesc".Translate();
                RB_Gizmo_Queens_Waiting.icon = ContentFinder<Texture2D>.Get("UI/RB_Queens_Waiting", true);
                yield return RB_Gizmo_Queens_Waiting;
            } else
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

            string strToAddSpaceIfElectricityUsed = "";

            if (this.def.defName == "RB_AdvancedBeehouse")
            {
                strToAddSpaceIfElectricityUsed = "\n";
            }

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
                if (flagInitializeConditions) {
                    if (!flagLight)
                    {
                        strStoppedBecauseNight = "\n" + "RB_BeehouseCombNoProgressNight".Translate();
                    } else
                    if (!flagRain)
                    {
                        strStoppedBecauseRain = "\n" + "RB_BeehouseCombNoProgressRain".Translate();
                    }
                    else
                    if (!flagTemperature)
                    {
                        strStoppedBecauseTemperature = "\n" + "RB_BeehouseCombNoProgressTemperature".Translate()+avgTempMin+"-"+avgTempMax+"ºC)";
                    }
                    else
                    if (!flagPlants)
                    {
                        strStoppedBecauseNoPLants = "\n" + "RB_BeehouseCombNoProgressPlants1".Translate() + whichPlantNeeds + "RB_BeehouseCombNoProgressPlants2".Translate();
                    }
                }
                

            }
            else {
                strPercentProgress = "RB_BeehouseCombNoProgress".Translate();
                strDaysProgress = "";

            }
      

            return text + strToAddSpaceIfElectricityUsed + "RB_BeehouseContainsDrone".Translate() + ": " + strContentDrones.CapitalizeFirst()
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
            this.TickRare();
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
            this.TickRare();
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
            flagInitializeConditions = false;
            this.TickRare();
        }

        public virtual void EjectContentsQueens()
        {
            this.innerContainerQueens.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null);
            this.contentsKnownQueens = true;
            flagInitializeConditions = false;
            this.TickRare();
        }


        public override void TickRare()
        {
            if(BeehouseIsExpectingBees)
            {
                ticksToResetJobs--;
                if (ticksToResetJobs <= 0)
                {
                    ticksToResetJobs = 20;
                    this.BeehouseIsExpectingBees = false;
                    
                }
            }
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
                flagInitializeConditions = true;


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
                flagLight = true;
                return true;
            } else
            {
                int currentHour = GenLocalDate.HourInteger(this.Map);
                //float num = this.Map.glowGrid.GameGlowAt(this.Position, false);
                if (currentHour>=5&& currentHour<=22)
                {
                    flagLight = true;
                    return true;

                }
                else {
                    flagLight = false;
                    return false;
                }

            }

        }

        public bool CheckRainLevels()
        {
            bool bee1pluviophile = innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetPluviophile;
            bool bee2pluviophile = innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetPluviophile;
            if (bee1pluviophile || bee2pluviophile)
            {
                flagRain = true;
                return true;
            }
            else
            {
                bool isWeatherRain = (this.Map.weatherManager.curWeather.defName != "Clear")&& (this.Map.weatherManager.curWeather.defName != "Fog")&& (this.Map.weatherManager.curWeather.defName != "DryThunderstorm");
                if (isWeatherRain)
                {
                    flagRain = false;
                    return false;

                }
                else
                {
                    flagRain = true;
                    return true;
                }

            }
        }

        public bool CheckTemperatureLevels()
        {
            if (this.def.defName == "RB_ClimatizedBeehouse")
            {
                flagTemperature = true;
                return true;
            }
            int bee1tempMin = innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetTempMin;
            int bee2tempMin = innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetTempMin;

            int bee1tempMax = innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetTempMax;
            int bee2tempMax = innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetTempMax;

            avgTempMin = (bee1tempMin + bee2tempMin) / 2;
            avgTempMax = (bee1tempMax + bee2tempMax) / 2;

            float currentTempInMap = this.Map.mapTemperature.OutdoorTemp;

            if ((currentTempInMap > avgTempMin) && (currentTempInMap < avgTempMax))
            {
                flagTemperature = true;
                return true;

            }
            else {
                flagTemperature = false;
                return false;
            }

        }

        public bool CheckPlantsNearby()
        {

            string bee1plantNeeded = innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetWeirdPlant;
            string bee2plantNeeded = innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetWeirdPlant;

            if ((bee1plantNeeded=="no") && (bee2plantNeeded == "no"))
            {
                flagPlants = true;
                return true;

            }else 
            {
                if (bee1plantNeeded == "no") {
                    whichPlantNeeds = ThingDef.Named(bee2plantNeeded).label;

                } else if (bee2plantNeeded == "no")
                {
                    whichPlantNeeds = ThingDef.Named(bee1plantNeeded).label;

                }

                CellRect rect = GenAdj.OccupiedRect(this.Position, this.Rotation, IntVec2.One);
                rect = rect.ExpandedBy(6);

                foreach (IntVec3 current in rect.Cells)
                {
                    List<Thing> plantList = current.GetThingList(this.Map);
                    for (int i = 0; i < plantList.Count; i++)
                    {
                        if ((plantList[i].def.defName == bee1plantNeeded)||(plantList[i].def.defName == bee2plantNeeded))
                        {
                            flagPlants = true;
                            return true;
                        }
                    }

                }
                flagPlants = false;
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
                if (this.def.defName == "RB_ClimatizedBeehouse")
                {
                    extraRate = (float)2.5;
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
