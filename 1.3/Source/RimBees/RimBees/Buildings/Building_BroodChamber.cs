using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimBees
{
    public class Building_BroodChamber : Building
    {
        public int tickCounter = 0;
        public int ticksToDays = 240;
        public int daysTotal = 3;
        public bool broodChamberFull = false;

        public override void ExposeData()
        {
            base.ExposeData();
       
            Scribe_Values.Look<bool>(ref this.broodChamberFull, "broodChamberFull", false, false);
            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounter", 0, false);
        }

        public Building_Beehouse GetAdjacentBeehouse()
        {
            var c = this.Position + IntVec3.West;
            var edifice = c.GetEdifice(base.Map) as Building_Beehouse;
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
                text.Append("GU_NoAdjacentBeehouse".Translate());
                return text.ToString();
            }

            text.Append(beehouse.BeehouseIsRunning ? "GU_AdjacentBeehouseRunning".Translate() : "GU_AdjacentBeehouseInactive".Translate());
            text.Append("GU_BroodChamberProgress".Translate()).Append(" ");
            text.Append(((float)tickCounter / (ticksToDays * daysTotal)).ToStringPercent());

            if (!beehouse.BeehouseIsRunning)
            {
                text.Append(" ").Append("GU_BroodChamberStopped".Translate());
            }

            return text.ToString();
        }

        public override void TickRare()
        {
            base.TickRare();

            if (GetAdjacentBeehouse()?.BeehouseIsRunning == true && !broodChamberFull)
            {
                tickCounter++;
                if (tickCounter > (ticksToDays * daysTotal) - 1)
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
