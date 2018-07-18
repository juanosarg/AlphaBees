

using Verse;
using UnityEngine;
using RimWorld;

using System;


namespace RimBees
{
    class Building_HybridizationChamber : Building
    {

        public int tickCounter = 0;
        public int ticksToDays = 240;
        public int daysTotal = 3;
        public string hybridizedBee = "";
        public bool hybridizationChamberFull = false;
        private System.Random rand = new System.Random();

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            daysTotal =  rand.Next(1, 4);
        }

        public void RandomizeDays()
        {
            daysTotal = rand.Next(1, 4);
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look<bool>(ref this.hybridizationChamberFull, "hybridizationChamberFull", false, false);
            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounter", 0, false);
            Scribe_Values.Look<string>(ref this.hybridizedBee, "hybridizedBee", "", false);

        }


        public Building_Beehouse GetAdjacentBeehouse()
        {
            Building_Beehouse result;
           
                
                IntVec3 c = this.Position+ GenAdj.CardinalDirections[1];
                Building_Beehouse edifice = (Building_Beehouse)c.GetEdifice(base.Map);
                if (edifice != null && edifice.def == DefDatabase<ThingDef>.GetNamed("RB_Beehouse", true))
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
                if (GetAdjacentBeehouse().BeehouseIsRunning) {
                    return "GU_AdjacentBeehouseRunningHybridization".Translate() + "\n" + "GU_BroodChamberProgress".Translate() + " " + "GU_HybridMutationsRunning".Translate();
                } else return "GU_AdjacentBeehouseInactive".Translate() + "\n" + "GU_BroodChamberProgress".Translate() + " " + "GU_HybridMutationsStopped".Translate();

            }
            else return "GU_NoAdjacentBeehouseHybridization".Translate();
        }

        public override void TickRare()
        {
            base.TickRare();
            if (GetAdjacentBeehouse().BeehouseIsRunning && !hybridizationChamberFull)
            {
                tickCounter++;
                if (tickCounter > ((ticksToDays * daysTotal) - 1))
                {
                    hybridizedBee = HybridizationChecker();
                    if (hybridizedBee != "")
                    {
                        Messages.Message("RB_Hybrid".Translate(), this, MessageTypeDefOf.NegativeEvent);
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
            string beeDrone = this.GetAdjacentBeehouse().innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetSpecies;
            string beeQueen = this.GetAdjacentBeehouse().innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetSpecies;

            if (beeDrone == "Temperate")
            {
                
                if (beeQueen == "Mild")
                {
                    return "Hybrid";
                }
                if (beeQueen == "Hybrid")
                {
                    return "Amalgam";
                }
            }

            if (beeDrone == "Mild") {
                if (beeQueen == "Temperate")
                {
                    return "Hybrid";
                }

                if (beeQueen == "Hybrid")
                {
                    return "Amalgam";
                }
            }

            if (beeDrone == "Hybrid")
            {
                if (beeQueen == "Temperate")
                {
                    return "Amalgam";
                }
                if (beeQueen == "Mild")
                {
                    return "Amalgam";
                }
             
            }


            return "";
         }
    }
}
