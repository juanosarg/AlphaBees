

using Verse;
using System.Collections.Generic;
using UnityEngine;

using System.Diagnostics;



namespace RimBees
{
    class Building_BroodChamber : Building
    {

        public int tickCounter = 0;
        public int ticksToDays = 240;
        public bool broodChamberFull = false;

        public Building_Beehouse cachedBeehouse = null;

        public override void ExposeData()
        {
            base.ExposeData();
       
            Scribe_Values.Look<bool>(ref this.broodChamberFull, "broodChamberFull", false, false);
            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounter", 0, false);
        }

        public Building_Beehouse GetAdjacentBeehouse
        {
            get
            {
                if(cachedBeehouse is null)
                {                  
                    IntVec3 c = this.Position + GenAdj.CardinalDirections[3];
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
            string text = base.GetInspectString();

            if (GetAdjacentBeehouse != null)
            {
                string strPercentProgress = ((float)tickCounter / ((ticksToDays) * RimBees_Settings.broodChamberMultiplier)).ToStringPercent();

                if (GetAdjacentBeehouse.BeehouseIsRunning) {

                    return text + "GU_AdjacentBeehouseRunning".Translate() + "\n" + "GU_BroodChamberProgress".Translate()+" "+ strPercentProgress;

                } else return text + "GU_AdjacentBeehouseInactive".Translate() + "\n" + "GU_BroodChamberProgress".Translate() + " " + strPercentProgress +" (stopped)";

            }
            else return text+"GU_NoAdjacentBeehouse".Translate();
        }

        public override void TickRare()
        {
            base.TickRare();
            if (GetAdjacentBeehouse != null && GetAdjacentBeehouse.BeehouseIsRunning && !broodChamberFull)
            {
                tickCounter++;
                if (tickCounter > ((ticksToDays * RimBees_Settings.broodChamberMultiplier) - 1))
                {
                    SignalBroodChamberFull();
                }
            }

        }

        public void SignalBroodChamberFull()
        {

            broodChamberFull = true;
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
                    tickCounter = (int)(ticksToDays * RimBees_Settings.broodChamberMultiplier);
                };
                yield return command_Action;
            }
        }


    }
}
