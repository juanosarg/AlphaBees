

using Verse;
using UnityEngine;
using RimWorld;
using System.Collections.Generic;
using System;


namespace RimBees
{
    class Building_HybridizationChamber : Building
    {

        public int tickCounter = 0;
        public int ticksToDays = 240;//240;
        public float daysTotal = 3;
        public int numOfCombinationsFromXML = 1;
        public string hybridizedBee = "";
        public bool hybridizationChamberFull = false;
        private System.Random rand = new System.Random();
        private System.Random beeRandomizer = new System.Random();

        public Building_Beehouse cachedBeehouse = null;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            daysTotal = rand.Next(1, 4) * RimBees_Settings.hybridizationChamberMultiplier;
        }

        public void RandomizeDays()
        {
            daysTotal = rand.Next(1, 4) * RimBees_Settings.hybridizationChamberMultiplier;
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look<bool>(ref this.hybridizationChamberFull, "hybridizationChamberFull", false, false);
            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounter", 0, false);
            Scribe_Values.Look<string>(ref this.hybridizedBee, "hybridizedBee", "", false);

        }

        public Building_Beehouse GetAdjacentBeehouse
        {
            get
            {
                if (cachedBeehouse is null)
                {
                    IntVec3 c = this.Position + GenAdj.CardinalDirections[1];
                    Building_Beehouse edifice = (Building_Beehouse)c.GetEdifice(base.Map);
                    if ((edifice != null) && (edifice.TryGetComp<CompBeeHouse>().GetIsBeehouse))
                    {
                        cachedBeehouse = edifice;

                    }
                }
                return cachedBeehouse;
            }
        }

     

        public override string GetInspectString()
        {

            if (GetAdjacentBeehouse != null)
            {
                if (GetAdjacentBeehouse.BeehouseIsRunning) {
                    return "GU_AdjacentBeehouseRunningHybridization".Translate() + "\n" + "GU_BroodChamberProgress".Translate() + " " + "GU_HybridMutationsRunning".Translate(RimBees_Settings.hybridizationChamberMultiplier.ToString(), (RimBees_Settings.hybridizationChamberMultiplier*3).ToString());
                } else return "GU_AdjacentBeehouseInactive".Translate() + "\n" + "GU_BroodChamberProgress".Translate() + " " + "GU_HybridMutationsStopped".Translate(RimBees_Settings.hybridizationChamberMultiplier.ToString(), (RimBees_Settings.hybridizationChamberMultiplier * 3).ToString());

            }
            else return "GU_NoAdjacentBeehouseHybridization".Translate();
        }

        public override void TickRare()
        {
            base.TickRare();
            if (GetAdjacentBeehouse!=null&&GetAdjacentBeehouse.BeehouseIsRunning && !hybridizationChamberFull)
            {
                tickCounter++;
                if (tickCounter > ((ticksToDays * daysTotal) - 1))
                {
                    hybridizedBee = HybridizationChecker();

                    if (hybridizedBee != "")
                    {
                        if (!RimBees_Settings.RB_DisableMessages)
                        {
                            switch (numOfCombinationsFromXML)
                            {
                                case 1:
                                    Messages.Message("RB_Hybrid".Translate(hybridizedBee), this, MessageTypeDefOf.NegativeEvent);
                                    break;

                                default:
                                    Messages.Message("RB_HybridNumberAlert".Translate(numOfCombinationsFromXML - 1, hybridizedBee), this, MessageTypeDefOf.NegativeEvent);
                                    break;

                            }
                        }
                        

                        SignalHybridizationChamberFull();
                        RandomizeDays();

                    }
                    else {
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

        public string HybridizationChecker()
        {
            string beeDrone = "";
            string beeQueen = "";
            if ((this.GetAdjacentBeehouse.innerContainerDrones.TotalStackCount>0)&& (this.GetAdjacentBeehouse.innerContainerQueens.TotalStackCount > 0)){
                beeDrone = this.GetAdjacentBeehouse.innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetSpecies;
                beeQueen = this.GetAdjacentBeehouse.innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetSpecies;
            }
            foreach (BeeCombinationDef element in DefDatabase<BeeCombinationDef>.AllDefs)
            {
               if((beeDrone==element.bee1&&beeQueen==element.bee2)|| (beeDrone == element.bee2 && beeQueen == element.bee1))
                {
                        numOfCombinationsFromXML = element.result.Count;
                        string result = element.result.RandomElement();
                        GameComponent_KnownBees.Instance.LogAttempt(beeQueen, beeDrone, result);
                        return result;
                        
                }

            }

            string resultNull = "";
            GameComponent_KnownBees.Instance.LogAttempt(beeQueen, beeDrone,  null );
            return resultNull;
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
                    tickCounter = (int)(ticksToDays * daysTotal);
                };
                yield return command_Action;
            }
        }

    }
}
