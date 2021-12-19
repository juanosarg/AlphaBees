﻿

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;


namespace RimBees
{
    public class Building_Beehouse : Building, IThingHolder
    {

        public int tickCounter = 0;
        public bool BeehouseIsFull = false;
        public bool BeehouseIsRunning = false;
        public bool BeehouseIsExpectingBees = false;
        public bool BeehouseIsExpectingQueens = false;

        public float growOptimalGlow = 0.3f;

        public int ticksToDays = 240;
        public int ticksToResetJobs = 120;

        public ThingOwner innerContainerDrones = null;
        public ThingOwner innerContainerQueens = null;

        public string whichPlantNeeds = "";

        public bool flagLight = false;
        public bool flagTemperature = false;
        public bool flagRain = false;
        public bool flagPlants = false;
        public bool flagPower = false;

        public bool flagInitializeConditions = false;

        public int avgTempMin = 0;
        public int avgTempMax = 0;

        public ThingDef theDroneIAmGoingToInsert;
        public ThingDef theQueenIAmGoingToInsert;

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
            Scribe_Deep.Look(ref this.innerContainerDrones, "innerContainerDrones", new[] { this });
            Scribe_Deep.Look(ref this.innerContainerQueens, "innerContainerQueens", new[] { this });

            Scribe_Values.Look(ref this.contentsKnown, "contentsKnown");
            Scribe_Values.Look(ref this.contentsKnownQueens, "contentsKnownQueens");
            Scribe_Values.Look(ref this.tickCounter, "tickCounter");
            Scribe_Values.Look(ref this.BeehouseIsFull, "BeehouseIsFull");
            Scribe_Values.Look(ref this.BeehouseIsRunning, "BeehouseIsRunning");
            Scribe_Values.Look(ref this.BeehouseIsExpectingBees, "BeehouseIsExpectingBees");
            Scribe_Values.Look(ref this.BeehouseIsExpectingQueens, "BeehouseIsExpectingQueens");
            Scribe_Values.Look(ref this.whichPlantNeeds, "whichPlantNeeds");
            Scribe_Values.Look(ref this.flagLight, "flagLight");
            Scribe_Values.Look(ref this.flagTemperature, "flagTemperature");
            Scribe_Values.Look(ref this.flagRain, "flagRain");
            Scribe_Values.Look(ref this.flagPlants, "flagPlants");
            Scribe_Values.Look(ref this.flagPower, "flagPower");
            Scribe_Values.Look(ref this.flagInitializeConditions, "flagInitializeConditions");
            Scribe_Values.Look(ref this.avgTempMin, "avgTempMin");
            Scribe_Values.Look(ref this.avgTempMax, "avgTempMax");
            Scribe_Defs.Look(ref this.theDroneIAmGoingToInsert, "theDroneIAmGoingToInsert");
            Scribe_Defs.Look(ref this.theQueenIAmGoingToInsert, "theQueenIAmGoingToInsert");
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (base.Faction != null && base.Faction.IsPlayer)
            {
                this.contentsKnown = true;
            }
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            map = this.Map;
            foreach (Gizmo g in base.GetGizmos())
            {
                yield return g;
            }
            if (this.BeehouseIsExpectingBees)
            {
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
            }
            else
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

