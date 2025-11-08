using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace RimBees
{
    public class Building_BeeLightSource: Building
    {

        public const int tickCounterToDelete = 10000;
        public int tickCounter = tickCounterToDelete;

        protected override void Tick()
        {
            base.Tick();

            tickCounter--;
            if (tickCounter <= 0) {
                this.Destroy();
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.tickCounter, "tickCounter", tickCounterToDelete, false);
        }

    }
}
