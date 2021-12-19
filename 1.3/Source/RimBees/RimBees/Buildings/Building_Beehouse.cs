using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            foreach (Gizmo g in base.GetGizmos())
            {
                yield return g;
            }

            if (this.BeehouseIsExpectingBees)
            {
                yield return new Command_Action
                {
                    action = delegate { },
                    disabled = true,
                    defaultLabel = "RB_InsertBees".Translate(),
                    defaultDesc = "RB_InsertBeesDesc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/RB_Drones_Waiting", true)
                };
            }
            else if (innerContainerDrones.NullOrEmpty())
            {
                yield return BeeListSetupUtility.SetBeeListCommand(this);
            }
            else
            {
                yield return new Command_Action
                {
                    action = delegate
                    {
                        this.EjectContents();
                    },
                    defaultLabel = "RB_ExtractBees".Translate(),
                    defaultDesc = "RB_ExtractBeesDesc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/RB_ExtractDrones_FromBeehouse", true)
                };
            }

            if (this.BeehouseIsExpectingQueens)
            {
                yield return new Command_Action
                {
                    action = delegate { },
                    disabled = true,
                    defaultLabel = "RB_InsertQueens".Translate(),
                    defaultDesc = "RB_InsertQueensDesc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/RB_Queens_Waiting", true)
                };
            }
            else if (innerContainerQueens.NullOrEmpty())
            {
                yield return BeeListSetupUtility.SetQueenListCommand(this);
            }
            else
            {
                yield return new Command_Action
                {
                    action = delegate
                    {
                        this.EjectContentsQueens();
                    },
                    defaultLabel = "RB_ExtractQueens".Translate(),
                    defaultDesc = "RB_ExtractQueensDesc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/RB_ExtractQueens_FromBeehouse", true)
                };
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
            var text = new StringBuilder(base.GetInspectString());
            text.AppendLineIfNotEmpty();

            text.Append("RB_BeehouseContainsDrone".Translate()).Append(": ");

            var drone = innerContainerDrones.FirstOrFallback();
            if (drone == null)
            {
                text.Append("RB_BeehouseNonePresent".Translate());
            }
            else
            {
                text.Append(drone.def.LabelCap);
            }

            text.Append("      ").Append("RB_BeehouseContainsQueen".Translate()).Append(": ");

            var queen = innerContainerQueens.FirstOrFallback();
            if (drone == null)
            {
                text.Append("RB_BeehouseNonePresent".Translate());
            }
            else
            {
                text.Append(queen.def.LabelCap);
            }

            text.AppendLine();

            if (drone == null || queen == null)
            {
                text.Append("RB_BeehouseCombNoProgress".Translate());
            }
            else
            {
                var avgTicks = CalculateTheTicksAverage();
                text.Append("RB_BeehouseCombProgress2".Translate((tickCounter / (ticksToDays * avgTicks)).ToStringPercent().Named("PERCENT"), avgTicks.ToString("N1").Named("DAYS")));

                if (flagInitializeConditions)
                {
                    if (!flagPower)
                    {
                        text.AppendLine().Append("RB_BeehouseNoPower".Translate());
                    }
                    else if (!flagLight)
                    {
                        text.AppendLine().Append("RB_BeehouseCombNoProgressNight".Translate());
                    }
                    else if (!flagRain)
                    {
                        text.AppendLine().Append("RB_BeehouseCombNoProgressRain".Translate());
                    }
                    else if (!flagTemperature)
                    {
                        text.AppendLine().Append("RB_BeehouseCombNoProgressTemperatureRange".Translate(avgTempMin.Named("MIN"), avgTempMax.Named("MAX")));
                    }
                    else if (!flagPlants)
                    {
                        text.AppendLine().Append("RB_BeehouseCombNoProgressPlants".Translate(whichPlantNeeds.Named("PLANT")));
                    }
                }
            }

            return text.ToString();
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
            var designator = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing>();
            var actuallyGrowableCells = GrowableCells.Where(c => designator.CanDesignateCell(c));
            designator.DesignateMultiCell(actuallyGrowableCells);
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