            if (this.BeehouseIsExpectingQueens)
            {
                Command_Action RB_Gizmo_Queens_Waiting = new Command_Action();
                RB_Gizmo_Queens_Waiting.action = delegate
                {


                };
                RB_Gizmo_Queens_Waiting.defaultLabel = "RB_InsertQueens".Translate();
                RB_Gizmo_Queens_Waiting.defaultDesc = "RB_InsertQueensDesc".Translate();
                RB_Gizmo_Queens_Waiting.icon = ContentFinder<Texture2D>.Get("UI/RB_Queens_Waiting", true);
                yield return RB_Gizmo_Queens_Waiting;
            }
            else
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
            if (DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing>() != null)
            {
                yield return new Command_Action
                {
                    action = new Action(this.MakeMatchingGrowZone),
                    hotKey = KeyBindingDefOf.Misc2,
                    defaultDesc = "RB_CommandBeehouseMakeGrowingZoneDesc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing", true),
                    defaultLabel = "CommandSunLampMakeGrowingZoneLabel".Translate()
                };
            }
        }

        public override string GetInspectString()
        {
            string text = base.GetInspectString();
            string strContentDrones;
            string strContentQueens;
            string strPercentProgress;
            string strDaysProgress;
            string strStoppedBecause = "";

            string strToAddSpaceIfElectricityUsed = "";

            if (this.TryGetComp<CompBeeHouse>().GetIsElectricBeehouse)
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

            if (!innerContainerDrones.NullOrEmpty() && !innerContainerQueens.NullOrEmpty())
            {
                //str3 = (((float)tickCounter/240)*100).ToString();
                strPercentProgress = ((float)tickCounter / ((ticksToDays) * CalculateTheTicksAverage())).ToStringPercent();
                strDaysProgress = " (aprox " + CalculateTheTicksAverage().ToString("N1") + " days)";
                if (flagInitializeConditions)
                {
                    if (!flagPower)
                    {
                        strStoppedBecause = "\n" + "RB_BeehouseNoPower".Translate();
                    }
                    else
                    if (!flagLight)
                    {
                        strStoppedBecause = "\n" + "RB_BeehouseCombNoProgressNight".Translate();
                    }
                    else
                    if (!flagRain)
                    {
                        strStoppedBecause = "\n" + "RB_BeehouseCombNoProgressRain".Translate();
                    }
                    else
                    if (!flagTemperature)
                    {
                        strStoppedBecause = "\n" + "RB_BeehouseCombNoProgressTemperatureRange".Translate(avgTempMin.Named("MIN"), avgTempMax.Named("MAX"));
                    }
                    else
                    if (!flagPlants)
                    {
                        strStoppedBecause = "\n" + "RB_BeehouseCombNoProgressPlants".Translate(whichPlantNeeds.Named("PLANT"));
                    }
                }
            }
            else
            {
                strPercentProgress = "RB_BeehouseCombNoProgress".Translate();
                strDaysProgress = "";
            }

            return text + strToAddSpaceIfElectricityUsed + "RB_BeehouseContainsDrone".Translate() + ": " + strContentDrones.CapitalizeFirst()
                + "      " + "RB_BeehouseContainsQueen".Translate() + ": " + strContentQueens.CapitalizeFirst() + "\n" +
                "RB_BeehouseCombProgress".Translate() + ": " + strPercentProgress + strDaysProgress + strStoppedBecause;
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
            tickCounter = 0;
            this.TickRare();
        }

        public virtual void EjectContentsQueens()
        {
            this.innerContainerQueens.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null);
            this.contentsKnownQueens = true;
            flagInitializeConditions = false;
            tickCounter = 0;
            this.TickRare();
        }


        public override void TickRare()
        {
            if (BeehouseIsExpectingBees)
            {
                ticksToResetJobs--;
                if (ticksToResetJobs <= 0)
                {
                    ticksToResetJobs = 50;
                    this.BeehouseIsExpectingBees = false;
                    this.BeehouseIsExpectingQueens = false;
                }
            }

            base.TickRare();

            if (innerContainerDrones.NullOrEmpty() || innerContainerQueens.NullOrEmpty())
            {
                BeehouseIsRunning = false;

                return;
            }

            flagInitializeConditions = true;
            if (BeehouseIsFull)
            {
                return;
            }

            if (!CheckPower() || !CheckLightLevels() || !CheckRainLevels() || !CheckTemperatureLevels() || !CheckPlantsNearby())
            {
                BeehouseIsRunning = false;

                return;
            }

            BeehouseIsRunning = true;
            tickCounter++;
            if (tickCounter > (ticksToDays * CalculateTheTicksAverage()) - 1)
            {
                SignalBeehouseFull();
            }
        }

        public bool CheckPower()
        {
            if (!this.TryGetComp<CompBeeHouse>().GetIsElectricBeehouse)
            {
                flagPower = true;
                return true;
            }

            CompPowerTrader power = this.GetComp<CompPowerTrader>();
            flagPower = power.PowerOn;
            return flagPower;
        }


        public bool CheckLightLevels()
        {
            bool bee1nocturnal = innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetNocturnal;
            bool bee2nocturnal = innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetNocturnal;
            if (bee1nocturnal || bee2nocturnal || RimBees_Settings.RB_IgnoreNight)
            {
                flagLight = true;
                return true;
            }

            int currentHour = GenLocalDate.HourInteger(this.Map);
            flagLight = currentHour >= 5 && currentHour <= 22;
            return flagLight;
        }

        public bool CheckRainLevels()
        {
            bool bee1pluviophile = innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetPluviophile;
            bool bee2pluviophile = innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetPluviophile;
            if (bee1pluviophile || bee2pluviophile || RimBees_Settings.RB_IgnoreRain)
            {
                flagRain = true;
                return true;
            }

            flagRain = this.Map.weatherManager.curWeather.rainRate <= 0;
            return flagRain;
        }

        public bool CheckTemperatureLevels()
        {
            if (this.TryGetComp<CompBeeHouse>().GetIsClimatizedBeehouse || RimBees_Settings.RB_IgnoreTemperature)
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

            float currentTempInMap;
            if (RimBees_Settings.RB_GreenhouseBees)
            {
                currentTempInMap = this.Position.GetTemperature(this.Map);
            }
            else
            {
                currentTempInMap = this.Map.mapTemperature.OutdoorTemp;
            }

            flagTemperature = currentTempInMap >= avgTempMin && currentTempInMap <= avgTempMax;
            return flagTemperature;
        }

        public bool CheckPlantsNearby()
        {
            var bee1plantNeeded = innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetWeirdPlant;
            var bee2plantNeeded = innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetWeirdPlant;

            if ((bee1plantNeeded == null && bee2plantNeeded == null) || RimBees_Settings.RB_IgnorePlants)
            {
                flagPlants = true;
                return true;
            }

            whichPlantNeeds = (bee1plantNeeded ?? bee2plantNeeded).label;

            foreach (var c in GrowableCells)
            {
                var plantList = c.GetThingList(this.Map);
                for (int i = 0; i < plantList.Count; i++)
                {
                    if (plantList[i].def == bee1plantNeeded || plantList[i].def == bee2plantNeeded)
                    {
                        flagPlants = true;
                        return true;
                    }
                }
            }

            flagPlants = false;
            return false;
        }

        public void SignalBeehouseFull()
        {
            BeehouseIsFull = true;
        }

        public float CalculateTheTicksAverage()
        {
            if (!innerContainerDrones.NullOrEmpty() && !innerContainerQueens.NullOrEmpty())
            {
                float extraRate = this.TryGetComp<CompBeeHouse>().GetBeehouseRate;

                float bee1ticks = innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetCombtimedays;
                float bee2ticks = innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetCombtimedays;
                float beeticksaverage = extraRate * ((bee1ticks + bee2ticks) / 2);
                return beeticksaverage;
            }

            return 0;
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            EjectContents();
            EjectContentsQueens();
            base.Destroy(mode);
        }

        private void MakeMatchingGrowZone()
        {
            Designator designator = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing>();
            designator.DesignateMultiCell(from tempCell in GrowableCells
                                          where designator.CanDesignateCell(tempCell).Accepted
                                          select tempCell);
        }

        public IEnumerable<IntVec3> GrowableCells
        {
            get
            {
                return GenRadial.RadialCellsAround(this.Position, 6, true);
            }
        }
    }
}
