using System.Collections.Generic;
using System.Text;
using RimWorld;
using Verse;

namespace RimBees
{
    public class Building_HybridizationChamber : Building
    {
        public int tickCounter = 0;
        public const int ticksToDays = 240;
        public int daysTotal = 0;
        public int numOfCombinationsFromXML = 1;
        public BeeSpeciesDef hybridizedBee;
        public bool hybridizationChamberFull = false;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            if (daysTotal == 0)
            {
                RandomizeDays();
            }
        }

        public void RandomizeDays()
        {
            daysTotal = Rand.Range(1, 4);
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref this.hybridizationChamberFull, "hybridizationChamberFull");
            Scribe_Values.Look(ref this.daysTotal, "daysTotal");
            Scribe_Values.Look(ref this.tickCounter, "tickCounter");
            Scribe_Values.Look(ref this.numOfCombinationsFromXML, "numOfCombinationsFromXML");
            Scribe_Defs.Look(ref this.hybridizedBee, "hybridizedBee");
        }

        public Building_Beehouse GetAdjacentBeehouse()
        {
            var c = this.Position + IntVec3.East;
            var edifice = c.GetEdifice(this.Map) as Building_Beehouse;
            if (edifice?.TryGetComp<CompBeeHouse>()?.GetIsBeehouse == true)
            {
                return edifice;
            }

            return null;
        }

        public override string GetInspectString()
        {
            var text = new StringBuilder(base.GetInspectString());
            text.AppendLineIfNotEmpty();

            var beehouse = GetAdjacentBeehouse();
            if (beehouse == null)
            {
                text.Append("GU_NoAdjacentBeehouseHybridization".Translate());
                return text.ToString();
            }

            if (hybridizationChamberFull)
            {
                text.Append(GetHybridizedBeeMessage());
            }
            else
            {
                text.Append(beehouse.BeehouseIsRunning ? "GU_AdjacentBeehouseRunningHybridization".Translate() : "GU_AdjacentBeehouseInactive".Translate());
                text.AppendLine();
                text.Append("GU_HybridMutationsProgress".Translate(((float)tickCounter / ticksToDays).ToString("N1").Named("DAYS")));
            }

            return text.ToString();
        }

        public override void TickRare()
        {
            base.TickRare();
            if (GetAdjacentBeehouse()?.BeehouseIsRunning == true && !hybridizationChamberFull)
            {
                tickCounter++;
                if (tickCounter > ((ticksToDays * daysTotal) - 1))
                {
                    var messageType = MessageTypeDefOf.TaskCompletion;

                    hybridizedBee = HybridizationChecker();
                    if (hybridizedBee != null)
                    {
                        SignalHybridizationChamberFull();
                    }
                    else
                    {
                        tickCounter = 0;
                        messageType = MessageTypeDefOf.NegativeEvent;
                    }

                    RandomizeDays();

                    if (!RimBees_Settings.RB_DisableMessages)
                    {
                        Messages.Message(GetHybridizedBeeMessage(), this, messageType);
                    }
                }
            }
        }

        public string GetHybridizedBeeMessage()
        {
            if (hybridizedBee == null)
            {
                return "RB_NoHybrid".Translate();
            }

            if (numOfCombinationsFromXML == 1)
            {
                return "RB_Hybrid".Translate(hybridizedBee.LabelCap);
            }

            return "RB_HybridNumberAlert".Translate(numOfCombinationsFromXML - 1, hybridizedBee.LabelCap);
        }

        public void SignalHybridizationChamberFull()
        {
            hybridizationChamberFull = true;
        }

        public BeeSpeciesDef HybridizationChecker()
        {
            var beehouse = GetAdjacentBeehouse();
            var drone = beehouse?.innerContainerDrones.FirstOrFallback()?.TryGetComp<CompBees>()?.GetSpecies;
            var queen = beehouse?.innerContainerQueens.FirstOrFallback()?.TryGetComp<CompBees>()?.GetSpecies;

            foreach (var combo in DefDatabase<BeeCombinationDef>.AllDefs)
            {
                if ((drone == combo.bee1 && queen == combo.bee2) || (drone == combo.bee2 && queen == combo.bee1))
                {
                    numOfCombinationsFromXML = combo.result.Count;
                    var result = combo.result.RandomElement();
                    GameComponent_KnownBees.Instance.LogAttempt(queen, drone, result);
                    return result;
                }
            }

            numOfCombinationsFromXML = 0;
            GameComponent_KnownBees.Instance.LogAttempt(queen, drone, null);
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
                yield return new Command_Action
                {
                    defaultLabel = "Finish operation",
                    action = delegate
                    {
                        tickCounter = ticksToDays * daysTotal;
                        this.TickRare();
                    }
                };
            }
        }
    }
}
