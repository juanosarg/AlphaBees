using System.Collections.Generic;
using RimWorld;
using Verse;

namespace RimBees
{
    public class Building_HybridizationChamber : Building
    {
        public int tickCounter = 0;
        public int ticksToDays = 240;
        public int daysTotal = 3;
        public int numOfCombinationsFromXML = 1;
        public BeeSpeciesDef hybridizedBee;
        public bool hybridizationChamberFull = false;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            RandomizeDays();
        }

        public void RandomizeDays()
        {
            daysTotal = Rand.Range(1, 4);
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref this.hybridizationChamberFull, "hybridizationChamberFull");
            Scribe_Values.Look(ref this.tickCounter, "tickCounter");
            Scribe_Defs.Look(ref this.hybridizedBee, "hybridizedBee");

        }


        public Building_Beehouse GetAdjacentBeehouse()
        {
            Building_Beehouse result;

            IntVec3 c = this.Position + GenAdj.CardinalDirections[1];
            var edifice = c.GetEdifice(base.Map) as Building_Beehouse;
            if ((edifice != null) && (edifice.TryGetComp<CompBeeHouse>().GetIsBeehouse))
            {
                result = edifice;
                return result;
            }

            result = null;
            return result;
        }

        public override string GetInspectString()
        {

            if (GetAdjacentBeehouse() != null)
            {
                if (GetAdjacentBeehouse().BeehouseIsRunning)
                {
                    return "GU_AdjacentBeehouseRunningHybridization".Translate() + "\n" + "GU_BroodChamberProgress".Translate() + " " + "GU_HybridMutationsRunning".Translate();
                }
                else return "GU_AdjacentBeehouseInactive".Translate() + "\n" + "GU_BroodChamberProgress".Translate() + " " + "GU_HybridMutationsStopped".Translate();

            }
            else return "GU_NoAdjacentBeehouseHybridization".Translate();
        }

        public override void TickRare()
        {
            base.TickRare();
            if (GetAdjacentBeehouse() != null && GetAdjacentBeehouse().BeehouseIsRunning && !hybridizationChamberFull)
            {
                tickCounter++;
                if (tickCounter > ((ticksToDays * daysTotal) - 1))
                {
                    hybridizedBee = HybridizationChecker();
                    if (hybridizedBee != null)
                    {
                        if (!RimBees_Settings.RB_DisableMessages)
                        {
                            switch (numOfCombinationsFromXML)
                            {
                                case 1:
                                    Messages.Message("RB_Hybrid".Translate(hybridizedBee.LabelCap), this, MessageTypeDefOf.TaskCompletion);
                                    break;

                                default:
                                    Messages.Message("RB_HybridNumberAlert".Translate(numOfCombinationsFromXML - 1, hybridizedBee.LabelCap), this, MessageTypeDefOf.TaskCompletion);
                                    break;
                            }
                        }

                        SignalHybridizationChamberFull();
                        RandomizeDays();
                    }
                    else
                    {
                        Messages.Message("RB_NoHybrid".Translate(), this, MessageTypeDefOf.NegativeEvent);
                        tickCounter = 0;
                        RandomizeDays();
                    }
                }
            }
        }

        public void SignalHybridizationChamberFull()
        {
            hybridizationChamberFull = true;
        }

        public BeeSpeciesDef HybridizationChecker()
        {
            BeeSpeciesDef beeDrone = null;
            BeeSpeciesDef beeQueen = null;
            if ((this.GetAdjacentBeehouse().innerContainerDrones.TotalStackCount > 0) && (this.GetAdjacentBeehouse().innerContainerQueens.TotalStackCount > 0))
            {
                beeDrone = this.GetAdjacentBeehouse().innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetSpecies;
                beeQueen = this.GetAdjacentBeehouse().innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetSpecies;
            }
            foreach (BeeCombinationDef element in DefDatabase<BeeCombinationDef>.AllDefs)
            {
                if ((beeDrone == element.bee1 && beeQueen == element.bee2) || (beeDrone == element.bee2 && beeQueen == element.bee1))
                {
                    numOfCombinationsFromXML = element.result.Count;
                    var result = element.result.RandomElement();
                    GameComponent_KnownBees.Instance.LogAttempt(beeQueen, beeDrone, result);
                    return result;
                }
            }

            GameComponent_KnownBees.Instance.LogAttempt(beeQueen, beeDrone, null);
            return null;
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }
            if (Prefs.DevMode)
            {
                Command_Action command_Action = new Command_Action();
                command_Action.defaultLabel = "Finish operation";
                command_Action.action = delegate
                {
                    tickCounter = ticksToDays * daysTotal;
                };
                yield return command_Action;
            }
        }

    }
}
